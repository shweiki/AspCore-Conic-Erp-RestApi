		DROP TABLE [Conic_Erp].[dbo].[InventoryMovement] 
		DROP TABLE [Conic_Erp].[dbo].[StockMovement] 
		DROP TABLE [Conic_Erp].[dbo].[StocktakingInventory] 
	    DROP TABLE [Conic_Erp].[dbo].[ItemMUO] 
	    DROP TABLE [Conic_Erp].[dbo].[MenuItem] 
		DROP TABLE [Conic_Erp].[dbo].[OrderInventory] 
		DROP TABLE [Conic_Erp].[dbo].[OriginItem] 
		DROP TABLE [Conic_Erp].[dbo].[UnitItem] 
		DROP TABLE [Conic_Erp].[dbo].[InventoryItem] 


		DROP TABLE [Conic_Erp].[dbo].[PurchaseInvoice]
		DROP TABLE [Conic_Erp].[dbo].[SalesInvoice] 

	    DROP TABLE [Conic_Erp].[dbo].[MemberFace] 
		DROP TABLE [Conic_Erp].[dbo].[MemberLog] 
		DROP TABLE [Conic_Erp].[dbo].[MembershipMovementOrder] 
	    DROP TABLE [Conic_Erp].[dbo].[MembershipMovement]
		DROP TABLE [Conic_Erp].[dbo].[Membership] 


	    DROP TABLE [Conic_Erp].[dbo].[Service] 
		DROP TABLE [Conic_Erp].[dbo].[Item] 
		DROP TABLE [Conic_Erp].[dbo].Bank 
		DROP TABLE [Conic_Erp].[dbo].Cash 
		DROP TABLE [Conic_Erp].[dbo].Cheque 
		DROP TABLE [Conic_Erp].[dbo].Discount 
		DROP TABLE [Conic_Erp].[dbo].EntryMovement 
		DROP TABLE [Conic_Erp].[dbo].EntryAccounting 
		DROP TABLE [Conic_Erp].[dbo].Payment 
		DROP TABLE [Conic_Erp].[dbo].ActionLog 

		DROP TABLE [Conic_Erp].[dbo].Vendor 
		DROP TABLE [Conic_Erp].[dbo].[Member] 
		DROP TABLE [Conic_Erp].[dbo].Account 

	/*	DROP TABLE [Conic_Erp].[dbo].[Device]
		DROP TABLE [Conic_Erp].[dbo].[EditorsUser]  
		DROP TABLE [Conic_Erp].[dbo].[FileData] */
		DROP TABLE [Conic_Erp].[dbo].[Oprationsys]



		select * into [Conic_Erp].[dbo].Account from [SuperMarket].Accounting.Account
		select * into [Conic_Erp].[dbo].Bank from [SuperMarket].Accounting.Bank
		select * into [Conic_Erp].[dbo].Cash from [SuperMarket].Accounting.Cash
		select * into [Conic_Erp].[dbo].Cheque from [SuperMarket].Accounting.Cheque
		select * into [Conic_Erp].[dbo].Discount from [SuperMarket].Accounting.Discount
		select * into [Conic_Erp].[dbo].EntryAccounting from [SuperMarket].Accounting.EntryAccounting
		select * into [Conic_Erp].[dbo].EntryMovement from [SuperMarket].Accounting.EntryMovement
	/*	select * into [Conic_Erp].[dbo].Payment from [SuperMarket].Accounting.Payment */
		select * into [Conic_Erp].[dbo].Vendor from [SuperMarket].Accounting.Vendor


		select * into [Conic_Erp].[dbo].ActionLog from [SuperMarket].Config.ActionLog
		/*select * into [Conic_Erp].[dbo].[BackUp] from [SuperMarket].[dbo].[BackUp]
		select * into [Conic_Erp].[dbo].[CompanyInfo] from [SuperMarket].Config.[CompanyInfo]
		select * into [Conic_Erp].[dbo].[Device]  from [SuperMarket].Config.[Device] 
		select * into [Conic_Erp].[dbo].[EditorsUser] from [SuperMarket].[dbo].[EditorsUser]
		select * into [Conic_Erp].[dbo].[FileData] from [SuperMarket].Config.[FileData]*/
		select * into [Conic_Erp].[dbo].[Oprationsys] from [SuperMarket].Config.[Oprationsys]

		/*select * into [Conic_Erp].[dbo].[Member] from [SuperMarket].[dbo].[Member]
	   select * into [Conic_Erp].[dbo].[MemberFace] from [SuperMarket].[dbo].[MemberFace]
		select * into [Conic_Erp].[dbo].[MemberLog] from [SuperMarket].[dbo].[MemberLog]
	    select * into [Conic_Erp].[dbo].[Membership] from [SuperMarket].[dbo].[Membership]
	    select * into [Conic_Erp].[dbo].[MembershipMovement] from [SuperMarket].[dbo].[MembershipMovement]
	    select * into [Conic_Erp].[dbo].[MembershipMovementOrder] from [SuperMarket].[dbo].[MembershipMovementOrder]
	    select * into [Conic_Erp].[dbo].[Service] from [SuperMarket].[dbo].[Service]*/


	    select * into [Conic_Erp].[dbo].[InventoryItem] from [SuperMarket].[Inventory].[InventoryItem]
		select * into [Conic_Erp].[dbo].[InventoryMovement] from [SuperMarket].[Inventory].[InventoryMovement]
	    select * into [Conic_Erp].[dbo].[Item] from [SuperMarket].[Inventory].[Item]
	    select * into [Conic_Erp].[dbo].[ItemMUO] from [SuperMarket].[Inventory].[ItemMUO]
	    select * into [Conic_Erp].[dbo].[MenuItem] from [SuperMarket].[Inventory].[MenuItem]
		select * into [Conic_Erp].[dbo].[OrderInventory] from [SuperMarket].[Inventory].[OrderInventory]
		select * into [Conic_Erp].[dbo].[OriginItem] from [SuperMarket].[Inventory].[OriginItem]
		select * into [Conic_Erp].[dbo].[StockMovement] from [SuperMarket].[Inventory].[StockMovement]
		select * into [Conic_Erp].[dbo].[StocktakingInventory] from [SuperMarket].[Inventory].[StocktakingInventory]
		select * into [Conic_Erp].[dbo].[UnitItem] from [SuperMarket].[Inventory].[UnitItem]


		select * into [Conic_Erp].[dbo].[PurchaseInvoice] from [SuperMarket].[Purchases].[PurchaseInvoice]
		select * into [Conic_Erp].[dbo].[SalesInvoice] from [SuperMarket].[Sales].[SalesInvoice]


		ALTER TABLE [Conic_Erp].[dbo].Account ALTER COLUMN Description nvarchar(MAX)  
		ALTER TABLE [Conic_Erp].[dbo].Account ALTER COLUMN Type nvarchar(MAX)
		ALTER TABLE [Conic_Erp].[dbo].Vendor ALTER COLUMN Description nvarchar(MAX)  
		ALTER TABLE [Conic_Erp].[dbo].Vendor ALTER COLUMN Address nvarchar(MAX)  

