using System;
using Entities.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Entities
{
    public partial class ConicErpContext :  IdentityDbContext
    {
        public ConicErpContext()
        {
        }

        public ConicErpContext(DbContextOptions<ConicErpContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<UserRouter> UserRouter { get; set; }
        public virtual DbSet<ActionLog> ActionLogs { get; set; }
        public virtual DbSet<Area> Areas { get; set; }
        public virtual DbSet<BackUp> BackUps { get; set; }
        public virtual DbSet<Bank> Banks { get; set; }
        public virtual DbSet<Cash> Cashes { get; set; }
        public virtual DbSet<Cheque> Cheques { get; set; }
        public virtual DbSet<CompanyInfo> CompanyInfos { get; set; }
        public virtual DbSet<Device> Devices { get; set; }
        public virtual DbSet<Discount> Discounts { get; set; }
        public virtual DbSet<EditorsUser> EditorsUsers { get; set; }
        public virtual DbSet<Visit> Visits { get; set; }
        public virtual DbSet<CashPool> CashPools { get; set; }
        public virtual DbSet<Driver> Drivers { get; set; }
        public virtual DbSet<EntryAccounting> EntryAccountings { get; set; }
        public virtual DbSet<EntryMovement> EntryMovements { get; set; }
        public virtual DbSet<FileDatum> FileData { get; set; }
        public virtual DbSet<InventoryItem> InventoryItems { get; set; }
        public virtual DbSet<InventoryMovement> InventoryMovements { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<ItemMuo> ItemMuos { get; set; }
        public virtual DbSet<Massage> Massages { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<FingerPrint> FingerPrints { get; set; }
        public virtual DbSet<DeviceLog> DeviceLogs { get; set; }
        public virtual DbSet<Membership> Memberships { get; set; }
        public virtual DbSet<MembershipMovement> MembershipMovements { get; set; }
        public virtual DbSet<MembershipMovementOrder> MembershipMovementOrders { get; set; }
        public virtual DbSet<MenuItem> MenuItems { get; set; }
        public virtual DbSet<Oprationsy> Oprationsys { get; set; }
        public virtual DbSet<OrderInventory> OrderInventories { get; set; }
        public virtual DbSet<OriginItem> OriginItems { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Receive> Receives { get; set; }
        public virtual DbSet<PurchaseInvoice> PurchaseInvoices { get; set; }
        public virtual DbSet<WorkShop> WorkShops { get; set; }
        public virtual DbSet<SalesInvoice> SalesInvoices { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<StockMovement> StockMovements { get; set; }
        public virtual DbSet<StocktakingInventory> StocktakingInventories { get; set; }
        public virtual DbSet<UnitItem> UnitItems { get; set; }
        public virtual DbSet<Vendor> Vendors { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<OrderDelivery> OrderDeliveries { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<SalaryAdjustmentLog> SalaryAdjustmentLogs { get; set; }
        public virtual DbSet<SalaryPayment> SalaryPayments { get; set; }
        public virtual DbSet<Adjustment> Adjustments { get; set; }
    /*  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                 optionsBuilder.UseSqlServer(GetCon());            
            }

        }
        public string GetCon() {
            //   return "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=Conic_Erp;Integrated Security=True";
           return "Data Source="+GetServerName()+";Initial Catalog="+GetDataBaseName()+ ";Integrated Security=True; MultipleActiveResultSets=true; timeout=1000000";
          //  return "Data Source=tcp:aspcore-conic-erp-restapidbserver.database.windows.net,1433;Initial Catalog=AspCore-Conic-Erp-RestApi_db;User Id=taha;Password=()=>{Allah}";
        
        }
        public string GetServerName()
        {
        //    return "(localdb)\\mssqllocaldb";
            return ""+Environment.MachineName + "\\SQLEXPRESS";
        }
        public string GetDataBaseName()
        {
            //return "Conic_Erp";
            int lat = Environment.CurrentDirectory.LastIndexOf("\\")+1;
            string Name = Environment.CurrentDirectory.Substring(lat  ,( Environment.CurrentDirectory.Length - lat));
            return Name.Replace("-", "").ToUpper();
        }*/
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("Arabic_CI_AI");

            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new CashConfiguration());
            modelBuilder.ApplyConfiguration(new VendorConfiguration());
            modelBuilder.ApplyConfiguration(new InventoryItemConfiguration());

            modelBuilder
    .HasAnnotation("ProductVersion", "3.0.0")
    .HasAnnotation("Relational:MaxIdentifierLength", 128)
    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
            {
                b.Property<string>("Id")
                    .HasColumnType("nvarchar(450)");

                b.Property<string>("ConcurrencyStamp")
                    .IsConcurrencyToken()
                    ;

                b.Property<string>("Name")
                    .HasColumnType("nvarchar(256)")
                    .HasMaxLength(256);

                b.Property<string>("NormalizedName")
                    .HasColumnType("nvarchar(256)")
                    .HasMaxLength(256);

                b.HasKey("Id");

                b.HasIndex("NormalizedName")
                    .IsUnique()
                    .HasDatabaseName("RoleNameIndex")
                    .HasFilter("[NormalizedName] IS NOT NULL");

                b.ToTable("AspNetRoles");

                b.HasData(new
                {
                    Id = "2890efde-5d28-406d-ae5e-72576f74870f", //  Guid.NewGuid(),
                    ConcurrencyStamp = "f9fd5fb8-1ea9-4afd-8029-b8a027ee943f",
                    Name = "Developer",
                    NormalizedName = "DEVELOPER"
                },new {
                    Id = "f4c8a1a5-0530-41a3-9ae1-99d51857de42", //  Guid.NewGuid(),
                    ConcurrencyStamp = "4a1971c9-71fd-41d8-8cad-35176671d26a",
                    Name = "admin",
                    NormalizedName = "ADMIN"
             
            });
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<string>("ClaimType")
                    ;

                b.Property<string>("ClaimValue")
                    ;

                b.Property<string>("RoleId")
                    .IsRequired()
                    .HasColumnType("nvarchar(450)");

                b.HasKey("Id");

                b.HasIndex("RoleId");

                b.ToTable("AspNetRoleClaims");
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
            {
                b.Property<string>("Id")
                    .HasColumnType("nvarchar(450)");

                b.Property<int>("AccessFailedCount")
                    .HasColumnType("int");

                b.Property<string>("ConcurrencyStamp")
                    .IsConcurrencyToken()
                    ;

                b.Property<string>("Email")
                    .HasColumnType("nvarchar(256)")
                    .HasMaxLength(256);

                b.Property<bool>("EmailConfirmed")
                    .HasColumnType("bit");

                b.Property<bool>("LockoutEnabled")
                    .HasColumnType("bit");

                b.Property<DateTimeOffset?>("LockoutEnd")
                    .HasColumnType("datetimeoffset");

                b.Property<string>("NormalizedEmail")
                    .HasColumnType("nvarchar(256)")
                    .HasMaxLength(256);

                b.Property<string>("NormalizedUserName")
                    .HasColumnType("nvarchar(256)")
                    .HasMaxLength(256);

                b.Property<string>("PasswordHash")
                    ;

                b.Property<string>("PhoneNumber")
                    ;

                b.Property<bool>("PhoneNumberConfirmed")
                    .HasColumnType("bit");

                b.Property<string>("SecurityStamp")
                    ;

                b.Property<bool>("TwoFactorEnabled")
                    .HasColumnType("bit");

                b.Property<string>("UserName")
                    .HasColumnType("nvarchar(256)")
                    .HasMaxLength(256);

                b.HasKey("Id");

                b.HasIndex("NormalizedEmail")
                    .HasDatabaseName("EmailIndex");

                b.HasIndex("NormalizedUserName")
                    .IsUnique()
                    .HasDatabaseName("UserNameIndex")
                    .HasFilter("[NormalizedUserName] IS NOT NULL");

                b.ToTable("AspNetUsers");
                b.HasData(new
                {
                    Id = "2c4f9fbb-cefc-4217-909d-dad1b6afd726", //  Guid.NewGuid(),
                    UserName = "Developer",
                    NormalizedUserName = "DEVELOPER",
                    Email = "tahashweiki.1994@Gmail.com",
                    NormalizedEmail = "TAHASHWEIKI.1994@GMAIL.COM",
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAEAACcQAAAAEDlKsBqScI1exq/bzxvkvaDbqjeVK5MbABg6aA9S8KbO9QRBnSO79l9grdjvH9+gMg==",
                    SecurityStamp = "IZEQASXPA5Z6U7O2RPM32FSODDDDDIOW",
                    ConcurrencyStamp = "b41b9a0f-f0df-4826-a8cf-0733c0c94f56",
                    PhoneNumber = "00962788675843",
                    PhoneNumberConfirmed = true,
                    TwoFactorEnabled = false,
                    // LockoutEnd = Nullable,
                    LockoutEnabled = true,
                    AccessFailedCount = 0
                });
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int")
                    .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                b.Property<string>("ClaimType")
                    ;

                b.Property<string>("ClaimValue")
                    ;

                b.Property<string>("UserId")
                    .IsRequired()
                    .HasColumnType("nvarchar(450)");

                b.HasKey("Id");

                b.HasIndex("UserId");

                b.ToTable("AspNetUserClaims");
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
            {
                b.Property<string>("LoginProvider")
                    .HasColumnType("nvarchar(128)")
                    .HasMaxLength(128);

                b.Property<string>("ProviderKey")
                    .HasColumnType("nvarchar(128)")
                    .HasMaxLength(128);

                b.Property<string>("ProviderDisplayName")
                    ;

                b.Property<string>("UserId")
                    .IsRequired()
                    .HasColumnType("nvarchar(450)");

                b.HasKey("LoginProvider", "ProviderKey");

                b.HasIndex("UserId");

                b.ToTable("AspNetUserLogins");
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
            {
                b.Property<string>("UserId")
                    .HasColumnType("nvarchar(450)");

                b.Property<string>("RoleId")
                    .HasColumnType("nvarchar(450)");

                b.HasKey("UserId", "RoleId");

                b.HasIndex("RoleId");

                b.ToTable("AspNetUserRoles");
                b.HasData(new
                {
                    UserId = "2c4f9fbb-cefc-4217-909d-dad1b6afd726",
                    RoleId = "2890efde-5d28-406d-ae5e-72576f74870f"

                }, new {
                    UserId = "2c4f9fbb-cefc-4217-909d-dad1b6afd726",
                    RoleId = "f4c8a1a5-0530-41a3-9ae1-99d51857de42"
                });
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
            {
                b.Property<string>("UserId")
                    .HasColumnType("nvarchar(450)");

                b.Property<string>("LoginProvider")
                    .HasColumnType("nvarchar(128)")
                    .HasMaxLength(128);

                b.Property<string>("Name")
                    .HasColumnType("nvarchar(128)")
                    .HasMaxLength(128);

                b.Property<string>("Value")
                    ;

                b.HasKey("UserId", "LoginProvider", "Name");

                b.ToTable("AspNetUserTokens");
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
            {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
            {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
            {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
            {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
            {
                b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                    .WithMany()
                    .HasForeignKey("UserId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

////////////////////////////////////////////////////////////////

            modelBuilder.Entity<ActionLog>(entity =>
            {
                entity.ToTable("ActionLog");

                entity.HasIndex(e => e.OprationId, "IX_ActionLog_OprationID");

                entity.Property(e => e.Id);

                entity.Property(e => e.AccountId);

                entity.Property(e => e.BankId);

                entity.Property(e => e.CashId);

                entity.Property(e => e.ChequeId);

                entity.Property(e => e.Description);

                entity.Property(e => e.DiscountId);

                entity.Property(e => e.EntryId);

                entity.Property(e => e.InventoryItemId);

                entity.Property(e => e.InventoryMovementId);

                entity.Property(e => e.ItemsId);

                entity.Property(e => e.MemberId);

                entity.Property(e => e.MembershipId);

                entity.Property(e => e.MembershipMovementId);

                entity.Property(e => e.MembershipMovementOrderId);

                entity.Property(e => e.MenuId);

                entity.Property(e => e.OprationId);

                entity.Property(e => e.OrderInventoryId);

                entity.Property(e => e.OriginId);

                entity.Property(e => e.PaymentId);

                entity.Property(e => e.PostingDateTime);

                entity.Property(e => e.PurchaseInvoiceId);

                entity.Property(e => e.SalesInvoiceId);

                entity.Property(e => e.ServiceId);

                entity.Property(e => e.StockMovementId);

                entity.Property(e => e.StocktakingInventoryId);

                entity.Property(e => e.UnitId);

                entity.Property(e => e.UserId)
                    .IsRequired()
                    ;

                entity.Property(e => e.VendorId);

                entity.HasOne(d => d.Opration)
                    .WithMany(p => p.ActionLogs)
                    .HasForeignKey(d => d.OprationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ActionLog_Oprationsys");
            });

            modelBuilder.Entity<BackUp>(entity =>
            {
                entity.ToTable("BackUp");

                entity.Property(e => e.Id);

                entity.Property(e => e.BackUpPath)
                    .HasMaxLength(255)
                    ;

                entity.Property(e => e.DataBaseName)
                    .HasMaxLength(255)
                    ;

                entity.Property(e => e.DateTime);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    ;

                entity.Property(e => e.UserId)
                    ;
            });

            modelBuilder.Entity<Bank>(entity =>
            {
                entity.ToTable("Bank");

                entity.HasIndex(e => e.AccountId, "IX_Bank_AccountID");

                entity.Property(e => e.Id);

                entity.Property(e => e.AccountId);

                entity.Property(e => e.AccountType)
                    .HasMaxLength(100)
                    .IsFixedLength(true);

                entity.Property(e => e.BranchName).HasMaxLength(200);

                entity.Property(e => e.Currency)
                    .HasMaxLength(100)
                    .IsFixedLength(true);

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    ;

                entity.Property(e => e.Iban);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    ;

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Banks)
                    .HasForeignKey(d => d.AccountId)
                    .HasConstraintName("FK_Bank_Account");
            });

            modelBuilder.Entity<Cheque>(entity =>
            {
                entity.ToTable("Cheque");

                entity.HasIndex(e => e.VendorId, "IX_Cheque_VendorID");

                entity.Property(e => e.Id);

                entity.Property(e => e.BankAddress)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.BankName);

                entity.Property(e => e.Currency)
                    .HasMaxLength(100)
                    .IsFixedLength(true);

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    ;

                entity.Property(e => e.FakeDate);

                entity.Property(e => e.Payee);

                entity.Property(e => e.PaymentType)
                    .HasMaxLength(10)
                    .IsFixedLength(true);

                entity.Property(e => e.VendorId);

                entity.HasOne(d => d.Vendor)
                    .WithMany(p => p.Cheques)
                    .HasForeignKey(d => d.VendorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Cheque_Vendor");
            });

            modelBuilder.Entity<CompanyInfo>(entity =>
            {
                entity.ToTable("CompanyInfo");

                entity.Property(e => e.Id);

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    ;


                entity.Property(e => e.BusinessDescription);


                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    ;

                entity.Property(e => e.Fax)
                    .HasMaxLength(25)
                    ;

                entity.Property(e => e.Logo);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    ;

                entity.Property(e => e.NickName)
                    .HasMaxLength(50)
                    ;

                entity.Property(e => e.PhoneNumber1)
                    .HasMaxLength(25)
                    ;

                entity.Property(e => e.PhoneNumber2)
                    .HasMaxLength(25)
                    ;

                entity.Property(e => e.RateNumber)
                    .HasMaxLength(255)
                    ;



                entity.Property(e => e.TaxNumber)
                    .HasMaxLength(100)
                    ;

                entity.Property(e => e.Website)
                    .HasMaxLength(255)
                    ;
            });

            modelBuilder.Entity<Device>(entity =>
            {
                entity.ToTable("Device");

                entity.Property(e => e.Id);

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    ;

                entity.Property(e => e.Ip)
                    .IsRequired()
                    .HasMaxLength(255)
                    ;

                entity.Property(e => e.LastSetDateTime);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    ;
            });

            modelBuilder.Entity<Discount>(entity =>
            {
                entity.ToTable("Discount");

                entity.Property(e => e.Id);

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    ;

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    ;

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50)
                    ;
            });

            modelBuilder.Entity<EditorsUser>(entity =>
            {
                entity.ToTable("EditorsUser");

                entity.Property(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    ;
            });

            modelBuilder.Entity<EntryAccounting>(entity =>
            {
                entity.ToTable("EntryAccounting");

                entity.Property(e => e.Id);

                entity.Property(e => e.Description);

                entity.Property(e => e.FakeDate);

                entity.Property(e => e.Type)
                    .IsRequired()
                    ;
            });

            modelBuilder.Entity<EntryMovement>(entity =>
            {
                entity.ToTable("EntryMovement");

                entity.HasIndex(e => e.AccountId, "IX_EntryMovement_AccountID");

                entity.HasIndex(e => e.EntryId, "IX_EntryMovement_EntryID");

                entity.Property(e => e.Id);

                entity.Property(e => e.AccountId);

                entity.Property(e => e.Description);

                entity.Property(e => e.EntryId);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.EntryMovements)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EntryMovement_Account");

                entity.HasOne(d => d.Entry)
                    .WithMany(p => p.EntryMovements)
                    .HasForeignKey(d => d.EntryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_EntryMovement_EntryAccounting");
            });

            modelBuilder.Entity<FileDatum>(entity =>
            {
                entity.ToTable("FileData");

                entity.Property(e => e.Id);

                entity.Property(e => e.File)
                    .IsRequired()
                    ;

                entity.Property(e => e.FileType)
                    .IsRequired()
                    .HasMaxLength(255)
                    ;

                entity.Property(e => e.Fktable);

                entity.Property(e => e.TableName)
                    .IsRequired()
                    .HasMaxLength(255)
                    ;
            });



            modelBuilder.Entity<InventoryMovement>(entity =>
            {
                entity.ToTable("InventoryMovement");

                entity.HasIndex(e => e.InventoryItemId, "IX_InventoryMovement_InventoryItemID");

                entity.HasIndex(e => e.ItemsId, "IX_InventoryMovement_ItemsID");

                entity.HasIndex(e => e.OrderInventoryId, "IX_InventoryMovement_OrderInventoryID");

                entity.HasIndex(e => e.PurchaseInvoiceId, "IX_InventoryMovement_PurchaseInvoiceID");

                entity.HasIndex(e => e.SalesInvoiceId, "IX_InventoryMovement_SalesInvoiceID");

                entity.Property(e => e.Id);

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    ;

                entity.Property(e => e.InventoryItemId);

                entity.Property(e => e.ItemsId);

                entity.Property(e => e.OrderInventoryId);

                entity.Property(e => e.PurchaseInvoiceId);

                entity.Property(e => e.SalesInvoiceId);

                entity.Property(e => e.TypeMove)
                    .IsRequired()
                    .HasMaxLength(255)
                    ;

                entity.HasOne(d => d.InventoryItem)
                    .WithMany(p => p.InventoryMovements)
                    .HasForeignKey(d => d.InventoryItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InventoryMovement_InventoryItem");

                entity.HasOne(d => d.Items)
                    .WithMany(p => p.InventoryMovements)
                    .HasForeignKey(d => d.ItemsId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_InventoryMovement_Item");

                entity.HasOne(d => d.OrderInventory)
                    .WithMany(p => p.InventoryMovements)
                    .HasForeignKey(d => d.OrderInventoryId)
                    .HasConstraintName("FK_InventoryMovement_OrderInventory");

                entity.HasOne(d => d.PurchaseInvoice)
                    .WithMany(p => p.InventoryMovements)
                    .HasForeignKey(d => d.PurchaseInvoiceId)
                    .HasConstraintName("FK_InventoryMovement_PurchaseInvoice");

                entity.HasOne(d => d.SalesInvoice)
                    .WithMany(p => p.InventoryMovements)
                    .HasForeignKey(d => d.SalesInvoiceId)
                    .HasConstraintName("FK_InventoryMovement_SalesInvoice");
            });

            modelBuilder.Entity<Item>(entity =>
            {
                entity.ToTable("Item");

                entity.Property(e => e.Id);

                entity.Property(e => e.Barcode)
                    .HasMaxLength(255)
                    ;

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    ;

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    ;
            });

            modelBuilder.Entity<ItemMuo>(entity =>
            {
                entity.ToTable("ItemMUO");

                entity.HasIndex(e => e.ItemsId, "IX_ItemMUO_ItemsID");

                entity.HasIndex(e => e.MenuItemId, "IX_ItemMUO_MenuItemID");

                entity.HasIndex(e => e.OriginItemId, "IX_ItemMUO_OriginItemID");

                entity.HasIndex(e => e.UnitItemId, "IX_ItemMUO_UnitItemID");

                entity.Property(e => e.Id);

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    ;

                entity.Property(e => e.ItemsId);

                entity.Property(e => e.MenuItemId);

                entity.Property(e => e.OriginItemId);

                entity.Property(e => e.UnitItemId);

                entity.HasOne(d => d.Items)
                    .WithMany(p => p.ItemMuos)
                    .HasForeignKey(d => d.ItemsId)
                    .HasConstraintName("FK_ItemMUO_Item");

                entity.HasOne(d => d.MenuItem)
                    .WithMany(p => p.ItemMuos)
                    .HasForeignKey(d => d.MenuItemId)
                    .HasConstraintName("FK_ItemMUO_MenuItem");

                entity.HasOne(d => d.OriginItem)
                    .WithMany(p => p.ItemMuos)
                    .HasForeignKey(d => d.OriginItemId)
                    .HasConstraintName("FK_ItemMUO_OriginItem");

                entity.HasOne(d => d.UnitItem)
                    .WithMany(p => p.ItemMuos)
                    .HasForeignKey(d => d.UnitItemId)
                    .HasConstraintName("FK_ItemMUO_UnitItem");
            });

            modelBuilder.Entity<Massage>(entity =>
            {
                entity.ToTable("Massage");

                entity.Property(e => e.Id);

                entity.Property(e => e.Body)
                    .IsRequired()
                    ;

                entity.Property(e => e.Fktable);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(13)
                    ;

                entity.Property(e => e.SendDate);

                entity.Property(e => e.TableName)
                    .IsRequired()
                    .HasMaxLength(255)
                    ;

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(255)
                    ;
            });
      
            modelBuilder.Entity<Member>(entity =>
            {
                entity.ToTable("Member");

                entity.HasIndex(e => e.AccountId, "IX_Member_AccountID");

                entity.Property(e => e.Id);

                entity.Property(e => e.AccountId);

                entity.Property(e => e.DateofBirth);

                entity.Property(e => e.Description);

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    ;

                entity.Property(e => e.Name)
                    .IsRequired()
                    ;

                entity.Property(e => e.PhoneNumber1)
                    .HasMaxLength(13)
                    ;

                entity.Property(e => e.PhoneNumber2)
                    .HasMaxLength(13)
                    ;

                entity.Property(e => e.Ssn)
                    .IsRequired()
                    .HasMaxLength(50)
                    ;

                entity.Property(e => e.Tag)
                    .HasMaxLength(255)
                    ;

                entity.Property(e => e.Type)
                    .IsRequired()
                    ;

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Members)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Member_Account");
            });

            modelBuilder.Entity<DeviceLog>(entity =>
            {
                entity.ToTable("DeviceLog");

                entity.HasIndex(e => e.DeviceId, "IX_DeviceLog_DeviceID");

                entity.HasIndex(e => e.Status, "IX_DeviceLog_Status");

                entity.Property(e => e.Id);

                entity.Property(e => e.DateTime);

                entity.Property(e => e.Description);

                entity.Property(e => e.DeviceId);


                entity.Property(e => e.Type)
                    .IsRequired()
                    ;

                entity.HasOne(d => d.Device)
                    .WithMany(p => p.DeviceLogs)
                    .HasForeignKey(d => d.DeviceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DeviceLog_Device");

             
            });
            modelBuilder.Entity<FingerPrint>(entity =>
            {
                entity.ToTable("FingerPrint");


                entity.Property(e => e.Id);

                entity.Property(e => e.Str).IsRequired();


           
            });
            modelBuilder.Entity<Membership>(entity =>
            {
                entity.ToTable("Membership");

                entity.Property(e => e.Id);

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    ;

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    ;
            });

            modelBuilder.Entity<MembershipMovement>(entity =>
            {
                entity.ToTable("MembershipMovement");

                entity.HasIndex(e => e.MemberId, "IX_MembershipMovement_MemberID");

                entity.HasIndex(e => e.MembershipId, "IX_MembershipMovement_MembershipID");

                entity.Property(e => e.Id);

                entity.Property(e => e.Description);

                entity.Property(e => e.DiscountDescription)
                    .HasMaxLength(50)
                    ;

                entity.Property(e => e.EditorName);

                entity.Property(e => e.EndDate);

                entity.Property(e => e.MemberId);

                entity.Property(e => e.MembershipId);

                entity.Property(e => e.StartDate);

                entity.Property(e => e.Type)
                    .IsRequired()
                    ;

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.MembershipMovements)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MembershipMovement_Member");

                entity.HasOne(d => d.Membership)
                    .WithMany(p => p.MembershipMovements)
                    .HasForeignKey(d => d.MembershipId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MembershipMovement_Membership");
            });

            modelBuilder.Entity<MembershipMovementOrder>(entity =>
            {
                entity.ToTable("MembershipMovementOrder");

                entity.HasIndex(e => e.MemberShipMovementId, "IX_MembershipMovementOrder_MemberShipMovementID");

                entity.Property(e => e.Id);

                entity.Property(e => e.Description);

                entity.Property(e => e.EditorName);

                entity.Property(e => e.EndDate);

                entity.Property(e => e.MemberShipMovementId);

                entity.Property(e => e.StartDate);

                entity.Property(e => e.Type)
                    .HasMaxLength(50)
                    ;

                entity.HasOne(d => d.MemberShipMovement)
                    .WithMany(p => p.MembershipMovementOrders)
                    .HasForeignKey(d => d.MemberShipMovementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MembershipMovementOrder_MembershipMovement");
            });

            modelBuilder.Entity<MenuItem>(entity =>
            {
                entity.ToTable("MenuItem");

                entity.Property(e => e.Id);

                entity.Property(e => e.Description);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    ;
            });

            modelBuilder.Entity<Oprationsy>(entity =>
            {
                entity.ToTable("Oprationsys");

                entity.Property(e => e.Id);

                entity.Property(e => e.ArabicOprationDescription)
                    .HasMaxLength(255)
                    ;

                entity.Property(e => e.ClassName)
                    .HasMaxLength(255)
                    ;

                entity.Property(e => e.ControllerName)
                    .HasMaxLength(255)
                    ;

                entity.Property(e => e.IconClass)
                    .HasMaxLength(255)
                    ;

                entity.Property(e => e.OprationDescription)
                    .HasMaxLength(255)
                    ;

                entity.Property(e => e.OprationName)
                    .IsRequired()
                    .HasMaxLength(255)
                    ;

                entity.Property(e => e.RoleName).HasMaxLength(256);

                entity.Property(e => e.TableName)
                    .HasMaxLength(255)
                    ;
            });

            modelBuilder.Entity<OrderInventory>(entity =>
            {
                entity.ToTable("OrderInventory");

                entity.Property(e => e.Id);

                entity.Property(e => e.Description);

                entity.Property(e => e.FakeDate);

                entity.Property(e => e.OrderType)
                    .IsRequired()
                    .HasMaxLength(255)
                    ;
            });

            modelBuilder.Entity<OriginItem>(entity =>
            {
                entity.ToTable("OriginItem");

                entity.Property(e => e.Id);

                entity.Property(e => e.Description);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    ;
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.HasIndex(e => e.MemberId, "IX_Payment_MemberID");

                entity.HasIndex(e => e.VendorId, "IX_Payment_VendorID");

                entity.Property(e => e.Id);

                entity.Property(e => e.Description);

                entity.Property(e => e.EditorName);

                entity.Property(e => e.FakeDate);

                entity.Property(e => e.MemberId);

                entity.Property(e => e.Name);

                entity.Property(e => e.PaymentMethod)
                    .IsRequired()
                    .HasMaxLength(255)
                    ;

                entity.Property(e => e.Type);

                entity.Property(e => e.VendorId);

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.MemberId)
                    .HasConstraintName("FK_Payment_Member");

                entity.HasOne(d => d.Vendor)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.VendorId)
                    .HasConstraintName("FK_Payment_Vendor");
            });

            modelBuilder.Entity<PurchaseInvoice>(entity =>
            {
                entity.ToTable("PurchaseInvoice");

                entity.HasIndex(e => e.VendorId, "IX_PurchaseInvoice_VendorID");

                entity.Property(e => e.Id);

                entity.Property(e => e.AccountInvoiceNumber).HasMaxLength(50);

                entity.Property(e => e.Description);

                entity.Property(e => e.FakeDate);

                entity.Property(e => e.InvoicePurchaseDate);

                entity.Property(e => e.Name)
                    ;

                entity.Property(e => e.PaymentMethod)
                    .IsRequired()
                    .HasMaxLength(255)
                    ;

                entity.Property(e => e.VendorId);

                entity.HasOne(d => d.Vendor)
                    .WithMany(p => p.PurchaseInvoices)
                    .HasForeignKey(d => d.VendorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PurchaseInvoice_Vendor");
            });

            modelBuilder.Entity<SalesInvoice>(entity =>
            {
                entity.ToTable("SalesInvoice");

                entity.HasIndex(e => e.MemberId, "IX_SalesInvoice_MemberID");

                entity.HasIndex(e => e.VendorId, "IX_SalesInvoice_VendorID");

                entity.Property(e => e.Id);

                entity.Property(e => e.Description);

                entity.Property(e => e.FakeDate);

                entity.Property(e => e.MemberId);

                entity.Property(e => e.Name);

                entity.Property(e => e.PaymentMethod)
                    .IsRequired()
                    .HasMaxLength(255)
                    ;

                entity.Property(e => e.Type);

                entity.Property(e => e.VendorId);

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.SalesInvoices)
                    .HasForeignKey(d => d.MemberId)
                    .HasConstraintName("FK_SalesInvoice_Member");

                entity.HasOne(d => d.Vendor)
                    .WithMany(p => p.SalesInvoices)
                    .HasForeignKey(d => d.VendorId)
                    .HasConstraintName("FK_SalesInvoice_Vendor");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("Service");

                entity.HasIndex(e => e.ItemId, "IX_Service_ItemID");

                entity.Property(e => e.Id);

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    ;

                entity.Property(e => e.ItemId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    ;

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50)
                    ;

                entity.HasOne(d => d.Item)
                    .WithMany(p => p.Services)
                    .HasForeignKey(d => d.ItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Service_Item");
            });

            modelBuilder.Entity<StockMovement>(entity =>
            {
                entity.ToTable("StockMovement");

                entity.HasIndex(e => e.InventoryItemId, "IX_StockMovement_InventoryItemID");

                entity.HasIndex(e => e.ItemsId, "IX_StockMovement_ItemsID");

                entity.HasIndex(e => e.StocktakingInventoryId, "IX_StockMovement_StocktakingInventoryID");

                entity.Property(e => e.Id);

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    ;

                entity.Property(e => e.InventoryItemId);

                entity.Property(e => e.ItemsId);

                entity.Property(e => e.StocktakingInventoryId);

                entity.HasOne(d => d.InventoryItem)
                    .WithMany(p => p.StockMovements)
                    .HasForeignKey(d => d.InventoryItemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StockMovement_InventoryItem");

                entity.HasOne(d => d.Items)
                    .WithMany(p => p.StockMovements)
                    .HasForeignKey(d => d.ItemsId)
                    .HasConstraintName("FK_StockMovement_Item");

                entity.HasOne(d => d.StocktakingInventory)
                    .WithMany(p => p.StockMovements)
                    .HasForeignKey(d => d.StocktakingInventoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StockMovement_StocktakingInventory");
            });

            modelBuilder.Entity<StocktakingInventory>(entity =>
            {
                entity.ToTable("StocktakingInventory");

                entity.Property(e => e.Id);

                entity.Property(e => e.Description);

                entity.Property(e => e.FakeDate);
            });

            modelBuilder.Entity<UnitItem>(entity =>
            {
                entity.ToTable("UnitItem");

                entity.Property(e => e.Id);

                entity.Property(e => e.Description);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255)
                    ;
            });

   

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
