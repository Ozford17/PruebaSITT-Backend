using Backend_SITT_Api.Aplication.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend_SITT_Api.Aplication.Contracts.Identity
{
    public interface IAuthService
    {
        Task<AuthResponseLogin> Login(AuthRequest request);
        (string, Guid)? ValidateToken(string token);
        Task<AuthResponse> Register(RegistrationRequest request);

        Task<Object> GetUserById(Guid userId);
         

    }
}
