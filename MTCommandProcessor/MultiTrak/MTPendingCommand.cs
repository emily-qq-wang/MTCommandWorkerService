using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace MTCommandProcessor.MultiTrak
{
    public class MTPendingCommand  : IComparable<MTPendingCommand>
    {
        public int PendingID { get; set; }
        public int CommandID { get; set; }
        public string OID { get; set; }

        public string Command { get; set; }

        public string SerialNumber { get; set; }

        public DateTime CreatedDate { get; set; }

        public string POGroup { get; set; }

        public ZoneInfo zoneInfo { get; set; }

        public int CompareTo([AllowNull] MTPendingCommand other)
        {
            throw new NotImplementedException();
        }
    }
}
