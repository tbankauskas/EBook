namespace EBook.Services.Models.Interfaces
{
    public interface IRequestResult
    {
        string Message { get; set; }
        bool Success { get; set; }
    }
}
