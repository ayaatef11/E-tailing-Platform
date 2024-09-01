using Core.Entities.OrderEntities;
using Core.interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController(IOrderService orderService) : ControllerBase
    {
        [HttpGet("deliveryMethod")]
        public async Task<ActionResult<IReadOnlyList<OrderDeliveryMethod>>> GetAllDeliveryMethods()
        {
            var deliveryMethods = await orderService.GetAllDeliveryMethodsAsync();

            return Ok(deliveryMethods);
        }
    }
}
