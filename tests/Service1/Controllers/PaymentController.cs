using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Service1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {        
        // POST api/values
        [HttpPost]
        [Route("Payment")]
        public IActionResult Payment()
        {
            return Ok();
        }
        [HttpPost]
        [Route("RePayment")]
        public IActionResult RePayment()
        {
            return Ok();
        }
    }
}
