﻿using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Pokemon.Rotomdex.Web.Api.Configuration;

namespace Pokemon.Rotomdex.Web.Api.UnitTests
{
    [TestFixture]
    public class StartupTests
    {
        [Test]
        public void When_Validating_Container_Then_All_Dependencies_Are_Setup_Correctly()
        {
            Assert.DoesNotThrow(() =>
            {
                new WebHostBuilder()
                    .ConfigureServices(x =>
                    {
                        x.AddSingleton(new PokeApiSettings
                        {
                            BaseAddress = new Uri("http://foo.com")
                        });
                    })
                    .UseStartup<Startup>()
                    .UseDefaultServiceProvider((c, o) => { o.ValidateOnBuild = true; })
                    .Build();
            });
        }
    }
}