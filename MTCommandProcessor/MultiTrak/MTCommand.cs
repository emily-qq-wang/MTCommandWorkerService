// <copyright file="MTCommand.cs" company="Sentinel Offender Services LLC">
//   Copyright (c) 2021 All rights reserved. Unauthorized copying of this file, via any medium is strictly prohibited
//   Proprietary and confidential
// </copyright>
// <author>Sentinel Offender Services - Software Development</author>
// <date>2/17/2021</date>

namespace MTCommandProcessor.MultiTrak
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Defines the <see cref="MTCommand" />.
    /// </summary>
    public class MTCommand
    {
        /// <summary>
        /// Gets or sets the Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the DisplayName.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the Code.
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Gets or sets the Message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the Params.
        /// </summary>
        public List<MTCommandParam> Params { get; set; }

        /// <summary>
        /// The ToJSON.
        /// </summary>
        /// <returns>The <see cref="string"/>.</returns>
        public string ToJSON()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            foreach (MTCommandParam p in Params)
            {
                if (!String.IsNullOrEmpty(p.Name))
                {
                    builder.AppendFormat("\"{0}\":", p.Name.ToLower());
                    if (p.Type.ToUpper().Equals("INTEGER"))
                    {
                        builder.AppendFormat("{0},", p.Value);
                    }
                    else if (p.Type.ToUpper().Equals("STRING"))
                    {
                        builder.AppendFormat("\"{0}\",", p.Value);
                    }
                    else if (p.Type.ToUpper().Equals("ARRAY"))
                    {
                        builder.AppendFormat("[{0}],", p.Value);

                    }
                }

            }
            if (builder.Length > 1)
            {
                builder.Remove(builder.Length - 1, 1);
            }

            builder.Append("}");
            return builder.ToString();
        }
    }
}
