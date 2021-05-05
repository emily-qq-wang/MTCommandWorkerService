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
using System.Text;
using MTCommandProcessor.Service;

namespace MTCommandProcessor.Tests
{
    public class CommandProcessorTests
    {
        IServiceProvider services;

        [SetUp]
        public void Setup()
        {
            var mtServiceAccessMock = new Mock<IServiceAccess>();
            mtServiceAccessMock.Setup(d => d.GetAuthorizationTokenAsync()).Returns(Task.FromResult("123"));


            services = Host.CreateDefaultBuilder(new string[] { })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHttpClient();
                services.AddSingleton<IServiceAccess, MTServiceAccesMock>();
                services.AddSingleton<IMTService, MTServiceMock>();
                services.AddSingleton<IMTCommandAttributesService, MTCommandAttributesServiceMock>();
                services.AddSingleton<IDNARepository, DNARepositoryMock>();
                services.AddSingleton<ICommandService, MTCommandServiceMock>();
                services.AddSingleton<ICommandProcessor, CommandProcessor>();
            }).Build().Services;
        }


        [Test]
        public async Task MTCommandService_CanProcessCommands()
        {
            var commandProcessor = services.GetRequiredService<ICommandProcessor>();

            Assert.IsTrue(commandProcessor.StartProcessor());
            var res = await commandProcessor.Process();
            Assert.IsTrue(res);
            var final = await commandProcessor.StopProcessor();
            Assert.IsTrue(final);


        }


    }

    class DNARepositoryMock : IDNARepository
    {
        public Task<List<MTPendingCommand>> GetDelayedCommands()
        {
            List<MTPendingCommand> list = new List<MTPendingCommand>();
            list.Add(new MTPendingCommand()
            {
                PendingID = 1,
                CommandID = 23,
                OID = "101",
                Command = "zod",
                SerialNumber = "MTDEV-SIM-1",
                CreatedDate = DateTime.Now,
                POGroup = "QA",
                zoneInfo = new ZoneInfo()
                {
                    lat = "33",
                    lon = "-117",
                    radius = "100",
                    type = "2",
                    zid = "1",
                    action = "2"
                }
            });
            list.Add(new MTPendingCommand()
            {
                PendingID = 2,
                CommandID = 23,
                OID = "102",
                Command = "zod",
                SerialNumber = "MTDEV-SIM-2",
                CreatedDate = DateTime.Now,
                POGroup = "QA",
                zoneInfo = new ZoneInfo()
                {
                    lat = "33",
                    lon = "-117",
                    radius = "100",
                    type = "2",
                    zid = "1",
                    action = "2"
                }
            });

            return Task.FromResult(list);
        }

        public Task<MTCommand> GetMTCommand(string pogroup, string command)
        {
            return Task.FromResult(new MTCommand()
            {
                Code = 23,
                DisplayName = "ZOD Update",
                Name = "zod",
                Message = string.Empty,
                Params = new List<MTCommandParam>()
            });
        }

        public Task<bool> UpdateCommandStatus(int commandId, int mtRequestId)
        {
            return Task.FromResult(true);
        }
        public Task<bool> UpdateCommandRetry(int commandId)
        {
            return Task.FromResult(true);
        }
        public Task<MTEnrollment> GetEnrollmentData(string OID)
        {
            return Task.FromResult(new MTEnrollment()
            {
                MTEnrollmentID = 1,
                OID = "ID2000014",
                ProcessStartDate = DateTime.Parse("05/04/2021 11:50"),
                MemoryClearSuccessful = true,
                RatePlanSuccessful = true,
                MotionSuccessful = true,
                TamperSuccessful = true,
                BatterySuccessful = true,
                ZoneSuccessful = true,
                CommunicationSuccessful = true,
                IsEnrollmentPending = true,
                AudioSuccessful = true,
                SuccessTime = DateTime.Parse(""),
                FailTime = DateTime.Parse("")
    
            });
        }

        public bool CheckEnrollmentStatus(MTEnrollment enrollmentData)
        {
            return true;
        }

        public Task<bool> CreateEvents(string OID, string pogroup, string serialNumber, bool enrollmentsuccess, string comments)
        {
            return Task.FromResult(true);
        }


    }

    class MTServiceAccesMock : IServiceAccess
    {
        public Task<string> GetAuthorizationTokenAsync()
        {
            return Task.FromResult("123");
        }
    }

    class MTServiceMock : IMTService
    {
        public Task<MTServiceResponse> SendMTCommandAsync(string serialnumber, MTCommand command)
        {
            return Task.FromResult(new MTServiceResponse()
            {
                Success = true
            });
        }
    }

    class MTCommandAttributesServiceMock : IMTCommandAttributesService
    {
        public Task<MTCommand> GetMTCommand(string command, string poGroup, Dictionary<string, string> userOptions)
        {
            MTCommand com = new MTCommand()
            {
                Code = 23,
                DisplayName = "ZOD Update",
                Name = "zod",
                Message = ZoneInfoBody(userOptions),
                Params = new List<MTCommandParam>()
            };

            return Task.FromResult(com);
        }

        private string ZoneInfoBody(Dictionary<string, string> options)
        {

            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            string value = options.TryGetValue("lat", out value) ? value : "0";
            builder.Append("\"lat\":" + value + ",");
            value = options.TryGetValue("lon", out value) ? value : "0";
            builder.Append("\"lng\":" + value + ",");
            value = options.TryGetValue("radius", out value) ? value : "0";
            builder.Append("\"radius\":" + value + ",");
            value = options.TryGetValue("type", out value) ? value : "0";
            builder.Append("\"type\":" + value + ",");
            value = options.TryGetValue("zid", out value) ? value : "0";
            builder.Append("\"zid\":" + value + ",");
            value = options.TryGetValue("action", out value) ? value : "0";
            builder.Append("\"action\":" + value + "");
            builder.Append("}");
            return builder.ToString();

        }


    }

    class MTCommandServiceMock : ICommandService
    {
        public Task<MTServiceResponse> SendMessageAsync(string oid, string serialNumber, string command, string poGroup, Dictionary<string, string> userOptions, string userName)
        {
            return Task.FromResult(new MTServiceResponse() 
            {
                 Success = true,
                 payload = new MTServicePayload() 
                 {
                    requestid = 0 
                 }
            });
        }
    }

}
