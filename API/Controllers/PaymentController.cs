
using API.Errors;
using Core.Entities.BasketEntites;
using Core.interfaces.Services;
using OrdersAndItemsService.Controllers;


namespace API.Controllers
{
    [Authorize]
    public class PaymentController(IPaymentService paymentService) : BaseApiController
    {

        [ProducesResponseType(typeof(Basket), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("{basketId}")]
        [Authorize]
        public async Task<ActionResult<Basket>> CreateOrUpdatePaymentIntend(string basketId)
        {
            var basket = await paymentService.CreateOrUpdatePaymentIntent(basketId);

            if (basket is null)
                return BadRequest(new ApiResponse(400));

            return Ok(basket);
        }
    }
}