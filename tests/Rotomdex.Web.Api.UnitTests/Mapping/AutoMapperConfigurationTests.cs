using AutoMapper;
using NUnit.Framework;
using Rotomdex.Web.Api.Configuration.Mapping;

namespace Rotomdex.Web.Api.UnitTests.Mapping
{
    [TestFixture]
    public class AutoMapperConfigurationTests
    {
        [Test]
        public void Domain_Mapping_Profile_Configuration_Is_Valid()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<DomainMappingProfile>();
            });
            config.AssertConfigurationIsValid();
        }
    }
}