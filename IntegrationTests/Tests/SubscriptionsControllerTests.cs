using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using EBook.Data.Entities;
using EBook.Services.Models.Dtos;
using IntegrationTests.Builders;
using IntegrationTests.Infrastructure;
using IntegrationTests.Infrastructure.Utilities;
using Xunit;

namespace IntegrationTests.Tests
{
    public class SubscriptionsControllerTests : BaseHttpApiTests
    {
        private const string ApiUrl = "api/subscriptions/";

        [Fact]
        public async Task Get_should_return_unauthorized_when_not_authenticated_user()
        {
            // Act
            var response = await HttpClient.GetAsync(ApiUrl);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Get_should_return_all_user_subscriptions()
        {
            // Arrange
            var user = await CreateUser("TestUser", "Book1", "Book2");

            Factory.AddAuthorization(HttpClient);

            // Act
            var result = await HttpClient.GetJson<List<BookDto>>(ApiUrl);

            // Assert
            Assert.Equal(user.UserBooks.Count, result.Count);
        }

        [Fact]
        public async Task Post_should_return_unauthorized_when_not_authenticated_user()
        {
            // Act
            var response = await HttpClient.PostJson(ApiUrl, new UserBookDto());

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Post_should_purchase_book_subscription()
        {
            // Arrange
            var book = await new BookBuilder().WithName("Book").SaveToDb();
            var user = await CreateUser("TestUser");

            Factory.AddAuthorization(HttpClient);

            // Act
            var response = await HttpClient.PostJson(ApiUrl,
                new UserBookDto { UserId = user.UserId, BookId = book.BookId });
            var result = await response.Deserialize<UserBookDto>();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.IsType<UserBookDto>(result);
        }

        [Fact]
        public async Task Post_should_return_bad_request_with_already_purchased_message()
        {
            // Arrange
            var user = await CreateUser("TestUser", "Book");

            Factory.AddAuthorization(HttpClient);

            // Act
            var response = await HttpClient.PostJson(ApiUrl,
                new UserBookDto { UserId = user.UserId, BookId = user.UserBooks.First().BookId });

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("Book subscription is already purchased", await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task Post_should_return_bad_request_if_book_does_not_exist()
        {
            // Arrange
            var user = await CreateUser("TestUser");

            Factory.AddAuthorization(HttpClient);

            // Act
            var response = await HttpClient.PostJson(ApiUrl,
                new UserBookDto { UserId = user.UserId, BookId = 9999 });

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("Book subscription doesn't exist", await response.Content.ReadAsStringAsync());
        }

        [Fact]
        public async Task Delete_should_return_unauthorized_when_not_authenticated_user()
        {
            // Arrange
            var user = await CreateUser("TestUser", "Book");

            // Act
            var response =
                await HttpClient.DeleteAsync($"{ApiUrl}{user.UserBooks.First().BookId}");

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task Delete_should_delete_user_subscription()
        {
            // Arrange
            var user = await CreateUser("TestUser", "Book");
            Factory.AddAuthorization(HttpClient);

            // Act
            var response =
                await HttpClient.DeleteAsync($"{ApiUrl}{user.UserBooks.First().BookId}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var context = DatabaseUtility.CreateDbContext();
            var userBooks = context.UserBooks.Where(a => a.UserId == user.UserId).ToList();
            Assert.Empty(userBooks);
        }

        private static async Task<User> CreateUser(string login, params string[] books)
        {
            var builder = new UserBuilder().WithLogin(login);
            builder = books.Aggregate(builder, (current, book) => current.WithUserBook(x => x.WithName(book)));

            return await builder.SaveToDb();
        }
    }
}
