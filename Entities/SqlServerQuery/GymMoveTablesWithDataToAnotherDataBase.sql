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
		DROP TABLE [Conic_Erp].[dbo].Vendor 
		DROP TABLE [Conic_Erp].[dbo].[Member] 
		DROP TABLE [Conic_Erp].[dbo].Account 


		DROP TABLE [Conic_Erp].[dbo].ActionLog 
		DROP TABLE [Conic_Erp].[dbo].[BackUp] 
		DROP TABLE [Conic_Erp].[dbo].[CompanyInfo] 
		DROP TABLE [Conic_Erp].[dbo].[Device] 
		DROP TABLE [Conic_Erp].[dbo].[EditorsUser] 
		DROP TABLE [Conic_Erp].[dbo].[FileData] 
		DROP TABLE [Conic_Erp].[dbo].[Oprationsys]



		select * into [Conic_Erp].[dbo].Account from [HighFit].Accounting.Account
		select * into [Conic_Erp].[dbo].Bank from [HighFit].Accounting.Bank
		select * into [Conic_Erp].[dbo].Cash from [HighFit].Accounting.Cash
		select * into [Conic_Erp].[dbo].Cheque from [HighFit].Accounting.Cheque
		select * into [Conic_Erp].[dbo].Discount from [HighFit].Accounting.Discount
		select * into [Conic_Erp].[dbo].EntryAccounting from [HighFit].Accounting.EntryAccounting
		select * into [Conic_Erp].[dbo].EntryMovement from [HighFit].Accounting.EntryMovement
		select * into [Conic_Erp].[dbo].Payment from [HighFit].Accounting.Payment
		select * into [Conic_Erp].[dbo].Vendor from [HighFit].Accounting.Vendor


		select * into [Conic_Erp].[dbo].ActionLog from [HighFit].Config.ActionLog
		select * into [Conic_Erp].[dbo].[BackUp] from [HighFit].[Config].[BackUp]
		select * into [Conic_Erp].[dbo].[CompanyInfo] from [HighFit].Config.[CompanyInfo]
		select * into [Conic_Erp].[dbo].[Device]  from [HighFit].Config.[Device]
		select * into [Conic_Erp].[dbo].[EditorsUser] from [HighFit].Config.[EditorsUser]
		select * into [Conic_Erp].[dbo].[FileData] from [HighFit].Config.[FileData]
		select * into [Conic_Erp].[dbo].[Oprationsys] from [HighFit].Config.[Oprationsys]

		select * into [Conic_Erp].[dbo].[Member] from [HighFit].[Gym].[Member]
	    select * into [Conic_Erp].[dbo].[MemberFace] from [HighFit].[Gym].[MemberFace]
		select * into [Conic_Erp].[dbo].[MemberLog] from [HighFit].[Gym].[MemberLog]
	    select * into [Conic_Erp].[dbo].[Membership] from [HighFit].[Gym].[Membership]
	    select * into [Conic_Erp].[dbo].[MembershipMovement] from [HighFit].[Gym].[MembershipMovement]
	    select * into [Conic_Erp].[dbo].[MembershipMovementOrder] from [HighFit].[Gym].[MembershipMovementOrder]
	    select * into [Conic_Erp].[dbo].[Service] from [HighFit].[Gym].[Service]


	    select * into [Conic_Erp].[dbo].[InventoryItem] from [HighFit].[Inventory].[InventoryItem]
		select * into [Conic_Erp].[dbo].[InventoryMovement] from [HighFit].[Inventory].[InventoryMovement]
	    select * into [Conic_Erp].[dbo].[Item] from [HighFit].[Inventory].[Item]
	    select * into [Conic_Erp].[dbo].[ItemMUO] from [HighFit].[Inventory].[ItemMUO]
	    select * into [Conic_Erp].[dbo].[MenuItem] from [HighFit].[Inventory].[MenuItem]
		select * into [Conic_Erp].[dbo].[OrderInventory] from [HighFit].[Inventory].[OrderInventory]
		select * into [Conic_Erp].[dbo].[OriginItem] from [HighFit].[Inventory].[OriginItem]
		select * into [Conic_Erp].[dbo].[StockMovement] from [HighFit].[Inventory].[StockMovement]
		select * into [Conic_Erp].[dbo].[StocktakingInventory] from [HighFit].[Inventory].[StocktakingInventory]
		select * into [Conic_Erp].[dbo].[UnitItem] from [HighFit].[Inventory].[UnitItem]


		select * into [Conic_Erp].[dbo].[PurchaseInvoice] from [HighFit].[Purchases].[PurchaseInvoice]
		select * into [Conic_Erp].[dbo].[SalesInvoice] from [HighFit].[Sales].[SalesInvoice]


		ALTER TABLE [Conic_Erp].[dbo].Account ALTER COLUMN Description nvarchar(MAX)  
		ALTER TABLE [Conic_Erp].[dbo].Account ALTER COLUMN Type nvarchar(MAX)
		ALTER TABLE [Conic_Erp].[dbo].Vendor ALTER COLUMN Description nvarchar(MAX)  
		ALTER TABLE [Conic_Erp].[dbo].Vendor ALTER COLUMN Address nvarchar(MAX)  
