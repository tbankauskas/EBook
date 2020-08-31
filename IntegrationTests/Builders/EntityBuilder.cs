using System.Threading.Tasks;
using IntegrationTests.Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests.Builders
{
    public abstract class EntityBuilder<T> where T:class
    {
        public abstract T Create();

        public virtual async Task<T> SaveToDb()
        {
            var entity = Create();

            await using var context = CreateContext();
            context.Add(entity);
            await context.SaveChangesAsync();

            return entity;
        }

        protected virtual DbContext CreateContext()
        {
            return DatabaseUtility.CreateDbContext();
        }
    }
}
