using Microsoft.AspNetCore.Mvc;

namespace Sam.ServiceDiscovery.consul.app1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "Instance 1";
        }
    }
}
