// <copyright file="MTEnrollment.cs" company="Sentinel Offender Services LLC">
//   Copyright (c) 2021 All rights reserved. Unauthorized copying of this file, via any medium is strictly prohibited
//   Proprietary and confidential
// </copyright>
// <author>Sentinel Offender Services - Software Development</author>
// <date>5/3/2021</date>

namespace MTCommandProcessor.MultiTrak
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Defines the <see cref="MTEnrollment" />.
    /// </summary>
    public class MTEnrollment
    {
        /// <summary>
        /// Gets or sets the MTEnrollmentID.
        /// </summary>
        public int MTEnrollmentID { get; set; }

        /// <summary>
        /// Gets or sets the OID.
        /// </summary>
        public string OID { get; set; }

        /// <summary>
        /// Gets or sets the ProcessStartDate.
        /// </summary>
        public DateTime ProcessStartDate { get; set; }

        /// <summary>
        /// Gets or sets the MemoryClearSuccessful.
        /// </summary>
        public bool MemoryClearSuccessful { get; set; }

        /// <summary>
        /// Gets or sets the RatePlanSuccessful.
        /// </summary>
        public bool RatePlanSuccessful { get; set; }

        /// <summary>
        /// Gets or sets the MotionSuccessful.
        /// </summary>
        public bool MotionSuccessful { get; set; }

        /// <summary>
        /// Gets or sets the TamperSuccessful.
        /// </summary>
        public bool TamperSuccessful { get; set; }

        /// <summary>
        /// Gets or sets the BatterySuccessful.
        /// </summary>
        public bool BatterySuccessful { get; set; }

        /// <summary>
        /// Gets or sets the ZoneSuccessful.
        /// </summary>
        public bool ZoneSuccessful { get; set; }

        /// <summary>
        /// Gets or sets the AudioSuccessful.
        /// </summary>
        public bool AudioSuccessful { get; set; }

        /// <summary>
        /// Gets or sets the CommunicationSuccessful.
        /// </summary>
        public bool CommunicationSuccessful { get; set; }

        /// <summary>
        /// Gets or sets the IsEnrollmentPending.
        /// </summary>
        public bool IsEnrollmentPending { get; set; }

        /// <summary>
        /// Gets or sets the SuccessTime.
        /// </summary>
        public DateTime SuccessTime { get; set; }

        /// <summary>
        /// Gets or sets the FailTime.
        /// </summary>
        public DateTime FailTime { get; set; }
    }
}
