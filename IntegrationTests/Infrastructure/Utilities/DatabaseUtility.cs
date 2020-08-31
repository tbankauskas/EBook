using System.Threading.Tasks;
using EBook.Data.Ef;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests.Infrastructure.Utilities
{
    public static class DatabaseUtility
    {
        public const string ConnectionString = "Server=(localdb)\\MSSQLLocalDB;Database=EBookDb.Testing;Trusted_Connection=True;";

        public static async Task CleanUp()
        {
            await using var context = CreateDbContext();
            await context.Database.ExecuteSqlRawAsync(
                @"
                    TRUNCATE TABLE [UserBook]
                    DELETE FROM [User];
                    DELETE FROM [Book];
                 "
                );
        }

        public static EBookDbContext CreateDbContext()
        {
            return new EBookDbContext(GetDbContextOptions<EBookDbContext>());
        }

        private static DbContextOptions<T> GetDbContextOptions<T>() where T : DbContext
        {
            return new DbContextOptionsBuilder<T>()
                .UseSqlServer(ConnectionString)
                .Options;
        }
    }
}
