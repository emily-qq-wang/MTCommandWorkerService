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
    public class MTCommandServiceTests
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
                services.AddSingleton<IMTService, MTService>();
                services.AddSingleton<IMTCommandAttributesService, MTCommandAttributesService>();
                services.AddSingleton<IDNARepository, DNARepository>();
                services.AddSingleton<ICommandService, MTCommandService>();
            }).Build().Services;
        }


        [Test]
        public async Task MTCommandService_CanSendACommand()
        {
            var mtCommandService = services.GetRequiredService<ICommandService>();
            
            Dictionary<string, string> options = new Dictionary<string, string>();
            var response = await mtCommandService.SendMessageAsync("ID901713","MT-DEVSIM-3","ping","QA",options, string.Empty);
            Assert.IsTrue(response.Success);
        }
    }
}
