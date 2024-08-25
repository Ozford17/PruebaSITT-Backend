using Azure.Core;
using Backend_SITT_Api.Aplication.Common;
using Backend_SITT_Api.Aplication.Common.Errors;
using Backend_SITT_Api.Aplication.Contracts.Identity;
using Backend_SITT_Api.Aplication.Models.Identity;
using Backend_SITT_Api.Identity.Models;
using Backend_SITT_Api.Identity.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Backend_SITT_Api.Identity.Services
{
    public  class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly SITTIdentityDbContext _context;
        private readonly JWTSettings _jwtSettings;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, SITTIdentityDbContext context, IOptions<JWTSettings> jwtSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _jwtSettings = jwtSettings.Value;

        }

        public async Task<AuthResponseLogin> Login(AuthRequest request)
        {
             

            var verifiUser = _context.Users.Where(x => x.UserName == request.Username).FirstOrDefault();

            if (verifiUser == null)
                throw new Exception("Authentication Error");

            if (!verifiUser.IsActive)
                throw new BadRequestException("Inactive user");

            var user = await _userManager.FindByNameAsync(request.Username);

            if (user == null) 
                throw new UnauthorizedAccessException("Usuario no encontrado");
         

            // Validar la contraseña
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!isPasswordValid) 
                throw new UnauthorizedAccessException("Contraseña incorrecta");
             
            var result = await _signInManager.PasswordSignInAsync(request.Username, request.Password, isPersistent: false, lockoutOnFailure: true);

            
            if (result.IsLockedOut) 
                throw new UnauthorizedAccessException("Cuenta bloqueada"); 

            if (!result.Succeeded)
                throw new BadRequestException("User and Password invalid");
 
            var token = await GenerateToken(verifiUser);
            var Objuser = new UserData(verifiUser.Id, $"{verifiUser.Name}{" "} {verifiUser.LastName}", request.Username, verifiUser.Email);
             

            var authReponse = new AuthResponseLogin
            {
                AccessToken = token.Item1,
                RefreshToken = token.Item2,
                ConnectionDate = DateTime.Now,
                User = Objuser, 
            };
            return authReponse;
        }

        private async Task<Tuple<string, string>> GenerateToken( ApplicationUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Key));

            var userClaims = await _userManager.GetClaimsAsync(user);





            var claimsIdentity = new[]
            {
                new Claim("Id", user.Id),
                new Claim("UserName", user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new
                Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            }
            .Union(userClaims);
            

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    claimsIdentity
                ),
                Expires = DateTime.UtcNow.Add(_jwtSettings.ExpireTime),
                SigningCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);

            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                IsUsed = false,
                IsRevoked = false,
                UserId = user.Id,
                CreatedDate = DateTime.UtcNow,
                ExpireDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenDurationInDays),
                Token = $"{GenerateRandomTokenCharacters(_jwtSettings.RefreshTokenCharacterLenght)}{Guid.NewGuid()}"
            };

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();

            return new Tuple<string, string>(jwtToken, refreshToken.Token);
        }

        public (string, Guid)? ValidateToken(string token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == "Id").Value);
                var user = jwtToken.Claims.First(x => x.Type == "UserName").Value.ToString();

                // return user id from JWT token if validation successful
                return (user, userId);
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }

        private string GenerateRandomTokenCharacters(int lenght)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, lenght)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        #region User
        public async Task<AuthResponse> Register(RegistrationRequest request)
        {
              
            var existingUser = await _userManager.FindByNameAsync(request.UserName);
            if (existingUser != null)
                throw new BadRequestException($"User with username {request.UserName} already exists");

            var existingEmail = await _userManager.FindByEmailAsync(request.Email);
            if (existingEmail != null)
                throw new Exception($"An account with email {request.Email} already exists");

            var user = new ApplicationUser
            {
                Name = request.Name,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.UserName,
                EmailConfirmed = true,
                IdentificacrionNumber = request.IdentificationNumber,
                FulName = $"{request.Name}{""}{request.Name}"
            };



            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                throw new Exception($"An error occured during user creation for user {request.UserName}");

              
            var token = await GenerateToken(user);
            return new AuthResponse
            {
                UserId = user.Id,
                UserName = user.UserName,
                LastName = user.LastName,
                FirstName = user.Name,
                AccessToken = token.Item1,
                RefreshToken = token.Item2,
                 
            };


            throw new Exception($"{result.Errors}");
        }

        public async Task<Object> GetUserById(Guid userId)
        {
            var user = await _context.Users.Where(x => x.Id == userId.ToString()).FirstOrDefaultAsync();
            if (user == null)
                throw new BadRequestException($"User not found");
             
            return user;
        }
        #endregion
    }
}
