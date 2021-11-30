using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rotomdex.Domain.Adapters;
using Rotomdex.Integration.Adapters;
using Rotomdex.Web.Api.Configuration;

namespace Rotomdex.Web.Api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddApplicationPart(typeof(Startup).Assembly)
                .AddControllersAsServices();

            ConfigureDependencies(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        protected virtual void ConfigureDependencies(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var apiSettings = serviceProvider.GetService<PokeApiSettings>();

            services.AddSingleton<IPokemonApiAdapter>(new PokeApiAdapter(new HttpClient(), apiSettings.BaseAddress));
        }
    }
}