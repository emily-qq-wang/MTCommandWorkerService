// <copyright file="AuthServiceResponse.cs" company="Sentinel Offender Services LLC">
//   Copyright (c) 2021 All rights reserved. Unauthorized copying of this file, via any medium is strictly prohibited
//   Proprietary and confidential
// </copyright>
// <author>Sentinel Offender Services - Software Development</author>
// <date>2/17/2021</date>

namespace MTCommandProcessor.MultiTrak
{
    /// <summary>
    /// Defines the <see cref="AuthServiceResponse" />.
    /// </summary>
    public class AuthServiceResponse
    {
        /// <summary>
        /// Gets or sets a value indicating whether isSuccessful.
        /// </summary>
        public bool isSuccessful { get; set; }

        /// <summary>
        /// Gets or sets the jwt.
        /// </summary>
        public string jwt { get; set; }
    }
}
