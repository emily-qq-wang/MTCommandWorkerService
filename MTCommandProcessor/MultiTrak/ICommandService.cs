// <copyright file="ICommandService.cs" company="Sentinel Offender Services LLC">
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
    /// Defines the <see cref="ICommandService" />.
    /// </summary>
    public interface ICommandService
    {
        /// <summary>
        /// The SendMessage.
        /// </summary>
        /// <param name="oid">The oid<see cref="string"/>.</param>
        /// <param name="serialNumber">The serialNumber<see cref="string"/>.</param>
        /// <param name="command">The command<see cref="string"/>.</param>
        /// <param name="poGroup">The poGroup<see cref="string"/>.</param>
        /// <param name="userOptions">The userOptions<see cref="Dictionary{string, string}"/>.</param>
        /// <param name="userName">The userName<see cref="string"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        Task<MTServiceResponse> SendMessageAsync(string oid, string serialNumber, string command, string poGroup, Dictionary<string, string> userOptions, string userName);
    }
}
