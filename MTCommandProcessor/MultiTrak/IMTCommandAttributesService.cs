// <copyright file="IMTCommandAttributesService.cs" company="Sentinel Offender Services LLC">
//   Copyright (c) 2021 All rights reserved. Unauthorized copying of this file, via any medium is strictly prohibited
//   Proprietary and confidential
// </copyright>
// <author>Sentinel Offender Services - Software Development</author>
// <date>2/17/2021</date>

namespace MTCommandProcessor.MultiTrak
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IMTCommandAttributesService" />.
    /// </summary>
    public interface IMTCommandAttributesService
    {
        /// <summary>
        /// The GetMTCommand.
        /// </summary>
        /// <param name="command">The command<see cref="string"/>.</param>
        /// <param name="poGroup">The poGroup<see cref="string"/>.</param>
        /// <param name="userOptions">The userOptions<see cref="Dictionary{string, string}"/>.</param>
        /// <returns>The <see cref="MTCommand"/>.</returns>
        Task<MTCommand> GetMTCommand(string command, string poGroup, Dictionary<string, string> userOptions);
    }
}
