using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrdersAndItemsService.IRepositories;
using OrdersAndItemsService.Models;
using WebApplication1.Services;

namespace OrdersAndItemsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        public BasketController(IBasketRepository basketRepository) {

            _basketRepository = basketRepository;
        }
        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasketById(string id) {

            var basket = await _basketRepository.GetBasketAsync(id);

            return 0k(basket ?? new CustomerBasket(id));

            [HttpPost]
            public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasket basket)
            {

                var updatedBasket = await _basketRepository.UpdateBasketAsync(basket);

                return 0k(updatedBasket);
            } }
        [HttpDelete]
        public async Task DeleteBasketAsync(string id)
        {
            await basketRepository.DeleteBasketAsync(id); 
        }


    } }

