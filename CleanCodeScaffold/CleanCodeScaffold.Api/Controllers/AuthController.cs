using CleanCodeScaffold.Api.Util;
using CleanCodeScaffold.Application.Dtos;
using CleanCodeScaffold.Application.Handlers.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CleanCodeScaffold.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserHandler _userHandler;
        public AuthController(IUserHandler userHandler)
        {
            _userHandler = userHandler;
        }

        [HttpPost("token")]
        public async Task<IActionResult> Token(LoginVM model)
        {
            var data = await _userHandler.GetToken(model);
            return data.ToResponse();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            var data = await _userHandler.Register(model);
            return data.ToResponse();
        }
        [HttpGet("refresh")]
        public async Task<IActionResult> Refresh(string refreshToken)
        {
            var data = await _userHandler.GetTokenByRefresh(refreshToken);
            return data.ToResponse();
        }

        [Authorize]
        [HttpPost("changepassword")]
        public async Task<IActionResult> changepassword(ChangePasswordVM model)
        {
            var data = await _userHandler.ChangePassword(User.GetId(), model);
            return data.ToResponse();
        }

        [HttpGet("forgotpassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var data = await _userHandler.GetForgotPasswordToken(email);
            return data.ToResponse();
        }
        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPasasword(ResetPasswordVM model)
        {
            var data = await _userHandler.ResetPassword(model);
            return data.ToResponse();
        }
    }
}
