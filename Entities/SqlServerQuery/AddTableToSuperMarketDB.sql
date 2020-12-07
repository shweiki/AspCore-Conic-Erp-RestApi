use SuperMarket


CREATE TABLE [dbo].[BackUp](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[DateTime] [datetime] NULL,
	[BackUpPath] [varchar](255) NULL,
	[UserID] [nvarchar](128) NULL,
	[DataBaseName] [varchar](255) NULL,
 CONSTRAINT [PK_BackUp] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[EditorsUser](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](255) NOT NULL,
 CONSTRAINT [PK_EditorsUser] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[MemberFace](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[FaceLength] [int] NOT NULL,
	[FaceStr] [nvarchar](max) NOT NULL,
	[MemberID] [bigint] NOT NULL,
 CONSTRAINT [PK_MemberFace] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[MemberFace]  WITH CHECK ADD  CONSTRAINT [FK_MemberFace_Member] FOREIGN KEY([MemberID])
REFERENCES [Gym].[Member] ([ID])
GO

ALTER TABLE [dbo].[MemberFace] CHECK CONSTRAINT [FK_MemberFace_Member]
GO

CREATE TABLE [dbo].[MemberLog](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](max) NOT NULL,
	[DateTime] [datetime] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Status] [int] NOT NULL,
	[MemberID] [bigint] NOT NULL,
	[DeviceID] [bigint] NOT NULL,
 CONSTRAINT [PK_MemberLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[MemberLog]  WITH CHECK ADD  CONSTRAINT [FK_MemberLog_Device] FOREIGN KEY([DeviceID])
REFERENCES [Config].[Device] ([ID])
GO

ALTER TABLE [dbo].[MemberLog] CHECK CONSTRAINT [FK_MemberLog_Device]
GO

ALTER TABLE [dbo].[MemberLog]  WITH CHECK ADD  CONSTRAINT [FK_MemberLog_Member] FOREIGN KEY([MemberID])
REFERENCES [Gym].[Member] ([ID])
GO

ALTER TABLE [dbo].[MemberLog] CHECK CONSTRAINT [FK_MemberLog_Member]
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
	[MaxFreezeLimitDays] [int] NULL,
 CONSTRAINT [PK_Membership] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[MembershipMovement](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[TotalAmmount] [float] NOT NULL,
	[Tax] [float] NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[Type] [nvarchar](max) NOT NULL,
	[VisitsUsed] [int] NOT NULL,
	[Discount] [float] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[Status] [int] NOT NULL,
	[MemberID] [bigint] NOT NULL,
	[MembershipID] [int] NOT NULL,
	[DiscountDescription] [varchar](50) NULL,
	[EditorName] [varchar](max) NULL,
 CONSTRAINT [PK_MembershipMovement] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[MembershipMovement]  WITH CHECK ADD  CONSTRAINT [FK_MembershipMovement_Member] FOREIGN KEY([MemberID])
REFERENCES [Gym].[Member] ([ID])
GO

ALTER TABLE [dbo].[MembershipMovement] CHECK CONSTRAINT [FK_MembershipMovement_Member]
GO

ALTER TABLE [dbo].[MembershipMovement]  WITH CHECK ADD  CONSTRAINT [FK_MembershipMovement_Membership] FOREIGN KEY([MembershipID])
REFERENCES [dbo].[Membership] ([ID])
GO

ALTER TABLE [dbo].[MembershipMovement] CHECK CONSTRAINT [FK_MembershipMovement_Membership]
GO

CREATE TABLE [dbo].[MembershipMovementOrder](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Type] [varchar](50) NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NOT NULL,
	[Status] [int] NOT NULL,
	[Description] [nvarchar](max) NULL,
	[EditorName] [varchar](max) NULL,
	[MemberShipMovementID] [bigint] NOT NULL,
 CONSTRAINT [PK_MembershipMovementOrder] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[MembershipMovementOrder]  WITH CHECK ADD  CONSTRAINT [FK_MembershipMovementOrder_MembershipMovement] FOREIGN KEY([MemberShipMovementID])
REFERENCES [dbo].[MembershipMovement] ([ID])
GO

ALTER TABLE [dbo].[MembershipMovementOrder] CHECK CONSTRAINT [FK_MembershipMovementOrder_MembershipMovement]
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
	[Description] [varchar](50) NULL,
 CONSTRAINT [PK_Service] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Service]  WITH CHECK ADD  CONSTRAINT [FK_Service_Item] FOREIGN KEY([ItemID])
REFERENCES [Inventory].[Item] ([ID])
GO

ALTER TABLE [dbo].[Service] CHECK CONSTRAINT [FK_Service_Item]
GO


