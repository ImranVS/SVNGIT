USE [CustomerTracking]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 05/01/2014 11:59:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Customer](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NULL,
	[Status_Type] [varchar](50) NULL,
	[Address] [varchar](50) NULL,
	[ServerCount] [nchar](10) NULL,
	[CompReplacement] [varchar](50) NULL,
	[OverallStatus] [varchar](50) NULL,
	[NextFollowUpDate] [date] NULL,
	[LicExpDate] [date] NULL,
	[BudInfo] [varchar](50) NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VersionInfo]    Script Date: 05/01/2014 11:59:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VersionInfo](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CustID] [int] NOT NULL,
	[VersionNumber] [nchar](10) NULL,
	[InstallDate] [date] NULL,
	[Details] [varchar](200) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Tickets]    Script Date: 05/01/2014 11:59:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Tickets](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CustID] [int] NOT NULL,
	[Date] [date] NULL,
	[TicketNumber] [nchar](10) NULL,
	[Details] [varchar](200) NULL,
	[Status] [varchar](50) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Notes]    Script Date: 05/01/2014 11:59:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Notes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CustID] [int] NOT NULL,
	[Date] [date] NULL,
	[Details] [varchar](200) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Contacts]    Script Date: 05/01/2014 11:59:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Contacts](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CustID] [int] NOT NULL,
	[Name] [varchar](50) NULL,
	[PhoneNumber] [nchar](15) NULL,
	[Title] [varchar](50) NULL,
	[Details] [varchar](200) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  ForeignKey [FK__Contacts__CustID__30F848ED]    Script Date: 05/01/2014 11:59:13 ******/
ALTER TABLE [dbo].[Contacts]  WITH CHECK ADD FOREIGN KEY([CustID])
REFERENCES [dbo].[Customer] ([ID])
GO
/****** Object:  ForeignKey [FK__Notes__CustID__32E0915F]    Script Date: 05/01/2014 11:59:13 ******/
ALTER TABLE [dbo].[Notes]  WITH CHECK ADD FOREIGN KEY([CustID])
REFERENCES [dbo].[Customer] ([ID])
GO
/****** Object:  ForeignKey [FK__Tickets__CustID__36B12243]    Script Date: 05/01/2014 11:59:13 ******/
ALTER TABLE [dbo].[Tickets]  WITH CHECK ADD FOREIGN KEY([CustID])
REFERENCES [dbo].[Customer] ([ID])
GO
/****** Object:  ForeignKey [FK__VersionIn__CustI__34C8D9D1]    Script Date: 05/01/2014 11:59:13 ******/
ALTER TABLE [dbo].[VersionInfo]  WITH CHECK ADD FOREIGN KEY([CustID])
REFERENCES [dbo].[Customer] ([ID])
GO
