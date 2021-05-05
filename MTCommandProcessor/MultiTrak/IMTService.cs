// <copyright file="IMTService.cs" company="Sentinel Offender Services LLC">
//   Copyright (c) 2021 All rights reserved. Unauthorized copying of this file, via any medium is strictly prohibited
//   Proprietary and confidential
// </copyright>
// <author>Sentinel Offender Services - Software Development</author>
// <date>2/17/2021</date>

namespace MTCommandProcessor.MultiTrak
{
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IMTService" />.
    /// </summary>
    public interface IMTService
    {
        /// <summary>
        /// The SendCommand.
        /// </summary>
        /// <param name="serialnumber">The serialnumber<see cref="string"/>.</param>
        /// <param name="command">The command<see cref="MTCommand"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        Task<MTServiceResponse> SendMTCommandAsync(string serialnumber, MTCommand command);
    }
}
