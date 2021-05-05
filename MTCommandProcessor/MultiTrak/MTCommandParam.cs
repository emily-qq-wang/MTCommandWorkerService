// <copyright file="MTCommandParam.cs" company="Sentinel Offender Services LLC">
//   Copyright (c) 2021 All rights reserved. Unauthorized copying of this file, via any medium is strictly prohibited
//   Proprietary and confidential
// </copyright>
// <author>Sentinel Offender Services - Software Development</author>
// <date>2/17/2021</date>

namespace MTCommandProcessor.MultiTrak
{
    /// <summary>
    /// Defines the <see cref="MTCommandParam" />.
    /// </summary>
    public class MTCommandParam
    {
        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Type
        /// Type = Integer or String.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the Value
        /// ///.
        /// </summary>
        public string Value { get; set; }
    }
}
