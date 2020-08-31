using System.Threading.Tasks;
using EBook.Services.Services;
using Microsoft.AspNetCore.Mvc;

namespace EBook.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class BooksController : ControllerBase
    {
        private readonly BooksService _booksService;

        public BooksController(BooksService booksService)
        {
            _booksService = booksService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var books = await _booksService.GetAllBooks();
            return Ok(books);
        }
    }
}