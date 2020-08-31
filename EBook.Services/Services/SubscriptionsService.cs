using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EBook.Data.Ef;
using EBook.Data.Entities;
using EBook.Services.Models.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace EBook.Services.Services
{
    public class SubscriptionsService
    {
        private readonly EBookDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string UserLogin => _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

        public SubscriptionsService(EBookDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<BookDto>> GetLogedInUserBookSubscriptions()
        {
            var books = await _dbContext.UserBooks.Where(x => x.User.Login == UserLogin).Select(x =>
                    new BookDto
                    {
                        BookId = x.BookId,
                        Name = x.Book.Name,
                        Price = x.Book.Price,
                        UserBookSubscription = new UserBookDto
                        {
                            BookId = x.BookId,
                            UserId = x.UserId,
                            PurchaseDateTime = x.PurchaseDateTime
                        }
                    })
                .ToListAsync();
            return books;
        }

        public async Task<bool> IsSubscriptionAlreadyPurchased(int bookId)
        {
            return await _dbContext.UserBooks.FirstOrDefaultAsync(x => x.User.Login == UserLogin && x.BookId == bookId) != null;
        }

        public async Task<bool> DoesBookExists(int bookId)
        {
            return await _dbContext.Books.FirstOrDefaultAsync(a => a.BookId == bookId) != null;
        }

        public async Task<UserBookDto> PurchaseSubscription(BookDto bookDto)
        {
            var userId = _dbContext.Users.First(a => a.Login == UserLogin).UserId;
            var userBook = new UserBook
            {
                UserId = userId,
                BookId = bookDto.BookId,
                PurchaseDateTime = DateTime.Now
            };
            _dbContext.UserBooks.Add(userBook);
            await _dbContext.SaveChangesAsync();

            return new UserBookDto
            {
                BookId = userBook.BookId,
                UserId = userBook.UserId,
                PurchaseDateTime = userBook.PurchaseDateTime
            };
        }

        public async Task Unsubscribe(int bookId)
        {
            var userBookToUnsubscribe = await _dbContext.UserBooks.FirstOrDefaultAsync(x => x.User.Login == UserLogin && x.BookId == bookId);
            if (userBookToUnsubscribe != null)
            {
                _dbContext.UserBooks.Remove(userBookToUnsubscribe);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
