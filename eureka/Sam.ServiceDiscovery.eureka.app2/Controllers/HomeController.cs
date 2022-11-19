using Microsoft.AspNetCore.Mvc;

namespace Sam.ServiceDiscovery.eureka.app2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Instance 2";
        }
    }

}
