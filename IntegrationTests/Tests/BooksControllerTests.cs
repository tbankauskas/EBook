using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBook.Services.Models.Dtos;
using IntegrationTests.Builders;
using IntegrationTests.Infrastructure;
using IntegrationTests.Infrastructure.Utilities;
using Xunit;

namespace IntegrationTests.Tests
{
    public class BooksControllerTests : BaseHttpApiTests
    {
        private const string ApiUrl = "api/books/";

        private static async Task CreateBooks()
        {
            await new BookBuilder().SaveToDb();
            await new BookBuilder().SaveToDb();
            await new BookBuilder().SaveToDb();
        }

        [Fact]
        public async Task Get_should_return_all_books()
        {
            // Arrange
            await CreateBooks();

            // Act
            var result = await HttpClient.GetJson<List<BookDto>>(ApiUrl);

            // Assert
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public async Task Get_should_return_all_books_with_user_book_subscription_if_exists()
        {
            // Arrange
            await CreateBooks();

            var user = await new UserBuilder()
                .WithLogin("TestUser")
                .WithUserBook(x => x.WithName("Book1"))
                .WithUserBook(x => x.WithName("Book2"))
                .SaveToDb();

            Factory.AddAuthorization(HttpClient);

            // Act
            var result = await HttpClient.GetJson<List<BookDto>>(ApiUrl);

            // Assert
            Assert.Equal(5, result.Count);
            Assert.Equal(user.UserBooks.Count, result.Where(x => x.UserBookSubscription != null).ToList().Count);
        }
    }
}
