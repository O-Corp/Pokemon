using System;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pokemon.Rotomdex.Web.Api.Adapters;

namespace Pokemon.Rotomdex.Web.Api
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
            services.AddSingleton<IPokemonApiAdapter>(new PokeApiStandardAdapter(new HttpClient(), new Uri("http://foo.com")));
        }
    }
}