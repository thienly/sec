using Microsoft.AspNetCore.Mvc;

namespace Service2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {        
        [HttpPost]
        [Route("Order")]
        public IActionResult Order()
        {
            return Ok();
        }

        [HttpPost]
        [Route("ReOrder")]
        public IActionResult ReOrder()
        {
            return Ok();
        }
    }
}
