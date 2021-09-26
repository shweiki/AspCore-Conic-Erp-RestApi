using System;
using System.Collections.Generic;

#nullable disable

namespace Entities
{
    public partial class Driver
    {
   

        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime? DateofBirth { get; set; }
        public string Email { get; set; }
        public string PhoneNumber1 { get; set; }
        public string PhoneNumber2 { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public string Type { get; set; }
        public string Company { get; set; }
        public string Ssn { get; set; }
        public string Tag { get; set; }
        public string Pass { get; set; }
        public string DriverUserId { get; set; }
        public int IsActive { get; set; }


    }
}
