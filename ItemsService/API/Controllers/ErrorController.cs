
using API.Errors;

namespace OrdersAndItemsService.Controllers
{
    [Route("error/{code}")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
       
        public IActionResult Error(int code)
        {
            return new ObjectResult(new ApiResponse(code)); 
        }
    }
}
