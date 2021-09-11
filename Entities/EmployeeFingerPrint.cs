using System;
using System.Collections.Generic;

#nullable disable

namespace Entities
{
    public partial class EmployeeFingerPrint
    {
        public long Id { get; set; }
        public string FingerPrint { get; set; }
        public long EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
