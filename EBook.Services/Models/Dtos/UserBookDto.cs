using System;

namespace EBook.Services.Models.Dtos
{
    public class UserBookDto
    {
        public int UserId { get; set; }
        public int BookId { get; set; }
        public DateTime PurchaseDateTime { get; set; }

    }
}
