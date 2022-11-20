using Consul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sam.ServiceDiscovery.consul.Client
{
    public interface IConsulClientService
    {
        Task<Uri> GetRequestUriAsync(string serviceName, string path);
    }
    public class ConsulClientService : IConsulClientService
    {
        private IConsulClient _consulclient;

        public ConsulClientService(IConsulClient consulclient)
        {
            _consulclient = consulclient;
        }

        public async Task<Uri> GetRequestUriAsync(string serviceName, string path)
        {
            //Get all services registered on Consul
            var allRegisteredServices = await _consulclient.Agent.Services();

            //Get all instance of the service went to send a request to
            var registeredServices = allRegisteredServices.Response?.Where(s => s.Value.Service.Equals(serviceName, StringComparison.OrdinalIgnoreCase)).Select(x => x.Value).ToList();

            //Get a random instance of the service
            var service = GetRandomInstance(registeredServices, serviceName);

            if (service == null)
                throw new Exception($"Consul service: '{serviceName}' was not found.");

            var uriBuilder = new UriBuilder()
            {
                Host = service.Address,
                Port = service.Port,
                Path = path
            };

            return uriBuilder.Uri;

            AgentService GetRandomInstance(IList<AgentService> services, string serviceName) => services[new Random().Next(0, services.Count)];
        }
    }
}