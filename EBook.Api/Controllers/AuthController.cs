using System.Threading.Tasks;
using EBook.Services.Models;
using EBook.Services.Models.Dtos;
using EBook.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace EBook.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthenticationService _authenticationService;

        public AuthController(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto userModel)
        {
            if (string.IsNullOrEmpty(userModel.Login) ||
                string.IsNullOrEmpty(userModel.Email) ||
                string.IsNullOrEmpty(userModel.Password))
            {
                return BadRequest(new RegisterResult { Message = "Bad registration parameters", Success = false });
            }

            var registerResult = await _authenticationService.RegisterAsync(userModel);
            if (!registerResult.Success)
            {
                return BadRequest(registerResult);
            }
            return Ok(registerResult);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDto userModel)
        {
            var loginResult = await _authenticationService.LoginAsync(userModel);
            return Ok(loginResult);
        }
    }
}
