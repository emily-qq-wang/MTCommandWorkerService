// <copyright file="IDNARepository.cs" company="Sentinel Offender Services LLC">
//   Copyright (c) 2021 All rights reserved. Unauthorized copying of this file, via any medium is strictly prohibited
//   Proprietary and confidential
// </copyright>
// <author>Sentinel Offender Services - Software Development</author>
// <date>2/17/2021</date>

namespace MTCommandProcessor.Data
{
    using MTCommandProcessor.MultiTrak;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IDNARepository" />.
    /// </summary>
    public interface IDNARepository
    {
        /// <summary>
        /// The GetMTCommand.
        /// </summary>
        /// <param name="pogroup">The pogroup<see cref="string"/>.</param>
        /// <param name="command">The command<see cref="string"/>.</param>
        /// <returns>The <see cref="MTCommand"/>.</returns>
        Task<MTCommand> GetMTCommand(string pogroup, string command);

        Task<List<MTPendingCommand>> GetDelayedCommands();

        Task<bool> UpdateCommandStatus(int commandId, int mtRequestId);

        Task<bool> UpdateCommandRetry(int commandId);

        Task<MTEnrollment> GetEnrollmentData(string OID);

        bool CheckEnrollmentStatus(MTEnrollment enrollmentData);

        Task<bool> CreateEvents(string OID, string pogroup, string serialNumber, bool enrollmentsuccess, string comments);
    }
}
