using System;
using System.Collections.Generic;

#nullable disable

namespace Entities
{
    public partial class Oprationsy
    {
        public Oprationsy()
        {
            ActionLogs = new HashSet<ActionLog>();
        }

        public int Id { get; set; }
        public string OprationName { get; set; }
        public string TableName { get; set; }
        public int Status { get; set; }
        public int? ReferenceStatus { get; set; }
        public string OprationDescription { get; set; }
        public string ControllerName { get; set; }
        public string Color { get; set; }
        public string ClassName { get; set; }
        public string RoleName { get; set; }
        public string ArabicOprationDescription { get; set; }
        public string IconClass { get; set; }

        public virtual ICollection<ActionLog> ActionLogs { get; set; }
    }
}
