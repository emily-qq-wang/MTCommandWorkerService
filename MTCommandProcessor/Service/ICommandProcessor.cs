// <copyright file="ICommandProcessor.cs" company="Sentinel Offender Services LLC">
//   Copyright (c) 2021 All rights reserved. Unauthorized copying of this file, via any medium is strictly prohibited
//   Proprietary and confidential
// </copyright>
// <author>Sentinel Offender Services - Software Development</author>
// <date>2/17/2021</date>

namespace MTCommandProcessor.Service
{
    using System.Threading.Tasks;
    /// <summary>
    /// Defines the <see cref="ICommandProcessor" />.
    /// </summary>
    public interface ICommandProcessor
    {
        bool StartProcessor();

        Task<bool> Process();

        Task<bool> StopProcessor();


    }
}