use Conic_Erp
CREATE TABLE [dbo].[Member](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](max) NOT NULL,
	[DateofBirth] [date] NULL,
	[Email] [varchar](255) NULL,
	[PhoneNumber1] [varchar](13) NULL,
	[PhoneNumber2] [varchar](13) NULL,
	[Description] [text] NULL,
	[Status] [int] NOT NULL,
	[Type] [text] NOT NULL,
	[AccountID] [bigint] NOT NULL,
	[SSN] [varchar](50) NOT NULL,
	[Tag] [varchar](255) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[Membership](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[NumberDays] [int] NOT NULL,
	[MorningPrice] [float] NOT NULL,
	[FullDayPrice] [float] NOT NULL,
	[Tax] [float] NULL,
	[Rate] [float] NULL,
	[Description] [varchar](255) NULL,
	[Status] [int] NOT NULL,
	[MinFreezeLimitDays] [int] NULL,
	[MaxFreezeLimitDays] [int] NULL
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[MembershipMovement](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[TotalAmmount] [float] NOT NULL,
	[Tax] [float] NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[Type] [text] NOT NULL,
	[VisitsUsed] [int] NOT NULL,
	[Discount] [float] NOT NULL,
	[Description] [text] NULL,
	[Status] [int] NOT NULL,
	[MemberID] [bigint] NOT NULL,
	[MembershipID] [int] NOT NULL,
	[DiscountDescription] [varchar](50) NULL,
	[EditorName] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


CREATE TABLE [dbo].[MembershipMovementOrder](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Type] [varchar](50) NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[Status] [int] NOT NULL,
	[Description] [text] NULL,
	[EditorName] [varchar](max) NULL,
	[MemberShipMovementID] [bigint] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[MemberFace](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[FaceLength] [int] NOT NULL,
	[FaceStr] [nvarchar](max) NOT NULL,
	[MemberID] [bigint] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


CREATE TABLE [dbo].[MemberLog](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Type] [text] NOT NULL,
	[DateTime] [datetime] NOT NULL,
	[Description] [text] NULL,
	[Status] [int] NOT NULL,
	[MemberID] [bigint] NOT NULL,
	[DeviceID] [bigint] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


CREATE TABLE [dbo].[Service](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Qty] [int] NOT NULL,
	[SellingPrice] [float] NOT NULL,
	[ItemID] [bigint] NOT NULL,
	[Type] [varchar](50) NOT NULL,
	[IsPrime] [bit] NOT NULL,
	[Status] [int] NOT NULL,
	[Description] [varchar](50) NULL
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[Payment](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](max) NULL,
	[FakeDate] [date] NOT NULL,
	[PaymentMethod] [varchar](255) NOT NULL,
	[TotalAmmount] [float] NOT NULL,
	[Description] [text] NULL,
	[Status] [int] NOT NULL,
	[VendorID] [bigint] NULL,
	[IsPrime] [bit] NOT NULL,
	[MemberID] [bigint] NULL,
	[Type] [text] NULL,
	[EditorName] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


