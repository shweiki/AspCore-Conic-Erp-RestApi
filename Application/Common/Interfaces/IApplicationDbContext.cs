using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces;

public interface IApplicationDbContext
{
    // Administration

    DbSet<TreeAccount> TreeAccount { get; }
    DbSet<Setting> Setting { get; }
    DbSet<SystemConfiguration> SystemConfiguration { get; }
    DbSet<UserRouter> UserRouter { get; }
    DbSet<ActionLog> ActionLog { get; }
    DbSet<Area> Area { get; }
    DbSet<BackUp> BackUp { get; }
    DbSet<Bank> Bank { get; }
    DbSet<Cash> Cash { get; }
    DbSet<Cheque> Cheque { get; }
    DbSet<CompanyInfo> CompanyInfo { get; }
    DbSet<Device> Device { get; }
    DbSet<Discount> Discount { get; }
    DbSet<EditorsUser> EditorsUser { get; }
    DbSet<Visit> Visit { get; }
    DbSet<CashPool> CashPool { get; }
    DbSet<Driver> Driver { get; }
    DbSet<EntryAccounting> EntryAccounting { get; }
    DbSet<EntryMovement> EntryMovement { get; }
    DbSet<FileDatum> FileData { get; }

    DbSet<InventoryItem> InventoryItem { get; }
    DbSet<InventoryMovement> InventoryMovement { get; }
    DbSet<Item> Item { get; }
    DbSet<ItemMuo> ItemMuo { get; }
    DbSet<Massage> Massage { get; }
    DbSet<Member> Member { get; }
    DbSet<FingerPrint> FingerPrint { get; }

    DbSet<DeviceLog> DeviceLog { get; }
    DbSet<Membership> Membership { get; }
    DbSet<MembershipMovement> MembershipMovement { get; }
    DbSet<MembershipMovementOrder> MembershipMovementOrder { get; }
    DbSet<MenuItem> MenuItem { get; }
    DbSet<Oprationsy> Oprationsy { get; }
    DbSet<OrderInventory> OrderInventory { get; }
    DbSet<OriginItem> OriginItem { get; }
    DbSet<Payment> Payment { get; }
    DbSet<Receive> Receive { get; }
    DbSet<PurchaseInvoice> PurchaseInvoice { get; }
    DbSet<BillOfEntery> BillOfEntery { get; }
    DbSet<WorkShop> WorkShop { get; }
    DbSet<SalesInvoice> SalesInvoice { get; }
    DbSet<Service> Service { get; }
    DbSet<StockMovement> StockMovement { get; }
    DbSet<StocktakingInventory> StocktakingInventory { get; }
    DbSet<UnitItem> UnitItem { get; }
    DbSet<Vendor> Vendor { get; }
    DbSet<Report> Report { get; }
    DbSet<OrderDelivery> OrderDelivery { get; }
    DbSet<Employee> Employee { get; }
    DbSet<SalaryAdjustmentLog> SalaryAdjustmentLog { get; }
    DbSet<SalaryPayment> SalaryPayment { get; }
    DbSet<Adjustment> Adjustment { get; }
    DbSet<OrderRestaurant> OrderRestaurant { get; }



    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken, string userName);
    Task MigrateAsync();
    Task<int> SaveChangesAsync();
    int SaveChanges();
}
