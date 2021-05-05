// <copyright file="MTServiceResponse.cs" company="Sentinel Offender Services LLC">
//   Copyright (c) 2021 All rights reserved. Unauthorized copying of this file, via any medium is strictly prohibited
//   Proprietary and confidential
// </copyright>
// <author>Sentinel Offender Services - Software Development</author>
// <date>2/17/2021</date>

namespace MTCommandProcessor.MultiTrak
{
    using System;

    /// <summary>
    /// Defines the <see cref="MTServiceResponse" />.
    /// </summary>
    public class MTServiceResponse
    {
        /// <summary>
        /// Defines the Success.
        /// </summary>
        public bool Success = false;

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public String message { get; set; }

        /// <summary>
        /// Gets or sets the statusCode.
        /// </summary>
        public int statusCode { get; set; }

        /// <summary>
        /// Gets or sets the mtCloudResponse.
        /// </summary>
        public MTCloudResponse mtCloudResponse { get; set; }

        /// <summary>
        /// Gets or sets the payload.
        /// </summary>
        public MTServicePayload payload { get; set; }
    }
}
