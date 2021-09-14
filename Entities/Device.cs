﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Entities
{
    public partial class Device
    {
        public Device()
        {
            MemberLogs = new HashSet<MemberLog>();
            WorkingHoursLogs = new HashSet<WorkingHoursLog>();
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
        public string MAC { get; set; }
        public int Port { get; set; }
        public int? Status { get; set; }
        public bool Feel { get; set; }
        public DateTime? LastSetDateTime { get; set; }
        public string Description { get; set; }

        public virtual ICollection<MemberLog> MemberLogs { get; set; }
        public virtual ICollection<WorkingHoursLog> WorkingHoursLogs { get; set; }
    }
}
