using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EBook.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EBook.Data.Ef
{
    public static class SeedData
    {
        public static async Task EnsureSeedData(IServiceProvider provider)
        {
            var dbContext = provider.GetRequiredService<EBookDbContext>();
            await dbContext.Database.MigrateAsync();

            if (!await dbContext.Books.AnyAsync())
            {
                await dbContext.Books.AddRangeAsync(GenerateRandomBooks());
                await dbContext.SaveChangesAsync();
            }
        }

        private static IEnumerable<Book> GenerateRandomBooks()
        {
            var books = new List<Book>();
            for (var i = 0; i < 50; i++)
            {
                books.Add(new Book
                {
                    Name = Guid.NewGuid().ToString(),
                    Price = new decimal(new Random().NextDouble() * 10)
                });
            }

            return books;
        }
    }
}
