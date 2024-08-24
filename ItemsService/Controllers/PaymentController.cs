
using OrdersAndItemsService.API.Errors;
using OrdersAndItemsService.Controllers;
using OrdersAndItemsService.Core.Entities.BasketEntites;
using OrdersAndItemsService.Core.interfaces.Services;


namespace API.Controllers
{
    [Authorize]
    public class PaymentController : BaseApiController
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [ProducesResponseType(typeof(Basket), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost("{basketId}")]
        [Authorize]
        public async Task<ActionResult<Basket>> CreateOrUpdatePaymentIntend(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);

            if (basket is null)
                return BadRequest(new ApiResponse(400));

            return Ok(basket);
        }
    }
}