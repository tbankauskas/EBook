using EBook.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EBook.Data.Ef.Configurations
{
    public class UserBookConfiguration : IEntityTypeConfiguration<UserBook>
    {
        public void Configure(EntityTypeBuilder<UserBook> builder)
        {
            builder
                .HasIndex(x => new {x.UserId, x.BookId})
                .IsUnique();

            builder
                .Property(p => p.PurchaseDateTime)
                .IsRequired()
                .HasDefaultValueSql("GetDate()");
        }
    }
}
