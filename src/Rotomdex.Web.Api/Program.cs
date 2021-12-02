using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rotomdex.Web.Api.Configuration;

namespace Rotomdex.Web.Api
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

                    var translatorApiSettings = new TranslatorApiSettings();
                    ctx.Configuration.Bind(nameof(TranslatorApiSettings), translatorApiSettings);
                    services.AddSingleton(translatorApiSettings);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }    }
}