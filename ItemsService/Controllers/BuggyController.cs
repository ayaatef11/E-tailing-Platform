
using OrdersAndItemsService.API.Errors;
using OrdersAndItemsService.Controllers;
using OrdersAndItemsService.Core.Entities;
using Repository.Data;

namespace API.Controllers
{
    public class ErrorsController : BaseApiController
    {
        private readonly StoreContext _storeContext;

        public ErrorsController(StoreContext storeContext)
        {
            _storeContext = storeContext;
        }


        [HttpGet("notfound")]
        public ActionResult<Product> GetNotFound(int productId) 
        {
            var product = _storeContext.Products.Find(productId);
            if (product == null)
                return NotFound(new ApiResponse(404));
            return Ok(product);
        }

        [HttpGet("badrequest")]
        public ActionResult GetBadRequest() 
        {
            return BadRequest(new ApiResponse(400));
        }

        [HttpGet("unauthorize")] 
        public ActionResult GetUnanouthorizeError(int id)
        {
            return Unauthorized(new ApiResponse(401));
        }

        [HttpGet("badrequest/{id}")] 
        public ActionResult GetBadRequest(int id)
        {
            return Ok(new ApiResponse(400));
        }

        [HttpGet("servererror")] 
        public ActionResult GetServerError(int id)
        {
            var product = _storeContext.Products.Find(id);
            var productToReturn = product.ToString();///it can't be converted so it will lead to server error 
            return Ok(productToReturn);
        }
}
}