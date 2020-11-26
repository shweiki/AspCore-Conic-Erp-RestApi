using System;
using System.Collections.Generic;

#nullable disable

namespace Entities
{
    public partial class MemberFace
    {
        public long Id { get; set; }
        public int FaceLength { get; set; }
        public string FaceStr { get; set; }
        public long MemberId { get; set; }

        public virtual Member Member { get; set; }
    }
}
