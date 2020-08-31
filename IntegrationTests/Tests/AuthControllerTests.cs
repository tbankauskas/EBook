using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EBook.Services.Models;
using EBook.Services.Models.Dtos;
using EBook.Services.Services;
using IntegrationTests.Builders;
using IntegrationTests.Infrastructure;
using IntegrationTests.Infrastructure.Utilities;
using Xunit;

namespace IntegrationTests.Tests
{
    public class AuthControllerTests : BaseHttpApiTests
    {
        private const string PwdMessage1 = "Passwords must be at least 8 characters.";
        private const string PwdMessage2 = "Passwords must have at least one digit ('0'-'9').";
        private const string PwdMessage3 = "Passwords must have at least one uppercase ('A'-'Z').";
        private const string WarningMessage = "Bad registration parameters";
        private const string RegisterUrl = "/api/auth/register";
        private const string LoginUrl = "/api/auth/login";

        private readonly UserDto _newUser = new UserDto { Email = "aaa@aaa.com", Login = "aaa", Password = "Bbbb1981" };

        [Fact]
        public async Task Register_should_register_user_successfully()
        {
            // Act
            var result = await HttpClient.PostJson<RegisterResult>(RegisterUrl, _newUser);

            // Assert
            Assert.True(result.Success);
        }

        [Fact]
        public async Task Register_should_return_password_validation_errors()
        {
            // Arrange
            _newUser.Password = "ccc";

            // Act
            var result = await HttpClient.PostJson<RegisterResult>(RegisterUrl, _newUser);

            // Assert
            Assert.False(result.Success);
            Assert.Collection(result.Message.Split(";").ToList(),
                x => Assert.Contains(PwdMessage1, x),
                x => Assert.Contains(PwdMessage2, x),
                x => Assert.Contains(PwdMessage3, x)
            );
        }

        [Fact]
        public async Task Register_should_return_bad_request_when_login_is_null()
        {
            // Act
            _newUser.Login = null;
            var response = await HttpClient.PostJson(RegisterUrl, _newUser );
            var result = await response.Deserialize<RegisterResult>();

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.False(result.Success);
            Assert.Equal(WarningMessage, result.Message);
        }

        [Fact]
        public async Task Register_should_return_bad_request_if_user_already_exists()
        {
            // Arrange
            var existingUser = await new UserBuilder().WithLogin("aaa").SaveToDb();

            // Act
            var response = await HttpClient.PostJson(RegisterUrl, _newUser);
            var result = await response.Deserialize<RegisterResult>();

            // Assert
            Assert.False(response.IsSuccessStatusCode);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal($"User with login {existingUser.Login} already exists", result.Message);
        }

        [Fact]
        public async Task Login_should_successfully_login()
        {
            // Arrange
            await HttpClient.PostJson(RegisterUrl, _newUser);

            // Act
            var result = await HttpClient.PostJson<LoginResult>(LoginUrl, _newUser);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Token);
        }

        [Fact]
        public async Task Login_should_return_password_or_user_name_is_incorrect_when_user_does_not_exist()
        {
            // Act
            var result = await HttpClient.PostJson<LoginResult>(LoginUrl, new UserDto { Login = "non_existing_user" });

            // Assert
            Assert.False(result.Success);
            Assert.Null(result.Token);
            Assert.Equal(AuthenticationService.PasswordOrUsernameIncorrect, result.Message);
        }

        [Fact]
        public async Task Login_should_return_password_or_user_name_is_incorrect_when_users_password_or_login_is_incorrect()
        {
            // Arrange
            await HttpClient.PostJson(RegisterUrl, _newUser);
            _newUser.Password = "wrong_password";

            // Act
            var result = await HttpClient.PostJson<LoginResult>(LoginUrl, _newUser);

            // Assert
            Assert.False(result.Success);
            Assert.Null(result.Token);
            Assert.Equal(AuthenticationService.PasswordOrUsernameIncorrect, result.Message);
        }
    }
}
