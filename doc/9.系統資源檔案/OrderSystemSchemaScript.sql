USE [master]
GO
/****** Object:  Database [OrderSystem]    Script Date: 2021/12/26 上午 03:09:53 ******/
CREATE DATABASE [OrderSystem]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'OrderSystem', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\OrderSystem.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'OrderSystem_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\OrderSystem_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [OrderSystem] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [OrderSystem].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [OrderSystem] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [OrderSystem] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [OrderSystem] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [OrderSystem] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [OrderSystem] SET ARITHABORT OFF 
GO
ALTER DATABASE [OrderSystem] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [OrderSystem] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [OrderSystem] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [OrderSystem] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [OrderSystem] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [OrderSystem] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [OrderSystem] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [OrderSystem] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [OrderSystem] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [OrderSystem] SET  DISABLE_BROKER 
GO
ALTER DATABASE [OrderSystem] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [OrderSystem] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [OrderSystem] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [OrderSystem] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [OrderSystem] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [OrderSystem] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [OrderSystem] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [OrderSystem] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [OrderSystem] SET  MULTI_USER 
GO
ALTER DATABASE [OrderSystem] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [OrderSystem] SET DB_CHAINING OFF 
GO
ALTER DATABASE [OrderSystem] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [OrderSystem] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [OrderSystem] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [OrderSystem] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [OrderSystem] SET QUERY_STORE = OFF
GO
USE [OrderSystem]
GO
/****** Object:  Table [dbo].[Permissions]    Script Date: 2021/12/26 上午 03:09:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Permissions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [int] NULL,
	[Code] [nvarchar](200) NULL,
 CONSTRAINT [PK_Permissions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductCategory]    Script Date: 2021/12/26 上午 03:09:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductCategory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Description] [nvarchar](500) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NOT NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_ProductCategory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductInventory]    Script Date: 2021/12/26 上午 03:09:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductInventory](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NULL,
	[Unit] [decimal](18, 4) NULL,
	[Description] [varchar](500) NULL,
	[CreatedAt] [datetime2](7) NULL,
 CONSTRAINT [PK_productInventory] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductProductCategoryRelationships]    Script Date: 2021/12/26 上午 03:09:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductProductCategoryRelationships](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProductId] [int] NULL,
	[ProductCategoryId] [int] NULL,
 CONSTRAINT [PK_ProductProductCategoryRelationships] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 2021/12/26 上午 03:09:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Number] [nvarchar](100) NULL,
	[Name] [nvarchar](100) NULL,
	[Price] [decimal](18, 4) NULL,
	[Description] [varchar](500) NULL,
	[CurrentUnit] [decimal](18, 4) NULL,
	[CreatedAt] [datetime2](7) NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReturnShipmentOrderDetails]    Script Date: 2021/12/26 上午 03:09:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReturnShipmentOrderDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ReturnShipmentOrderId] [int] NULL,
	[ShipmentOrderDetailId] [int] NULL,
	[Unit] [decimal](18, 4) NULL,
	[IsDeleted] [bit] NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdateAt] [datetime2](7) NOT NULL,
	[Remarks] [varchar](500) NULL,
 CONSTRAINT [PK_ReturnShipmentOrderDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReturnShipmentOrders]    Script Date: 2021/12/26 上午 03:09:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReturnShipmentOrders](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Number] [varchar](100) NULL,
	[ShipmentOrderId] [int] NULL,
	[Price] [decimal](18, 4) NULL,
	[ReturnDate] [datetime2](7) NULL,
	[Remarks] [nvarchar](500) NULL,
	[IsDeleted] [bit] NULL,
	[UpdatedAt] [datetime2](7) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[Total] [decimal](18, 4) NULL,
 CONSTRAINT [PK_ReturnShipmentOrders] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 2021/12/26 上午 03:09:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[CreatedAt] [datetime2](7) NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ShipmentOrderDetails]    Script Date: 2021/12/26 上午 03:09:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ShipmentOrderDetails](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NULL,
	[ProductId] [int] NULL,
	[ProductNumber] [nvarchar](100) NULL,
	[ProductName] [nvarchar](100) NULL,
	[ProductPrice] [decimal](18, 4) NULL,
	[ProductUnit] [decimal](18, 4) NULL,
	[Remarks] [nvarchar](500) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[UpdatedAt] [datetime2](7) NOT NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_OrderDetails] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ShipmentOrders]    Script Date: 2021/12/26 上午 03:09:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ShipmentOrders](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Number] [varchar](100) NOT NULL,
	[Type] [int] NULL,
	[Total] [decimal](18, 4) NULL,
	[SignName] [varchar](100) NULL,
	[Status] [int] NULL,
	[FinishDate] [datetime2](7) NULL,
	[DeliveryDate] [datetime2](7) NULL,
	[Address] [varchar](500) NULL,
	[Remarks] [varchar](500) NULL,
	[UpdateAt] [datetime2](7) NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 2021/12/26 上午 03:09:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NULL,
	[Salt] [varchar](50) NULL,
	[Email] [nvarchar](100) NULL,
	[Account] [nvarchar](100) NULL,
	[Password] [nvarchar](500) NULL,
	[RoleId] [int] NULL,
	[CreateAt] [datetime2](7) NULL,
	[UpdatedAt] [datetime2](7) NULL,
	[IsDeleted] [bit] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ProductCategory] ADD  CONSTRAINT [DF_ProductCategory_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[ProductCategory] ADD  CONSTRAINT [DF_ProductCategory_UpdatedAt]  DEFAULT (getdate()) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[ProductInventory] ADD  CONSTRAINT [DF_ProductInventory_created_at]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Products] ADD  CONSTRAINT [DF_Products_CreatedAt]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Products] ADD  CONSTRAINT [DF_Products_UpdatedAt]  DEFAULT (getdate()) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[Roles] ADD  CONSTRAINT [DF_Roles_UpdatedAt]  DEFAULT (getdate()) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[ShipmentOrderDetails] ADD  CONSTRAINT [DF_OrderDetails_CreatedAt_1]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[ShipmentOrderDetails] ADD  CONSTRAINT [DF_OrderDetails_UpdateAt]  DEFAULT (getdate()) FOR [UpdatedAt]
GO
ALTER TABLE [dbo].[ShipmentOrders] ADD  CONSTRAINT [DF_Orders_UpdateTime]  DEFAULT (getdate()) FOR [UpdateAt]
GO
ALTER TABLE [dbo].[ShipmentOrders] ADD  CONSTRAINT [DF_Orders_CreateTime]  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_CreateAt]  DEFAULT (getdate()) FOR [CreateAt]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_UpdatedAt]  DEFAULT (getdate()) FOR [UpdatedAt]
GO
USE [master]
GO
ALTER DATABASE [OrderSystem] SET  READ_WRITE 
GO
