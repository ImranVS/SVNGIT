USE [master]
GO
/****** Object:  Database [License]    Script Date: 4/28/2015 10:57:39 PM ******/
CREATE DATABASE [License] ON  PRIMARY 
( NAME = N'License', FILENAME = N'$(dbpath)\License.mdf' , SIZE = 2304KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'License_log', FILENAME = N'$(dbpath)\License_log.LDF' , SIZE = 504KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [License] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [License].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [License] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [License] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [License] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [License] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [License] SET ARITHABORT OFF 
GO
ALTER DATABASE [License] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [License] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [License] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [License] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [License] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [License] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [License] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [License] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [License] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [License] SET  ENABLE_BROKER 
GO
ALTER DATABASE [License] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [License] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [License] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [License] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [License] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [License] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [License] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [License] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [License] SET  MULTI_USER 
GO
ALTER DATABASE [License] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [License] SET DB_CHAINING OFF 
GO

/****** Object:  User [vs]    Script Date: 09/23/2013 16:59:28 ******/

CREATE LOGIN ls WITH PASSWORD = 'admin123!';
GO

USE [License]
GO
/****** Object:  User [vs]    Script Date: 09/23/2013 16:59:28 ******/
CREATE USER [ls] For LOGIN ls
GO
EXEC sp_addrolemember N'db_owner', N'ls'
go

USE [License]
GO
/****** Object:  Table [dbo].[License]    Script Date: 4/28/2015 10:57:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[License](
	[LicenseKey] [varchar](400) NULL,
	[Units] [int] NULL,
	[InstallType] [varchar](50) NULL,
	[LicenseType] [varchar](100) NULL,
	[ExpirationDate] [datetime] NULL,
	[EncUnits] [varbinary](400) NULL,
	[CreateBy] [int] NULL,
	[CreatedOn] [datetime] NULL,
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CompanyID] [int] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LicenseCompanys]    Script Date: 4/28/2015 10:57:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LicenseCompanys](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CompanyName] [nvarchar](50) NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Roles]    Script Date: 4/28/2015 10:57:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](max) NULL,
	[AccessTo] [nvarchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 4/28/2015 10:57:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Users](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LoginName] [varchar](100) NOT NULL,
	[Password] [varchar](50) NULL,
	[FullName] [varchar](100) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[Status] [varchar](50) NOT NULL,
	[UserType] [varchar](50) NOT NULL,
	[SecurityQuestion1] [varchar](255) NULL,
	[SecurityQuestion1Answer] [varchar](100) NULL,
	[SecurityQuestion2] [varchar](255) NULL,
	[SecurityQuestion2Answer] [varchar](100) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
--VSPLUS 1756  Durga 
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ServerTypes](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ServerType] [varchar](50) NOT NULL,
	[ServerTypeTable] [nvarchar](50) NULL,
	[FeatureId] [int] NULL,
	[UnitCost] [int] NULL,
 CONSTRAINT [PK_ServerType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
-- VSPLUS-2238 Sowjanya

USE [License]
GO
/****** Object:  Table [dbo].[UserPreferences]    Script Date: 10/05/2015 22:54:43 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UserPreferences](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[PreferenceName] [nvarchar](250) NULL,
	[PreferenceValue] [nvarchar](250) NULL,
	[UserID] [int] NOT NULL
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[UserPreferences]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
ON DELETE CASCADE
GO


USE [master]
GO
ALTER DATABASE [License] SET  READ_WRITE 
GO
