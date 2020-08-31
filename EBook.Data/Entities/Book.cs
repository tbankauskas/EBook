using System.Collections.Generic;

namespace EBook.Data.Entities
{
    public class Book
    {
        public int BookId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public ICollection<UserBook> UserBooks { get; set; }
    }
}
