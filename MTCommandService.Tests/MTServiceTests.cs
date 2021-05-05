using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MTCommandProcessor.MultiTrak;
using System;
using System.Threading.Tasks;

namespace MTCommandProcessor.Tests
{
    public class MTServiceTests
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
            }).Build().Services;
        }


        [Test]
        public async Task MTService_CanSendACommand()
        {
            var mtService = services.GetRequiredService<IMTService>();
            MTCommand command = new MTCommand()
            {
                Name = "ping",
                DisplayName = "locate",
                Code = 5,
                Params = null,
                Message = "{}"
            };


            var response = await mtService.SendMTCommandAsync("MT-DEVSIM-3", command);
            Assert.IsTrue(response.Success);
        }
    }
}
