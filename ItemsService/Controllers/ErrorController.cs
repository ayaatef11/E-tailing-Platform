
using OrdersAndItemsService.API.Errors;

namespace OrdersAndItemsService.Controllers
{
    [Route("error/{code}")]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [HttpGet]
        public IActionResult Error(int code)
        {
            return new ObjectResult(new ApiResponse(code)); 
        }
    }
}
