using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MTCommandProcessor.MultiTrak;
using System;
using System.Threading.Tasks;

namespace MTCommandProcessor.Tests
{
    public class MTServiceAccessTests
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
            }).Build().Services;
        }


        [Test]
        public async Task GetMTServiceToken_TokenIsValid()
        {
            var mtServiceAccess = services.GetRequiredService<IServiceAccess>();
            var token = await mtServiceAccess.GetAuthorizationTokenAsync();
            Assert.IsTrue(!String.IsNullOrEmpty(token));
        }

        [Test]
        public async Task GetMTServiceToken_MakeSureTokenIsSaved()
        {
            var mtServiceAccess = services.GetRequiredService<IServiceAccess>();
            var token = await mtServiceAccess.GetAuthorizationTokenAsync();
            Assert.IsTrue(!String.IsNullOrEmpty(token));
            await Task.Delay(2000);
            var token2 = await mtServiceAccess.GetAuthorizationTokenAsync();
            Assert.IsTrue(token.Equals(token2));
            await Task.Delay(2000);
            var token3 = await mtServiceAccess.GetAuthorizationTokenAsync();
            Assert.IsTrue(token3.Equals(token2));
        }
    }
}