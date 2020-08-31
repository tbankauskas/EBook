using EBook.Services.Models.Interfaces;

namespace EBook.Services.Models
{
    public class RegisterResult : IRequestResult
    {
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}
