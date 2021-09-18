using System;
using System.Collections.Generic;

#nullable disable

namespace Entities
{
    public partial class ActionLog
    {
        public long Id { get; set; }
        public int OprationId { get; set; }
        public DateTime PostingDateTime { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public string TableName { get; set; }
        public string Fktable { get; set; }
        public int? InventoryItemId { get; set; }
        public long? StocktakingInventoryId { get; set; }
        public long? StockMovementId { get; set; }
        public long? OrderInventoryId { get; set; }
        public long? InventoryMovementId { get; set; }
        public int? UnitId { get; set; }
        public int? MenuId { get; set; }
        public int? OriginId { get; set; }
        public long? ItemsId { get; set; }
        public long? AccountId { get; set; }
        public long? VendorId { get; set; }
        public int? CashId { get; set; }
        public int? BankId { get; set; }
        public int? ChequeId { get; set; }
        public long? EntryId { get; set; }
        public long? SalesInvoiceId { get; set; }
        public long? PurchaseInvoiceId { get; set; }
        public long? WorkShopId { get; set; }
        public int? MembershipId { get; set; }
        public long? MemberId { get; set; }
        public long? MembershipMovementId { get; set; }
        public int? DiscountId { get; set; }
        public int? ServiceId { get; set; }
        public int? MembershipMovementOrderId { get; set; }
        public long? PaymentId { get; set; }
        public long? ReceiveId { get; set; }
        public long? OrderDeliveryId { get; set; }
        public long? AreaId { get; set; }
        public long? DriverId { get; set; }
        public virtual Oprationsy Opration { get; set; }
    }
}
