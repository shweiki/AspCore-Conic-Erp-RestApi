using System;
using System.Collections.Generic;

#nullable disable

namespace Entities
{
    public partial class BackUp
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime? DateTime { get; set; }
        public string BackUpPath { get; set; }
        public string UserId { get; set; }
        public string DataBaseName { get; set; }
    }
}
