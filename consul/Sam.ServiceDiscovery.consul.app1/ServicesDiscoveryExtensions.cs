using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Sam.ServiceDiscovery.consul.app1
{
    public static class ServiceDiscoveryExtensions
    {
        public static IServiceCollection AddConsul(this IServiceCollection services, IConfiguration Configuration)
        {
            var config = Configuration.GetSection(nameof(ConsulOptions)).Get<ConsulOptions>();

            services.AddSingleton(config);

            if (config.Enabled)
            {
                services.AddSingleton<IConsulClient>(c => new ConsulClient(cfg =>
                {
                    if (!string.IsNullOrEmpty(config.Host))
                    {
                        cfg.Address = new Uri(config.Host);
                    }
                }));
            }

            return services;
        }


        public static string UseConsul(this IApplicationBuilder app, IConfiguration Configuration)
        {
            var config = Configuration.GetSection(nameof(ConsulOptions)).Get<ConsulOptions>();

            if (config.Enabled)
            {
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var Iconfig = scope.ServiceProvider.GetService<IConfiguration>();

                    var client = scope.ServiceProvider.GetService<IConsulClient>();

                    var consulServiceRistration = new AgentServiceRegistration
                    {
                        Name = config.Service,
                        ID = $"{config.Service}-{config.Address}",
                        Address = config.Address.Host,
                        Port = config.Address.Port,
                    };

                    client.Agent.ServiceRegister(consulServiceRistration).Wait();

                    return consulServiceRistration.ID;
                }
            }
            return string.Empty;
        }
    }
    public class ConsulOptions
    {
        public bool Enabled { get; set; }
        public string Service { get; set; }
        public Uri Address { get; set; }
        public string Host { get; set; }
    }
}
