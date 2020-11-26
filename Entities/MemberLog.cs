using System;
using System.Collections.Generic;

#nullable disable

namespace Entities
{
    public partial class MemberLog
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public DateTime DateTime { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public long MemberId { get; set; }
        public long DeviceId { get; set; }

        public virtual Device Device { get; set; }
        public virtual Member Member { get; set; }
    }
}
