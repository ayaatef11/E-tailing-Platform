using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Iservices;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //*******validations of the functions
    //validateAntiForgeryToken search on it 
    //search on repository pattern
    public class UserController(IRepository<AppUser> _service) : ControllerBase
    {

        public ActionResult GetUserById(int id)
        {
            var result = _service.GetByIdAsync(id);
            if (result != null) return Ok(result);
            else return NotFound();

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AppUser model)
        {
            try
            {
                var result = _service.SaveAsync(model);
                if (result != null) return Ok(result); else return NotFound();
            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AppUser model)
        {
            try
            {
                var result = _service.Update(model);
                if (result != null) return Ok();
                else return NotFound();
            }
            catch
            {
                return BadRequest();
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(AppUser model)
        {
            try
            {
                var result = _service.DeleteAsync(model);
                if (result != null) return Ok(); else return NotFound();
                // return RedirectToAction(nameof(Index));
            }
            catch
            {
                return BadRequest();
            }
        }
        //**********************add them to item controller
      

        [HttpPost("{userId}/orders/{orderId}/newItem")]
        public IActionResult AddItemToOrder(int userId, int orderId, [FromBody] Item newItem)
        {
            var user = _service.GetById(userId);
            if (user == null)
                return NotFound();

            var order = user.orders.FirstOrDefault(o => o.Id == orderId);
            if (order == null)
                return NotFound();

            newItem.Id = order.Items.Count + 1; 
            order.Items.Add(newItem);
            return Ok(newItem);
        }
    }
}

