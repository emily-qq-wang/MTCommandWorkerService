using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MTCommandProcessor.MultiTrak;
using System;
using System.Threading.Tasks;
using MTCommandProcessor.Data;
using Moq;
using System.Collections.Generic;

namespace MTCommandProcessor.Tests
{
    public class MTCommandAttributeTests
    {
        IServiceProvider services;

        [SetUp]
        public void Setup()
        {
             services = Host.CreateDefaultBuilder(new string[] { })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHttpClient();
                services.AddSingleton<IServiceAccess, MTServiceAccess>();
                services.AddSingleton<IMTCommandAttributesService, MTCommandAttributesService>();
                services.AddSingleton<IDNARepository, DNARepository>();
            }).Build().Services;
        }


        [Test]
        public async Task MTAttributesService_CanBuildACommand()
        {
            var mtAttService = services.GetRequiredService<IMTCommandAttributesService>();
            Dictionary<string, string> options = new Dictionary<string, string>();
            var command = await mtAttService.GetMTCommand("ping", "QA", options);

            Assert.IsTrue(command.Name.Equals("ping"));
            Assert.IsTrue(command.Params.Count == 1);
        }

        [Test]
        public async Task MTAttributesService_CanBuildZoneCommand()
        {
            var mtAttService = services.GetRequiredService<IMTCommandAttributesService>();
            Dictionary<string, string> options = new Dictionary<string, string>();
            var command = await mtAttService.GetMTCommand("zod", "QA", options);

            Assert.IsTrue(command.Name.Equals("zod"));
            Assert.IsTrue(command.DisplayName.Equals("ZOD Update"));
            Assert.IsTrue(command.Params[0].Type.Equals("Array"));
        }
    }
}
