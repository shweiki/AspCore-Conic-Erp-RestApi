using System;
using System.Collections.Generic;

#nullable disable

namespace Entities
{
    public partial class Cheque
    {
        public long Id { get; set; }
        public long? ChequeNumber { get; set; }
        public DateTime FakeDate { get; set; }
        public double? ChequeAmount { get; set; }
        public string Payee { get; set; }
        public string PaymentType { get; set; }
        public string BankName { get; set; }
        public string BankAddress { get; set; }
        public int? Status { get; set; }
        public bool IsPrime { get; set; }
        public string Description { get; set; }
        public string Currency { get; set; }
        public long VendorId { get; set; }

        public virtual Vendor Vendor { get; set; }
    }
}
