﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Entities
{
    public partial class CashPool
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public float TotalCash { get; set; } 
        public float TotalVisa { get; set; }
        public float TotalReject { get; set; }
        public float TotalOutlay { get; set; }
        public float Totalrestitution { get; set; }
        public DateTime DateTime { get; set; }
        public int Status { get; set; }
        public string TableName { get; set; }
        public long Fktable { get; set; }
    }
}
