using EBook.Data.Ef;
using IntegrationTests.Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests.Infrastructure
{
    public class BaseTest
    {
        protected BaseTest()
        {
            var options = new DbContextOptionsBuilder<EBookDbContext>()
                .UseSqlServer(DatabaseUtility.ConnectionString)
                .Options;
            var dbContext = new EBookDbContext(options);
            dbContext.Database.Migrate();
        }
    }

}
