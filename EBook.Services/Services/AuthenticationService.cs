using System.Linq;
using System.Threading.Tasks;
using EBook.Data.Entities;
using EBook.Services.Jwt;
using EBook.Services.Models;
using EBook.Services.Models.Dtos;
using EBook.Services.Models.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace EBook.Services.Services
{
    public class AuthenticationService
    {
        public const string PasswordOrUsernameIncorrect = "The username or password is incorrect.";

        private readonly SignInManager<User> _signInManager;
        private readonly JwtTokenFactory _jwtTokenFactory;

        public AuthenticationService(SignInManager<User> signInManager, JwtTokenFactory jwtTokenFactory)
        {
            _signInManager = signInManager;
            _jwtTokenFactory = jwtTokenFactory;
        }

        public async Task<IRequestResult> RegisterAsync(UserDto userModel)
        {
            var userExists = await _signInManager.UserManager.FindByNameAsync(userModel.Login);
            if (userExists != null)
            {
                return new RegisterResult
                { Success = false, Message = $"User with login {userModel.Login} already exists" };
            }

            var user = new User
            {
                Login = userModel.Login,
                Email = userModel.Email,
                FirstName = userModel.FirstName,
                LastName = userModel.LastName
            };

            var result = await _signInManager.UserManager.CreateAsync(user, userModel.Password);
            if (!result.Succeeded)
            {
                return new RegisterResult
                {
                    Success = false,
                    Message = string.Join(";", result.Errors.Select(x => x.Description))
                };
            }
            return new RegisterResult { Success = true, Message = "User registered successfully!" };
        }

        public async Task<IRequestResult> LoginAsync(UserDto userModel)
        {
            var user = await _signInManager.UserManager.FindByNameAsync(userModel.Login);
            if (user == null)
            {
                return new LoginResult { Success = false, Message = PasswordOrUsernameIncorrect };
            }
            var signinResult = await _signInManager.PasswordSignInAsync(user, userModel.Password, true, false);
            if (!signinResult.Succeeded)
            {
                return new LoginResult
                {
                    Message = PasswordOrUsernameIncorrect,
                    Success = false
                };
            }
            return new LoginResult
            {
                Success = true,
                Token = _jwtTokenFactory.GenerateJwtToken(user)
            };
        }
    }
}
