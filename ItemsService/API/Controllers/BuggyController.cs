
using API.Errors;
using Core.Entities.ProductEntities;
using OrdersAndItemsService.Controllers;
using Repository.Data;

namespace API.Controllers
{
    public class ErrorsController(StoreContext _storeContext) : BaseApiController
    {

        [HttpGet("notfound")]
        public ActionResult<Product> GetNotFound(int productId) 
        {
            var product = _storeContext.Products.Find(productId);
            if (product == null)
                return NotFound(new ApiResponse(404));
            return Ok(product);
        }

       

        [HttpGet("unauthorize")] 
        public ActionResult GetUnanouthorizeError()
        {
            return Unauthorized(new ApiResponse(401));
        }

        [HttpGet("badrequest")] 
        public ActionResult GetBadRequest()
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