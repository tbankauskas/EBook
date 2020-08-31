using System;
using System.Collections.Generic;
using EBook.Data.Entities;

namespace IntegrationTests.Builders
{
    public class UserBuilder : EntityBuilder<User>
    {
        private string _login = "Login";
        private string _firstName = "FirstName";
        private string _lastName = "LastName";
        private string _email = "email@email.com";
        private readonly string _pwdHash = "Aaaaa".GetHashCode().ToString();
        private readonly List<BookBuilder> _bookBuilders = new List<BookBuilder>();

        public override User Create()
        {
            var user = new User
            {
                Email = _email,
                FirstName = _firstName,
                LastName = _lastName,
                Login = _login,
                PasswordHash = _pwdHash,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserBooks = new List<UserBook>()
            };

            foreach (var bookBuilder in _bookBuilders)
            {
                var book = bookBuilder.Create();
                user.UserBooks.Add(new UserBook { Book = book, User = user });
            }
            
            return user;
        }

        public UserBuilder WithLogin(string login)
        {
            _login = login;
            return this;
        }

        public UserBuilder WithUserBook(Action<BookBuilder> config)
        {
            var builder = new BookBuilder();
            config?.Invoke(builder);
            _bookBuilders.Add(builder);
            return this;
        }
    }
}
