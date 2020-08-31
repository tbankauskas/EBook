using System.Threading.Tasks;
using EBook.Services.Models.Dtos;
using EBook.Services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EBook.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SubscriptionsController : ControllerBase
    {
        private readonly SubscriptionsService _subscriptionsService;

        public SubscriptionsController(SubscriptionsService subscriptionsService)
        {
            _subscriptionsService = subscriptionsService;
        }

        [HttpGet()]
        public async Task<IActionResult> Get()
        {
            var userBooks = await _subscriptionsService.GetLogedInUserBookSubscriptions();
            return Ok(userBooks);
        }

        [HttpPost]
        public async Task<IActionResult> Post(BookDto book)
        {
            if (!await _subscriptionsService.DoesBookExists(book.BookId))
            {
                return BadRequest("Book subscription doesn't exist");
            }

            if (await _subscriptionsService.IsSubscriptionAlreadyPurchased(book.BookId))
            {
                return BadRequest("Book subscription is already purchased");
            }

            var result = await _subscriptionsService.PurchaseSubscription(book);
            return Ok(result);
        }

        [HttpDelete("{bookId}")]
        public async Task<IActionResult> Delete(int bookId)
        {
            await _subscriptionsService.Unsubscribe(bookId);
            return Ok();
        }
    }
}