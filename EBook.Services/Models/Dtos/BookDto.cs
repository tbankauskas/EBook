namespace EBook.Services.Models.Dtos
{
    public class BookDto
    {
        public int BookId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public UserBookDto UserBookSubscription { get; set; }
    }
}
