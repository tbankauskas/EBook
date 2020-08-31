using System;

namespace EBook.Data.Entities
{
    public class UserBook
    {
        public int UserBookId { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime PurchaseDateTime { get; set; }
        public User User { get; set; }
        public Book Book { get; set; }
    }
}
