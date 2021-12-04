using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rotomdex.Domain.Services;
using Rotomdex.Integration.Adapters;
using Rotomdex.Integration.Factories;
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
            services.AddSingleton<ITranslationService, TranslationService>();
            services.AddSingleton<ITranslatorFactory, TranslatorFactory>();
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

            var translatorApiSettings = serviceProvider.GetService<TranslatorApiSettings>();
            var httpClient = new HttpClient() { BaseAddress = translatorApiSettings.BaseAddress };
            services.AddSingleton<ITranslationsApiAdapter>(new YodaTranslatorAdapter(httpClient));
            services.AddSingleton<ITranslationsApiAdapter>(new ShakespeareTranslatorAdapter(httpClient));
        }
    }
}