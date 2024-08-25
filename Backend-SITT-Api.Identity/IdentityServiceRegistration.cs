using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Backend_SITT_Api.Identity.Models;
using Backend_SITT_Api.Identity.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Backend_SITT_Api.Identity.Schemes;
using Backend_SITT_Api.Aplication.Contracts.Identity;
using Backend_SITT_Api.Identity.Services;
using Microsoft.AspNetCore.Http;
using Backend_SITT_Api.Aplication.Models.Identity;

namespace Backend_SITT_Api.Identity
{
    public static class IdentityServiceRegistration
    {
        public static IServiceCollection ConfigureIdentityServiceRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JWTSettings>(configuration.GetSection("JwtSettings"));
            services.AddDbContext<SITTIdentityDbContext>(options =>
            {

                options.UseSqlServer(configuration.GetConnectionString("IdentityConnectionString"), b => b.MigrationsAssembly(typeof(SITTIdentityDbContext).Assembly.FullName));
                  
            }, ServiceLifetime.Scoped, ServiceLifetime.Singleton);

            services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<SITTIdentityDbContext>();


            services.AddScoped<IAuthService, AuthService>();


            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["JwtSettings:Key"])),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                RequireExpirationTime = false,
                ClockSkew = TimeSpan.Zero
            };
            services.AddSingleton(tokenValidationParameters);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = tokenValidationParameters;
                options.Events = new()
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        // If the request is for our hub...
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) )
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            }).AddScheme<AuthenticationSchemeOptions, CustomAuthenticationScheme>(CustomAuthenticationScheme.CustomScheme, _ =>
            {
            });

            services.AddAuthorization(opt =>
            {
                AuthorizationPolicyBuilder policyBuilder = new AuthorizationPolicyBuilder();
                foreach (string scheme in opt.DefaultPolicy.AuthenticationSchemes)
                    policyBuilder.AuthenticationSchemes.Add(scheme);

                foreach (var requirement in opt.DefaultPolicy.Requirements)
                    policyBuilder.Requirements.Add(requirement);

                policyBuilder.RequireAssertion(async context =>
                {
                    if (context.Resource is HttpContext http)
                    {
                        SITTIdentityDbContext db = http.RequestServices.GetService<SITTIdentityDbContext>() ?? throw new UnauthorizedAccessException("context not found");
                        string userId = context.User.FindAll("Id").FirstOrDefault()?.Value ?? throw new BadHttpRequestException("user context not found");

                        ApplicationUser user = await db.Users.FirstOrDefaultAsync(row => row.Id == userId) ?? throw new BadHttpRequestException("user not found");
                        return true; 
                    }

                    return false;
                });

                opt.DefaultPolicy = policyBuilder.Build();
            });

            services.AddMemoryCache();

            services.AddHttpContextAccessor();
            return services;
        }
    }
}
