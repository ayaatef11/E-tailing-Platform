

using Core.Entities.Order_Entities;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController(IRepository<Order> _service) : ControllerBase
    {

        public async Task<ActionResult> Index()
        {
            var result = _service.GetAllAsync();
            if (result != null) return Ok(result);
            else return NotFound();
            // return await;
        }

        public ActionResult Details(int id)
        {
            var result = _service.GetByIdAsync(id);
            if (result != null) return Ok(result);
            else return NotFound();

        }
        public IActionResult CreateOrder(int id, [FromBody] Order newOrder)
        {
            var user = _service.GetById(id);
            if (user == null)
                return NotFound();

            newOrder.Id = user.orders.Count + 1;
            user.orders.Add(newOrder);
            return Ok(newOrder);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Order model)
        {
            try
            {
                var result = _service.SaveAsync(model);
                if (result != null) return Ok(result); else return NotFound();
                // return RedirectToAction(nameof(Index));
            }
            catch
            {
                return BadRequest();
            }
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Order model)
        {
            try
            {
                var result = _service.Update(model);
                return Ok(result);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Order model)
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
    }
}
