using Microsoft.Extensions.DependencyInjection;

namespace Pokemon.Rotomdex.Web.Api.ComponentTests
{
    public class TestStartup : Startup
    {
        private readonly DataContainer _dataContainer;

        public TestStartup(DataContainer dataContainer)
        {
            _dataContainer = dataContainer;
        }
        
        protected override void ConfigureDependencies(IServiceCollection services)
        {
            services.AddSingleton(_dataContainer.ApiAdapter);
        }
    }
}