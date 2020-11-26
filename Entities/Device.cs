using System;
using System.Collections.Generic;

#nullable disable

namespace Entities
{
    public partial class Device
    {
        public Device()
        {
            MemberLogs = new HashSet<MemberLog>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }
        public int? Status { get; set; }
        public bool IsPrime { get; set; }
        public DateTime? LastSetDateTime { get; set; }
        public string Description { get; set; }

        public virtual ICollection<MemberLog> MemberLogs { get; set; }
    }
}
