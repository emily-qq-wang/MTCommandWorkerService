// <copyright file="MTCommandAttributeService.cs" company="Sentinel Offender Services LLC">
//   Copyright (c) 2021 All rights reserved. Unauthorized copying of this file, via any medium is strictly prohibited
//   Proprietary and confidential
// </copyright>
// <author>Sentinel Offender Services - Software Development</author>
// <date>2/17/2021</date>

namespace MTCommandProcessor.MultiTrak
{
    using MTCommandProcessor.Data;
    using System;
    using System.Text;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="MTCommandAttributesService" />.
    /// </summary>
    public class MTCommandAttributesService : IMTCommandAttributesService
    {
        /// <summary>
        /// Defines the dnaRepository.
        /// </summary>
        private readonly IDNARepository dnaRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="MTCommandAttributesService"/> class.
        /// </summary>
        /// <param name="dnaRepository">The dnaRepository<see cref="IDNARepository"/>.</param>
        public MTCommandAttributesService(IDNARepository dnaRepository)
        {
            this.dnaRepository = dnaRepository;
        }

        /// <summary>
        /// The GetMTCommand.
        /// </summary>
        /// <param name="command">The command<see cref="string"/>.</param>
        /// <param name="poGroup">The poGroup<see cref="string"/>.</param>
        /// <param name="userOptions">The userOptions<see cref="Dictionary{string, string}"/>.</param>
        /// <returns>The <see cref="MTCommand"/>.</returns>
        public async Task<MTCommand> GetMTCommand(string command, string poGroup, Dictionary<string, string> userOptions)
        {
            // Get command Config from the database.  
            MTCommand mtCommand = await dnaRepository.GetMTCommand(poGroup, command);

            if (mtCommand != null)
            {
                // Ok, great you have the command
                // Combine agency defaults with user options 
                // Build the message to be sent to MT Service

                foreach (MTCommandParam p in mtCommand.Params)
                {
                    if (!String.IsNullOrEmpty(p.Name))
                    {
                        if (p.Type.ToLower() == "array")
                        {
                            
                            if (p.Name.ToLower().Equals("zones"))
                            {
                                
                                p.Value = ZoneInfoBody(userOptions);
                            }
                            
                        }
                        else if (userOptions.ContainsKey(p.Name.ToString()))
                        {
                            p.Value = userOptions[p.Name.ToString()];
                        }
                    }

                }

                //Build message
                mtCommand.Message = mtCommand.ToJSON();

            }

            return mtCommand;
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
}
