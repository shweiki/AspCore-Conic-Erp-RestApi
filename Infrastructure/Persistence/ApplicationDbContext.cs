using Application.Common.Enums;
using Application.Common.Interfaces;
using Domain.Common;
using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using System.Reflection;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>, IApplicationDbContext
{
    public virtual DbSet<TreeAccount> TreeAccount => Set<TreeAccount>();
    public virtual DbSet<Audit> AuditLog => Set<Audit>();
    public virtual DbSet<SystemConfiguration> SystemConfiguration => Set<SystemConfiguration>();
    public virtual DbSet<Setting> Setting => Set<Setting>();
    public virtual DbSet<UserRouter> UserRouter => Set<UserRouter>();
    public virtual DbSet<ActionLog> ActionLog => Set<ActionLog>();
    public virtual DbSet<Area> Area => Set<Area>();
    public virtual DbSet<BackUp> BackUp => Set<BackUp>();
    public virtual DbSet<Bank> Bank => Set<Bank>();
    public virtual DbSet<Cash> Cash => Set<Cash>();
    public virtual DbSet<Cheque> Cheque => Set<Cheque>();
    public virtual DbSet<CompanyInfo> CompanyInfo => Set<CompanyInfo>();
    public virtual DbSet<Device> Device => Set<Device>();
    public virtual DbSet<Discount> Discount => Set<Discount>();
    public virtual DbSet<EditorsUser> EditorsUser => Set<EditorsUser>();
    public virtual DbSet<Visit> Visit => Set<Visit>();
    public virtual DbSet<CashPool> CashPool => Set<CashPool>();
    public virtual DbSet<Driver> Driver => Set<Driver>();
    public virtual DbSet<EntryAccounting> EntryAccounting => Set<EntryAccounting>();
    public virtual DbSet<EntryMovement> EntryMovement => Set<EntryMovement>();
    public virtual DbSet<FileDatum> FileData => Set<FileDatum>();
    public virtual DbSet<InventoryItem> InventoryItem => Set<InventoryItem>();
    public virtual DbSet<InventoryMovement> InventoryMovement => Set<InventoryMovement>();
    public virtual DbSet<Item> Item => Set<Item>();
    public virtual DbSet<ItemMuo> ItemMuo => Set<ItemMuo>();
    public virtual DbSet<Massage> Massage => Set<Massage>();
    public virtual DbSet<Member> Member => Set<Member>();
    public virtual DbSet<FingerPrint> FingerPrint => Set<FingerPrint>();
    public virtual DbSet<DeviceLog> DeviceLog => Set<DeviceLog>();
    public virtual DbSet<Membership> Membership => Set<Membership>();
    public virtual DbSet<MembershipMovement> MembershipMovement => Set<MembershipMovement>();
    public virtual DbSet<MembershipMovementOrder> MembershipMovementOrder => Set<MembershipMovementOrder>();
    public virtual DbSet<MenuItem> MenuItem => Set<MenuItem>();
    public virtual DbSet<Oprationsy> Oprationsy => Set<Oprationsy>();
    public virtual DbSet<OrderInventory> OrderInventory => Set<OrderInventory>();
    public virtual DbSet<OriginItem> OriginItem => Set<OriginItem>();
    public virtual DbSet<Payment> Payment => Set<Payment>();
    public virtual DbSet<Receive> Receive => Set<Receive>();
    public virtual DbSet<PurchaseInvoice> PurchaseInvoice => Set<PurchaseInvoice>();
    public virtual DbSet<BillOfEntery> BillOfEntery => Set<BillOfEntery>();
    public virtual DbSet<WorkShop> WorkShop => Set<WorkShop>();
    public virtual DbSet<SalesInvoice> SalesInvoice => Set<SalesInvoice>();
    public virtual DbSet<Service> Service => Set<Service>();
    public virtual DbSet<StockMovement> StockMovement => Set<StockMovement>();
    public virtual DbSet<StocktakingInventory> StocktakingInventory => Set<StocktakingInventory>();
    public virtual DbSet<UnitItem> UnitItem => Set<UnitItem>();
    public virtual DbSet<Vendor> Vendor => Set<Vendor>();
    public virtual DbSet<Report> Report => Set<Report>();
    public virtual DbSet<OrderDelivery> OrderDelivery => Set<OrderDelivery>();
    public virtual DbSet<Employee> Employee => Set<Employee>();
    public virtual DbSet<SalaryAdjustmentLog> SalaryAdjustmentLog => Set<SalaryAdjustmentLog>();
    public virtual DbSet<SalaryPayment> SalaryPayment => Set<SalaryPayment>();
    public virtual DbSet<Adjustment> Adjustment => Set<Adjustment>();
    public virtual DbSet<OrderRestaurant> OrderRestaurant => Set<OrderRestaurant>();


    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<User>().HasQueryFilter(p => !p.IsDeleted);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.UseCollation("Arabic_CI_AI");

        base.OnModelCreating(modelBuilder);
    }
    private void OnBeforeSaveChanges(string userName = "System User")
    {
        ChangeTracker.DetectChanges();
        var auditEntries = new List<AuditEntry>();
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is Audit || entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                continue;

            var auditEntry = new AuditEntry(entry);
            auditEntry.TableName = entry.Entity.GetType().Name;
            auditEntry.UserId = string.IsNullOrWhiteSpace(userName) ? "System User" : userName;
            auditEntries.Add(auditEntry);
            foreach (var property in entry.Properties)
            {
                string propertyName = property.Metadata.Name;
                if (property.Metadata.IsPrimaryKey())
                {
                    auditEntry.KeyValues[propertyName] = property.CurrentValue;
                    continue;
                }

                switch (entry.State)
                {
                    case EntityState.Added:
                        auditEntry.AuditActionType = AuditActionType.Create;
                        auditEntry.NewValues[propertyName] = property.CurrentValue;
                        break;

                    case EntityState.Deleted:
                        auditEntry.AuditActionType = AuditActionType.Delete;
                        auditEntry.OldValues[propertyName] = property.OriginalValue;
                        break;

                    case EntityState.Modified:
                        if (property.IsModified)
                        {
                            auditEntry.ChangedColumns.Add(propertyName);
                            auditEntry.AuditActionType = AuditActionType.Update;
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                        }
                        break;
                }
            }
        }
        foreach (var auditEntry in auditEntries)
        {
            AuditLog.Add(auditEntry.ToAudit());
        }
    }
    private void PrepareSaveChanges(string userName = "System User")
    {
        foreach (var entry in ChangeTracker.Entries<AuditEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = userName;
                    entry.Entity.Created = DateTime.UtcNow;
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = userName;
                    entry.Entity.LastModified = DateTime.UtcNow;
                    break;
            }
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        PrepareSaveChanges();

        var result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }
    public virtual async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken(), string userName = null)
    {
        PrepareSaveChanges(userName);
        OnBeforeSaveChanges(userName);

        var result = await base.SaveChangesAsync(cancellationToken);
        return result;
    }

    public virtual async Task<int> SaveChangesAsync()
    {
        PrepareSaveChanges();

        var result = await base.SaveChangesAsync();
        return result;
    }
    public override int SaveChanges()
    {
        PrepareSaveChanges();

        return base.SaveChanges();
    }

    public async Task MigrateAsync()
    {
        await Database.MigrateAsync();
    }
}
