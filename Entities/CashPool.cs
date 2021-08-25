using System;
using System.Collections.Generic;

#nullable disable

namespace Entities
{
    public partial class CashPool
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public float Total { get; set; }

        public float TotalCash { get; set; } 
        public float TotalCoins { get; set; } 
        public float TotalVisa { get; set; }
        public float TotalReject { get; set; }
        public float TotalOutlay { get; set; }
        public float TotalRestitution { get; set; }
        public DateTime DateTime { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public string EditorName { get; set; }
        public string TableName { get; set; }
        public string Fktable { get; set; }
    }
}
