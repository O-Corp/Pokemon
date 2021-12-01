using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rotomdex.Domain.Services;
using Rotomdex.Integration.Adapters;
using Rotomdex.Integration.Services;
using Rotomdex.Web.Api.Configuration;
using Rotomdex.Web.Api.Middleware;

namespace Rotomdex.Web.Api
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAutoMapper(typeof(Startup))
                .AddControllers()
                .AddApplicationPart(typeof(Startup).Assembly)
                .AddControllersAsServices();

            ConfigureDependencies(services);

            services.AddSingleton<IPokemonService, PokemonService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ServiceUnavailableMiddleware>();
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