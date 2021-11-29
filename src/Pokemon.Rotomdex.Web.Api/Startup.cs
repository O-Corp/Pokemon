using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
    }
}