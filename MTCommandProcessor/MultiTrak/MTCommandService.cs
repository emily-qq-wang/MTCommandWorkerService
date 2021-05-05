// <copyright file="MTCommandService.cs" company="Sentinel Offender Services LLC">
//   Copyright (c) 2021 All rights reserved. Unauthorized copying of this file, via any medium is strictly prohibited
//   Proprietary and confidential
// </copyright>
// <author>Sentinel Offender Services - Software Development</author>
// <date>2/17/2021</date>

namespace MTCommandProcessor.MultiTrak
{
    using Microsoft.Extensions.Logging;
    using MTCommandProcessor.Data;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="MTCommandService" />.
    /// </summary>
    public class MTCommandService : ICommandService
    {
        /// <summary>
        /// Defines the logger.
        /// </summary>
        private readonly ILogger<MTCommandService> logger;

        /// <summary>
        /// Defines the repository.
        /// </summary>
        private readonly IDNARepository repository;

        /// <summary>
        /// Defines the mtService.
        /// </summary>
        private readonly IMTService mtService;

        /// <summary>
        /// Defines the mtAttributesService.
        /// </summary>
        private readonly IMTCommandAttributesService mtAttributesService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MTCommandService"/> class.
        /// </summary>
        /// <param name="mtService">The mtService<see cref="IDNAMTService"/>.</param>
        /// <param name="mtAttributesService">The mtAttributesService<see cref="IMTCommandAttributesService"/>.</param>
        /// <param name="repository">The repository<see cref="IDNARepository"/>.</param>
        /// <param name="logger">The logger<see cref="ILogger{MTCommandService}"/>.</param>
        public MTCommandService(IMTService mtService, IMTCommandAttributesService mtAttributesService, IDNARepository repository, ILogger<MTCommandService> logger)
        {
            if (mtService == null)
            {
                throw new ArgumentNullException("MT Service is null");
            }

            if (mtAttributesService == null)
            {
                throw new ArgumentNullException("MT Attribute Service is null");
            }

            this.mtService = mtService;
            this.mtAttributesService = mtAttributesService;
            this.logger = logger;
            this.repository = repository;
        }

        /// <summary>
        /// The SendMessage.
        /// Throws ArgumentNullException and ArgumentException.
        /// </summary>
        /// <param name="oid">The oid<see cref="string"/>.</param>
        /// <param name="serialNumber">The serialNumber<see cref="string"/>.</param>
        /// <param name="command">The command<see cref="string"/>.</param>
        /// <param name="poGroup">The poGroup<see cref="string"/>.</param>
        /// <param name="userOptions">The userOptions<see cref="Dictionary{string, string}"/>.</param>
        /// <param name="userName">The userName<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        public async Task<MTServiceResponse> SendMessageAsync(string oid, string serialNumber, string command, string poGroup, Dictionary<string, string> userOptions, string userName)
        {
            MTServiceResponse mtServiceResponse = new MTServiceResponse();
            mtServiceResponse.Success = false;

            MTCommand mtCommand = new MTCommand();
            try
            {
                mtCommand = await mtAttributesService.GetMTCommand(command, poGroup, userOptions);

                // Command not supported
                if (mtCommand == null)
                {
                    logger.LogWarning("MT Command is not found");
                    return mtServiceResponse;
                }

                mtServiceResponse = await mtService.SendMTCommandAsync(serialNumber, mtCommand);
                
            }
            catch (Exception ex)
            {
                // Catches all errors thrown by previous services. 
                // Log error 
                logger.LogError("MT Command Service : SendMessage", ex);

                return mtServiceResponse;
            }

            return mtServiceResponse;
        }
    }
}
