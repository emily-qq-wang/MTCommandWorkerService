using MTCommandProcessor.MultiTrak;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MTCommandProcessor.Data
{
    public class DNARepository : IDNARepository
    {

        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly ILogger<DNARepository> logger;

        /// <summary>
        /// Defines the configuration.
        /// </summary>
        private readonly IConfiguration configuration;

        private string connectionString {get;set;}

        public DNARepository(ILogger<DNARepository> logger, IConfiguration configuration) 
        {
            this.configuration = configuration;
            this.logger = logger;
            connectionString = configuration.GetSection("dna")["connstring"];
        }
        public async Task<List<MTPendingCommand>> GetDelayedCommands()
        {
            string proc = "mtcommandparser_getpendingcommands";

            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    await conn.OpenAsync();
                    var data = await conn.QueryAsync(proc, commandType: CommandType.StoredProcedure);

                    List<MTPendingCommand> pendingCommands = new List<MTPendingCommand>();
                    foreach (dynamic t in data) 
                    {
                        try
                        {
                            MTPendingCommand p = new MTPendingCommand()
                            {
                                PendingID = t.systemid,
                                CommandID = t.commandcode,
                                OID = t.oid,
                                Command = t.Command,
                                SerialNumber = t.serialnumber,
                                CreatedDate = t.createddate,
                                POGroup = t.pogroupnum,
                                zoneInfo = new ZoneInfo() 
                                {
                                    lat = t.Lat != null ? t.Lat.ToString() : "",
                                    lon = t.Lon != null ? t.Lon.ToString() : "",
                                    radius = t.Radius != null ? t.Radius.ToString() : "",
                                    type = t.zonetype != null ? t.zonetype.ToString() : "",
                                    zid = t.Zoneid != null ? t.Zoneid.ToString() : "",
                                    mtzid = t.MTZoneId != null ? t.MTZoneId.ToString() : "",
                                    action = t.zoneaction != null? t.zoneaction.ToString(): ""
                                }
                            };                            
                            pendingCommands.Add(p);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    conn.Close();
                    return pendingCommands;
                }
                catch (Exception ex)
                {
                    logger.LogError("GetDelayedCommands", ex);
                    return null;
                }
            }

            
        }

        public async Task<MTCommand> GetMTCommand(string pogroup, string command)
        {
            string proc = "dash_getMTCommandParams";

            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    await conn.OpenAsync();
                    var data = await conn.QueryAsync(proc, new { command = command, pogroupnum = pogroup }, commandType: CommandType.StoredProcedure);

                    MTCommand mtCommand = new MTCommand() { Code = -1 };
                    List<MTCommandParam> Params = new List<MTCommandParam>();
                    foreach (dynamic t in data)
                    {
                        if (mtCommand.Code < 0)
                        {
                            mtCommand.Code = t.commandCode;
                        }
                        if (String.IsNullOrEmpty(mtCommand.Name))
                        {
                            mtCommand.Name = t.commandName;
                        }
                        if (String.IsNullOrEmpty(mtCommand.DisplayName))
                        {
                            mtCommand.DisplayName = t.DisplayName;
                        }

                        Params.Add
                        (
                            new MTCommandParam()
                            {
                                Name = t.Configuration,
                                Type = t.ValueType,
                                Value = t.ParamValue
                            }
                        );
                    }
                    mtCommand.Params = Params;

                    
                    conn.Close();
                    return mtCommand;
                }
                catch (Exception ex)
                {
                    logger.LogError("GetDelayedCommands", ex);
                    return null;
                }
            }
        }

        public async Task<bool> UpdateCommandStatus(int commandId, int mtRequestId)
        {
            string proc = "mtcommandparser_updatecommandid";

            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    await conn.OpenAsync();
                    var res = await conn.ExecuteAsync(proc, new { pendingid = commandId, mtrequestid = mtRequestId }, commandType: CommandType.StoredProcedure);                   

                    conn.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    logger.LogError("UpdateCommandStatus", ex);
                    return false;
                }
            }
        }

        public async Task<bool> UpdateCommandRetry(int commandId)
        {
            string proc = "mtcommandparser_commandidfailed";

            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    await conn.OpenAsync();
                    var res = await conn.ExecuteAsync(proc, new { pendingid = commandId }, commandType: CommandType.StoredProcedure);

                    conn.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    logger.LogError("UpdateCommandStatus", ex);
                    return false;
                }
            }
        }

        public async Task<MTEnrollment> GetEnrollmentData(string OID)
        {
            string proc = "mtcommandparser_getMTEnrollment";

            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    await conn.OpenAsync();
                    var data = await conn.QueryAsync(proc, new { OID =OID},commandType: CommandType.StoredProcedure);
                    MTEnrollment enroll = new MTEnrollment();

                    foreach (dynamic t in data)
                    {
                        try
                        {
                            enroll.MTEnrollmentID = t.MTEnrollmentID;
                            enroll.OID = t.OID;
                            enroll.ProcessStartDate = t.ProcessStartDate;
                            enroll.MemoryClearSuccessful = Convert.ToBoolean(t.MemoryClearSuccessful);
                            enroll.RatePlanSuccessful = Convert.ToBoolean(t.RatePlanSuccessful);
                            enroll.MotionSuccessful = Convert.ToBoolean(t.MotionSuccessful);
                            enroll.TamperSuccessful = Convert.ToBoolean(t.TamperSuccessful);
                            enroll.BatterySuccessful = Convert.ToBoolean(t.BatterySuccessful);
                            enroll.ZoneSuccessful = Convert.ToBoolean(t.ZoneSuccessful);
                            enroll.CommunicationSuccessful = Convert.ToBoolean(t.CommunicationSuccessful);
                            enroll.IsEnrollmentPending = Convert.ToBoolean(t.IsEnrollmentPending);
                            enroll.AudioSuccessful = Convert.ToBoolean(t.AudioSuccessful);
                            enroll.SuccessTime = t.SuccessTime;
                            enroll.FailTime = t.FailTime;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    conn.Close();
                    return enroll;
                }
                catch (Exception ex)
                {
                    logger.LogError("GetEnrollmentData", ex);
                    return null;
                }
            }
        }

        public bool CheckEnrollmentStatus(MTEnrollment enrollmentData)
        {
            try{
                DateTime utcCurrent = DateTime.UtcNow;
                DateTime start = enrollmentData.ProcessStartDate;
                var timediff = utcCurrent.Subtract(start).TotalMinutes;
                bool MemoryClearSuccessful = enrollmentData.MemoryClearSuccessful;
                bool RatePlanSuccessful = enrollmentData.RatePlanSuccessful;
                bool MotionSuccessful = enrollmentData.MotionSuccessful;
                bool TamperSuccessful = enrollmentData.TamperSuccessful;
                bool BatterySuccessful = enrollmentData.BatterySuccessful;
                bool ZoneSuccessful = enrollmentData.ZoneSuccessful;
                bool AudioSuccessful = enrollmentData.AudioSuccessful;
                bool CommunicationSuccessful = enrollmentData.CommunicationSuccessful;
                bool IsEnrollmentPending = enrollmentData.IsEnrollmentPending;
                if(MemoryClearSuccessful && RatePlanSuccessful && MotionSuccessful && TamperSuccessful && BatterySuccessful && ZoneSuccessful && AudioSuccessful && CommunicationSuccessful && IsEnrollmentPending && timediff <= 5)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("CheckEnrollmentStatus", ex);
                return false;
            }
            
        }

        public async Task<bool> CreateEvents(string OID, string pogroup,string serialNumber, bool enrollmentsuccess,string comments)
        {
            string proc = "mtcommandparser_AddMTEnrollmentEvent";
            string EventID;
            string EventDescription;
            DateTime EventDateTime = DateTime.UtcNow;
            if (enrollmentsuccess)
            {
                EventID = "SYS472";
                EventDescription = "Enrollment Complete";
            }
            else
            {
                EventID = "SYS473";
                EventDescription = "Enrollment Failed";
            }
            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    await conn.OpenAsync();
                    var data = await conn.QueryAsync(proc, new { 
                        Agency = pogroup, 
                        OID = OID,
                        EventID = EventID,
                        EventDescription = EventDescription,
                        EventDateTime = EventDateTime,
                        EquipmentID = serialNumber,
                        Comments = comments
                        }, 
                        commandType: CommandType.StoredProcedure);
                    MTEnrollment enrollData = new MTEnrollment();
                    bool resp = false;
                    foreach (dynamic t in data)
                    {
                        try
                        {
                            resp = Convert.ToBoolean(t.Success);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    conn.Close();
                    return resp;
                }
                catch (Exception ex)
                {
                    logger.LogError("CreateEvents", ex);
                    return false;
                }
            }
        }


    }
}
