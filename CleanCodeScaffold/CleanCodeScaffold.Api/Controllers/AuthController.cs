using CleanCodeScaffold.Api.Util;
using CleanCodeScaffold.Application.Dtos;
using CleanCodeScaffold.Application.Handlers.Interface;
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
    }
}
