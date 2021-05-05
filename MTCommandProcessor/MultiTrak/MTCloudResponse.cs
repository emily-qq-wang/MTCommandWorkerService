// <copyright file="MTCloudResponse.cs" company="Sentinel Offender Services LLC">
//   Copyright (c) 2021 All rights reserved. Unauthorized copying of this file, via any medium is strictly prohibited
//   Proprietary and confidential
// </copyright>
// <author>Sentinel Offender Services - Software Development</author>
// <date>2/17/2021</date>

namespace MTCommandProcessor.MultiTrak
{
    using System;

    /// <summary>
    /// Defines the <see cref="MTCloudResponse" />.
    /// </summary>
    public class MTCloudResponse
    {
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public String message { get; set; }

        /// <summary>
        /// Gets or sets the status_code.
        /// </summary>
        public int status_code { get; set; }
    }
}
