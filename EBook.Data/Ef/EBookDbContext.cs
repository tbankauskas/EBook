using EBook.Data.Ef.Configurations;
using EBook.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace EBook.Data.Ef
{
    public class EBookDbContext : DbContext
    {
        public EBookDbContext(DbContextOptions<EBookDbContext> options)
            : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<UserBook> UserBooks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                entityType.SetTableName(entityType.DisplayName());
            }

            modelBuilder.ApplyConfiguration(new UserBookConfiguration());
        }
    }
}