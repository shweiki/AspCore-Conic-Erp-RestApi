using Entities;
using Entities.Interfaces;
using Entities.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ConicErpContext DB;


        public IBaseRepository<Account> Accounts { get; private set; }
        public IBaseRepository<Setting>     Settings { get; private set; }
        public IBaseRepository<UserRouter>   UserRouter { get; private set; }
        public IBaseRepository<ActionLog> ActionLogs { get; private set; }
        public IBaseRepository<Area> Areas { get; private set; }
        public IBaseRepository<BackUp> BackUps { get; private set; }
        public IBaseRepository<Bank> Banks { get; private set; }
        public IBaseRepository<Cash> Cashes { get; private set; }
        public IBaseRepository<Cheque> Cheques { get; private set; }
        public IBaseRepository<CompanyInfo> CompanyInfos { get; private set; }
        public IBaseRepository<Device> Devices { get; private set; }
        public IBaseRepository<Discount> Discounts { get; private set; }
        public IBaseRepository<EditorsUser> EditorsUsers { get; private set; }
        public IBaseRepository<Visit> Visits { get; private set; }
        public IBaseRepository<CashPool> CashPools { get; private set; }
        public IBaseRepository<Driver> Drivers { get; private set; }
        public IBaseRepository<EntryAccounting> EntryAccountings { get; private set; }
        public IBaseRepository<EntryMovement> EntryMovements { get; private set; }
        public IBaseRepository<FileDatum> FileData { get; private set; }
        public IBaseRepository<InventoryItem> InventoryItems { get; private set; }
        public IBaseRepository<InventoryMovement>  InventoryMovements { get; private set; }
        public IBaseRepository<Item> Items { get; private set; }
        public IBaseRepository<ItemMuo> ItemMuos { get; private set; }
        public IBaseRepository<Massage> Massages { get; private set; }
        public IBaseRepository<Member> Members { get; private set; }
        public IBaseRepository<FingerPrint> FingerPrints { get; private set; }
        public IBaseRepository<DeviceLog> DeviceLogs { get; private set; }
        public IBaseRepository<Membership> Memberships { get; private set; }
        public IBaseRepository<MembershipMovement> MembershipMovements { get; private set; }
        public IBaseRepository<MembershipMovementOrder> MembershipMovementOrders { get; private set; }
        public IBaseRepository<MenuItem> MenuItems { get; private set; }
        public IBaseRepository<Oprationsy> Oprationsys { get; private set; }
        public IBaseRepository<OrderInventory> OrderInventories { get; private set; }
        public IBaseRepository<OriginItem> OriginItems { get; private set; }
        public IBaseRepository<Payment> Payments { get; private set; }
        public IBaseRepository<Receive> Receives { get; private set; }
        public IBaseRepository<PurchaseInvoice> PurchaseInvoices { get; private set; }
        public IBaseRepository<BillOfEntery> BillOfEnterys { get; private set; }
        public IBaseRepository<WorkShop> WorkShops { get; private set; }
        public IBaseRepository<SalesInvoice> SalesInvoices { get; private set; }
        public IBaseRepository<Service> Services { get; private set; }
        public IBaseRepository<StockMovement> StockMovements { get; private set; }
        public IBaseRepository<StocktakingInventory> StocktakingInventories { get; private set; }
        public IBaseRepository<UnitItem> UnitItems { get; private set; }
        public IBaseRepository<Vendor> Vendors { get; private set; }
        public IBaseRepository<Report> Reports { get; private set; }
        public IBaseRepository<OrderDelivery> OrderDeliveries { get; private set; }
        public IBaseRepository<Employee> Employees { get; private set; }
        public IBaseRepository<SalaryAdjustmentLog> SalaryAdjustmentLogs { get; private set; }
        public IBaseRepository<SalaryPayment> SalaryPayments { get; private set; }
        public IBaseRepository<Adjustment> Adjustments { get; private set; }
        public IBaseRepository<OrderRestaurant> OrderRestaurants { get; private set; }

        public UnitOfWork(ConicErpContext DBcontext)
        {
            DB = DBcontext;


            Accounts = new BaseRepository<Account>();
            Settings = new BaseRepository<Setting>();
            UserRouter = new BaseRepository<UserRouter>();
            ActionLogs = new BaseRepository<ActionLog>();
            Areas = new BaseRepository<Area>();
            BackUps = new BaseRepository<BackUp>();
            Banks = new BaseRepository<Bank>();
            Cashes = new BaseRepository<Cash>();
            Cheques = new BaseRepository<Cheque>();
            CompanyInfos = new BaseRepository<CompanyInfo>();
            Devices = new BaseRepository<Device>();
            Discounts = new BaseRepository<Discount>();
            EditorsUsers = new BaseRepository<EditorsUser>();
            Visits = new BaseRepository<Visit>();
            CashPools = new BaseRepository<CashPool>();
            Drivers = new BaseRepository<Driver>();
            EntryAccountings = new BaseRepository<EntryAccounting>();
            EntryMovements = new BaseRepository<EntryMovement>();
            FileData = new BaseRepository<FileDatum>();
            InventoryItems = new BaseRepository<InventoryItem>();
            InventoryMovements = new BaseRepository<InventoryMovement>();
            Items = new BaseRepository<Item>();
            ItemMuos = new BaseRepository<ItemMuo>();
            Massages = new BaseRepository<Massage>();
            Members = new BaseRepository<Member>();
            FingerPrints = new BaseRepository<FingerPrint>();
            DeviceLogs = new BaseRepository<DeviceLog>();
            Memberships = new BaseRepository<Membership>();
            MembershipMovements = new BaseRepository<MembershipMovement>();
            MembershipMovementOrders = new BaseRepository<MembershipMovementOrder>();
            MenuItems = new BaseRepository<MenuItem>();
            Oprationsys = new BaseRepository<Oprationsy>();
            OrderInventories = new BaseRepository<OrderInventory>();
            OriginItems = new BaseRepository<OriginItem>();
            Payments = new BaseRepository<Payment>();
            Receives = new BaseRepository<Receive>();
            PurchaseInvoices = new BaseRepository<PurchaseInvoice>();
            BillOfEnterys = new BaseRepository<BillOfEntery>();
            WorkShops = new BaseRepository<WorkShop>();
            SalesInvoices = new BaseRepository<SalesInvoice>();
            Services = new BaseRepository<Service>();
            StockMovements = new BaseRepository<StockMovement>();
            StocktakingInventories = new BaseRepository<StocktakingInventory>();
            UnitItems = new BaseRepository<UnitItem>();
            Vendors = new BaseRepository<Vendor>();
            Reports = new BaseRepository<Report>();
            OrderDeliveries = new BaseRepository<OrderDelivery>();
            Employees = new BaseRepository<Employee>();
            SalaryAdjustmentLogs = new BaseRepository<SalaryAdjustmentLog>();
            SalaryPayments = new BaseRepository<SalaryPayment>();
            Adjustments = new BaseRepository<Adjustment>();
            OrderRestaurants = new BaseRepository<OrderRestaurant>();
    }

        public int Complete()
        {
            return DB.SaveChanges();
        }

        public void Dispose()
        {
            DB.Dispose();
        }
    }
}