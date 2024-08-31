
using Core.Entities.BasketEntites;
using Core.interfaces.Repositories;


namespace OrdersAndItemsService.Controllers
{
 
    public class BasketController(IBasketRepository _basketRepository) : BaseApiController
    {

        [HttpGet("{id}")]
        public async Task<ActionResult<Basket>> GetBasketById(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);

            return Ok(basket ?? new Basket(id));
        }

        
        [HttpPost]
        public async Task<ActionResult<Basket>> UpdateBasket(Basket basket)
        {
            var updatedBasket = await _basketRepository.CreateOrUpdateBasketAsync(basket);
     
            return Ok(updatedBasket);
        }

      
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBasketAsync(string id)
        {
            await _basketRepository.DeleteBasketAsync(id);

            return NoContent();
        }
    }
}
