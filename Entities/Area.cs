using System;
using System.Collections.Generic;

#nullable disable

namespace Entities
{
    public partial class Area
    {
      //  public Area()
      //  {
        //     Vendors = new HashSet<Vendor>();
      //  }
        public long Id { get; set; }
        public string Adress1 { get; set; }
        public string Adress2 { get; set; }
        public string Adress3 { get; set; }
        public double? DelievryPrice { get; set; }
        public int Status { get; set; }
        public virtual List<Vendor> Vendors { get; set; }

    }
}
