using EBook.Services.Models.Interfaces;

namespace EBook.Services.Models
{
    public class LoginResult:IRequestResult
    {
        public string Token { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }
}
