using Microsoft.AspNetCore.Mvc;
using Steeltoe.Common.Discovery;
using Steeltoe.Discovery;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sam.ServiceDiscovery.eureka.Client.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        DiscoveryHttpClientHandler _handler;

        public HomeController(IDiscoveryClient client)
        {
            _handler = new DiscoveryHttpClientHandler(client);
        }

        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            var list = new List<string>();

            var client = new HttpClient(_handler, false);

            for (int i = 0; i < 20; i++)
                list.Add(await client.GetStringAsync("http://Sam_ServiceDiscovery_eureka_app/Home"));

            return list;
        }
    }
}
