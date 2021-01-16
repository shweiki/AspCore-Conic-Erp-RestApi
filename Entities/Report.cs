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
        public string Json { get; set; }
        public string HtmlDesgin { get; set; }
        public string Printer { get; set; }
        public string TableName { get; set; }
        public long Fktable { get; set; }

    }
}
