// <copyright file="CommandProcessor.cs" company="Sentinel Offender Services LLC">
//   Copyright (c) 2021 All rights reserved. Unauthorized copying of this file, via any medium is strictly prohibited
//   Proprietary and confidential
// </copyright>
// <author>Sentinel Offender Services - Software Development</author>
// <date>2/17/2021</date>

namespace MTCommandProcessor.Service
{
    using System;
    using System.Collections.Generic;
    using System.Collections;
    using Microsoft.Extensions.Logging;
    using MTCommandProcessor.MultiTrak;
    using MTCommandProcessor.Data;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="CommandProcessor" />.
    /// </summary>
    public class CommandProcessor : ICommandProcessor
    {
        private List<MTPendingCommand> pendingCommands = new List<MTPendingCommand>();

        private readonly IDNARepository dnaRepository;
        private readonly ILogger<CommandProcessor> logger;
        private readonly ICommandService commandService;

        bool stopProcess = true;
        bool isProcessRunning = false;

        public CommandProcessor(IDNARepository dnaRepository, ILogger<CommandProcessor> logger, ICommandService commandService) 
        {
            this.dnaRepository = dnaRepository;
            this.logger = logger;
            this.commandService = commandService;
        }

        /// <summary>
        /// Processor Logic goes here
        /// </summary>
        /// <returns>The <see cref="bool"/>.</returns>
        public async Task<bool> Process()
        {
            // Get list of pending comands from DB
            List<MTPendingCommand> pendingCommandList = await dnaRepository.GetDelayedCommands();
            logger.LogInformation($"{pendingCommandList.Count} pending messages found");

            // Only once command is sent per device
            // If a device has more than one pending command, the next in queue is sent only
            // after the success message is received for the first command
            // This new queue only has command that wouyld be sent in this pass
            // Sort by Created Time
            pendingCommandList.Sort((p,q) => 
            {
                if (p.CreatedDate < q.CreatedDate) return -1;
                if (p.CreatedDate > q.CreatedDate) return 0;
                if (p.CreatedDate < q.CreatedDate) return -1;
                else return 0;
            });

            // Dictionary to keep track of serial numbers processed
            HashSet<string> processedSerialNumbers = new HashSet<string>();

            // Loop through the list
            foreach (MTPendingCommand command in pendingCommandList) 
            {
                logger.LogInformation($"Processing {command.SerialNumber} - {command.OID} - {command.Command}");
                if (!stopProcess)
                {
                    bool success = false;

                    if (processedSerialNumbers.Contains(command.SerialNumber.ToUpper()))
                    {
                        // Already processed a command for serial number
                        // We are in waiting mode
                        logger.LogInformation($"Cannot send this command in this pass. Already sent a command for this device {command.SerialNumber} : {command.OID} : {command.CommandID} : {command.Command}");
                        continue;
                    }
                    else
                    {


                        // The following logic is for zone updates. 

                        // Build options sending the zone command
                        Dictionary<string, string> options = new Dictionary<string, string>();
                        if (command.CommandID == 23)
                        {
                            options["lat"] = command.zoneInfo.lat;
                            options["lon"] = command.zoneInfo.lon;
                            options["radius"] = command.zoneInfo.radius;
                            options["type"] = command.zoneInfo.type;
                            options["zid"] = command.zoneInfo.mtzid;
                            options["action"] = command.zoneInfo.action;
                        }

                        //  Send the command to MT-Cloud via MT Service
                        MTServiceResponse response = await commandService.SendMessageAsync(command.OID, command.SerialNumber, command.Command, command.POGroup, options, string.Empty);

                        // Update command status in DNA
                        if (response.Success)
                        {
                            success = await dnaRepository.UpdateCommandStatus(command.PendingID, response.payload.requestid);
                        }
                        else
                        {
                            await dnaRepository.UpdateCommandRetry(command.PendingID);
                            // log the error
                            //someLoggingFunction(response.err);
                        }
                    }
                    if (success)
                    {
                        processedSerialNumbers.Add(command.SerialNumber.ToUpper());
                    }
                }
                else 
                {
                    break;
                }

            }
            isProcessRunning = false;
            return true;
        }

        /// <summary>
        /// Function called before starting  the process. 
        /// </summary>
        /// <returns>The <see cref="bool"/>.</returns>
        public bool StartProcessor()
        {
            stopProcess = false;
            isProcessRunning = true;
            return true;
        }

        /// <summary>
        /// Function called on process stop. Clean up here
        /// </summary>
        /// <returns>The <see cref="bool"/>.</returns>
        public async Task<bool> StopProcessor()
        {
            stopProcess = true;
            if (isProcessRunning) 
            {
                await Task.Delay(1000);
            }
            return true;
        }
    }
}
