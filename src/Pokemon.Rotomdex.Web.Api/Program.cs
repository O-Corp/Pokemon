using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pokemon.Rotomdex.Web.Api.Configuration;

namespace Pokemon.Rotomdex.Web.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host
                .CreateDefaultBuilder(args)
                .ConfigureServices((ctx, services) =>
                {
                    var apiSettings = new PokeApiSettings();
                    ctx.Configuration.Bind(nameof(PokeApiSettings), apiSettings);
                    services.AddSingleton(apiSettings);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }    }
}