using EBook.Data.Entities;

namespace IntegrationTests.Builders
{
    public class BookBuilder : EntityBuilder<Book>
    {
        private string _name = "book_name";
        private decimal _price = 99.99M;

        public override Book Create()
        {
            return new Book
            {
                Name = _name,
                Price = _price
            };
        }

        public BookBuilder WithName(string name)
        {
            _name = name;
            return this;
        }
    }
}
