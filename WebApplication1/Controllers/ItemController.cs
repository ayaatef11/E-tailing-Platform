

using OrdersAndItemsService.Controllers;

namespace WebApplication1.Controllers
{
    public class ItemController(IRepository<Item> _service) : BaseApiController
    {
       
        public async Task< ActionResult> Index()
        {
            var result =await  _service.GetAllAsync();
            if (result!=null) return Ok(result);
            else return NotFound();
          
        }

        public async Task<ActionResult> Details(int id)
        {
            var result=await _service.GetByIdAsync(id);
            if (result != null) return Ok(result);
            else return NotFound();
           
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Item model)
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
        public ActionResult Edit( Item model)
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

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Item model)
        {
            try
            {
                var result= _service.DeleteAsync(model);
                if (result != null) return Ok();else return NotFound();
               // return RedirectToAction(nameof(Index));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
