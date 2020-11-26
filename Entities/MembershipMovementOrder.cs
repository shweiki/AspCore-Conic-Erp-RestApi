using System;
using System.Collections.Generic;

#nullable disable

namespace Entities
{
    public partial class MembershipMovementOrder
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public string EditorName { get; set; }
        public long MemberShipMovementId { get; set; }

        public virtual MembershipMovement MemberShipMovement { get; set; }
    }
}
