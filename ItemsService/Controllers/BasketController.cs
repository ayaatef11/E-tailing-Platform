using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrdersAndItemsService.Core.interfaces.Repositories;
using OrdersAndItemsService.Core.Models;

namespace OrdersAndItemsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;

        public BasketController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;
        }

        // GET: api/Basket/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerBasket>> GetBasketById(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);

            // Return OK with the basket, or an empty basket if null
            return Ok(basket ?? new CustomerBasket(id));
        }

        // POST: api/Basket
        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasket basket)
        {
            var updatedBasket = await _basketRepository.UpdateBasketAsync(basket);

            // Return OK with the updated basket
            return Ok(updatedBasket);
        }

        // DELETE: api/Basket/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBasketAsync(string id)
        {
            await _basketRepository.DeleteBasketAsync(id);

            // Return NoContent indicating successful deletion
            return NoContent();
        }
    }
}
