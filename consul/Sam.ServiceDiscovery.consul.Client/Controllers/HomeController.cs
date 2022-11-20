using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Sam.ServiceDiscovery.consul.Client.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IConsulClientService consulClient;

        public HomeController(IConsulClientService consulClient)
        {
            this.consulClient = consulClient;
        }

        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            var list = new List<string>();

            var client = new HttpClient();

            for (int i = 0; i < 20; i++) 
            {
                var addr =await consulClient.GetRequestUriAsync("Sam.ServiceDiscovery.consul.app","Home");
                list.Add(await client.GetStringAsync(addr));
            }

            return list;

        }
    }

}
