using Backend_SITT_Api.Aplication.Common.Errors;
using Backend_SITT_Api.Aplication.Common.Helpers;
using Backend_SITT_Api.Aplication.Contracts.Identity;
using Backend_SITT_Api.Aplication.Models.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace Backend_SITT_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ProducesResponseType(typeof(CodeErrorException), (int)HttpStatusCode.BadRequest)]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseLogin), (int)HttpStatusCode.OK)]
        [ProducesDefaultResponseType]
        public async Task Login([FromBody] AuthRequest request)
        {
            try
            {
                Response.StatusCode = (int)HttpStatusCode.OK;
                await Response.WriteAsJsonAsync(await _authService.Login(request));
            }
            catch (BadRequestException ex)
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await Response.WriteAsJsonAsync(JsonSerializer.Deserialize<JsonElement>(ex.Message));
            }
        }

        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [ProducesResponseType(typeof(CodeErrorException), (int)HttpStatusCode.BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<AuthResponse>> SingIn([FromBody] RegistrationRequest request)
           => Ok(await _authService.Register(request.GetUserData(Request.Headers)));
    }
}
