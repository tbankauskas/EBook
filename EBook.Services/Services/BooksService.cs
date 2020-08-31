using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EBook.Data.Ef;
using EBook.Services.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EBook.Services.Services
{
    public class BooksService
    {
        private readonly EBookDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BooksService(EBookDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<BookDto>> GetAllBooks()
        {
            if (!_httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
            {
                return await _dbContext.Books
                    .Select(x => new BookDto { BookId = x.BookId, Name = x.Name, Price = x.Price })
                    .ToListAsync();
            }

            var user = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = (await _dbContext.Users.FirstAsync(a => a.Login == user.Value)).UserId;

            return await _dbContext.Books
                .Select(x => new BookDto
                {
                    BookId = x.BookId,
                    Name = x.Name,
                    Price = x.Price,
                    UserBookSubscription = x.UserBooks
                        .Where(ub => ub.UserId == userId)
                        .Select(ubs => new UserBookDto
                        {
                            UserId = ubs.UserId,
                            BookId = ubs.BookId,
                            PurchaseDateTime = ubs.PurchaseDateTime
                        })
                        .First()
                })
                .ToListAsync();
        }
    }
}
