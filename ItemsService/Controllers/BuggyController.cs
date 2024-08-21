using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrdersAndItemsService.Errors;
using OrdersAndItemsService.Repository.Data;
namespace OrdersAndItemsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BuggyController(AppDbContext context) : ControllerBase
    {
        private readonly AppDbContext _context = context;

        [HttpGet("notfound")]
        public ActionResult GetNotFoundRequest(int id ) {
            var thing = _context.Items.Find(id);
            if(thing==null)return NotFound(new ApiResponse(500));
            return Ok();
        }

        [HttpGet("serverError")]
        public ActionResult GetServerError(int id)
        {
            var thing = _context.Items.Find(id);
            var thingToReturn = thing!.ToString();//if it is null here will create an exception
            return Ok();
        }
        [HttpGet("badRequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest();
        }

        [HttpGet("badRequest/{id}")]
        public ActionResult GetNotFound(int id ) { 
        return Ok(new ApiResponse(400));
        }
    }
}
