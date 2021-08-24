using System;
using System.Collections.Generic;

#nullable disable

namespace Entities
{
    public partial class Report
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public string EmailSent { get; set; }
        public string Printer { get; set; }
        public string Html { get; set; }
        public string Icon { get; set; }
    }
}
