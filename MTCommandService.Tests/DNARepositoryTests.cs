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
    public class DNARepositoryTests
    {
        IServiceProvider services;

        [SetUp]
        public void Setup()
        {
            services = Host.CreateDefaultBuilder(new string[] { })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddHttpClient();
                services.AddSingleton<IDNARepository, DNARepository>();
            }).Build().Services;
        }


        [Test]
        public async Task MTCommandService_GetMTPingCommand()
        {
            var dnaRepository = services.GetRequiredService<IDNARepository>();

            var response = await dnaRepository.GetMTCommand("QA", "ping");
            Assert.IsTrue(response.Name.Equals("ping"));
            Assert.IsTrue(response.Params.Count == 1);
        }

        [Test]
        public async Task MTCommandService_GetMTZODCommand()
        {
            var dnaRepository = services.GetRequiredService<IDNARepository>();

            var response = await dnaRepository.GetMTCommand("QA", "zod");
            Assert.IsTrue(response.Name.Equals("zod"));
            Assert.IsTrue(response.Params[0].Type.Equals("Array"));
        }

        [Test]
        public async Task MTCommandService_GetDelayedCommands() 
        {
            var dnaRepository = services.GetRequiredService<IDNARepository>();
            // Insert a value here

            var response = await dnaRepository.GetDelayedCommands();
            MTPendingCommand command = response[0];
            Assert.IsTrue(response.Count > 0);
            Assert.IsTrue(command.OID == "ID901713");
            Assert.IsTrue(command.SerialNumber == "MT-DEVSIM-3");
            //Assert.IsTrue(command.zoneInfo.lat.Equals("33.669096374372636"));
        }

        [Test]
        public async Task MTCommandService_UpdateCommandStatus()
        {
            var dnaRepository = services.GetRequiredService<IDNARepository>();
            // Insert a value here

            var response = await dnaRepository.GetDelayedCommands();
            MTPendingCommand command = response[0];

            var updateresponse = await dnaRepository.UpdateCommandStatus(command.PendingID, 10);
            Assert.IsTrue(updateresponse);                    
        }

        [Test]
        public async Task MTCommandService_GetEnrollmentData()
        {
            var dnaRepository = services.GetRequiredService<IDNARepository>();

            var response = await dnaRepository.GetEnrollmentData("ID2000014");
            Assert.IsTrue(response.IsEnrollmentPending.Equals(true));
        }

        [Test]
        public void MTCommandService_CheckEnrollmentStatus()
        {
            var dnaRepository = services.GetRequiredService<IDNARepository>();
            MTEnrollment enroll = new MTEnrollment();
            {
                enroll.MTEnrollmentID = 1;
                enroll.OID = "ID2000014";
                enroll.ProcessStartDate = DateTime.Parse("05/05/2021 5:36 PM");
                enroll.MemoryClearSuccessful = true;
                enroll.RatePlanSuccessful = true;
                enroll.MotionSuccessful = true;
                enroll.TamperSuccessful = true;
                enroll.BatterySuccessful = true;
                enroll.ZoneSuccessful = true;
                enroll.CommunicationSuccessful = true;
                enroll.IsEnrollmentPending = true;
                enroll.AudioSuccessful = true;
                enroll.SuccessTime = DateTime.Parse("");
                enroll.FailTime = DateTime.Parse("");
            }
            bool response = dnaRepository.CheckEnrollmentStatus(enroll);
            Assert.IsTrue(response);
        }

        [Test]
        public async Task MTCommandService_CreateEvents()
        {
            var dnaRepository = services.GetRequiredService<IDNARepository>();
            var response = await dnaRepository.CreateEvents("ID2000014","QA", "MT-LOAD-DEV9",true,"success Enrolled");
            Assert.IsTrue(response);
        }
    }
}
