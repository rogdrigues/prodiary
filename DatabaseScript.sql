USE [master]
GO
/****** Object:  Database [DiaryNote]    Script Date: 7/23/2023 12:14:09 PM ******/
CREATE DATABASE [DiaryNote]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DiaryNote', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\DiaryNote.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'DiaryNote_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\DiaryNote_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [DiaryNote] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DiaryNote].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [DiaryNote] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DiaryNote] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DiaryNote] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DiaryNote] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DiaryNote] SET ARITHABORT OFF 
GO
ALTER DATABASE [DiaryNote] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [DiaryNote] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DiaryNote] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DiaryNote] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DiaryNote] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [DiaryNote] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DiaryNote] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DiaryNote] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DiaryNote] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DiaryNote] SET  ENABLE_BROKER 
GO
ALTER DATABASE [DiaryNote] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DiaryNote] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DiaryNote] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [DiaryNote] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [DiaryNote] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DiaryNote] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [DiaryNote] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [DiaryNote] SET RECOVERY FULL 
GO
ALTER DATABASE [DiaryNote] SET  MULTI_USER 
GO
ALTER DATABASE [DiaryNote] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [DiaryNote] SET DB_CHAINING OFF 
GO
ALTER DATABASE [DiaryNote] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [DiaryNote] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [DiaryNote] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [DiaryNote] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'DiaryNote', N'ON'
GO
ALTER DATABASE [DiaryNote] SET QUERY_STORE = ON
GO
ALTER DATABASE [DiaryNote] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [DiaryNote]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 7/23/2023 12:14:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](100) NOT NULL,
	[Password] [nvarchar](100) NOT NULL,
	[Email] [nvarchar](100) NULL,
	[FullName] [nvarchar](100) NULL,
	[Avatar] [varbinary](max) NULL,
	[CreatedDate] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Memo]    Script Date: 7/23/2023 12:14:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Memo](
	[MemoID] [int] IDENTITY(1,1) NOT NULL,
	[MemoTitle] [nvarchar](255) NOT NULL,
	[MemoContent] [nvarchar](max) NOT NULL,
	[MemoAttachments] [varbinary](max) NULL,
	[MemoCreated] [datetime] NULL,
	[MemoUpdated] [datetime] NULL,
	[MemoAuthor] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[MemoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MemoTag]    Script Date: 7/23/2023 12:14:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MemoTag](
	[MemoID] [int] NULL,
	[TagID] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tag]    Script Date: 7/23/2023 12:14:09 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tag](
	[TagID] [int] IDENTITY(1,1) NOT NULL,
	[TagName] [nvarchar](255) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[TagID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Account] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Memo] ADD  DEFAULT (getdate()) FOR [MemoCreated]
GO
ALTER TABLE [dbo].[Memo] ADD  DEFAULT (getdate()) FOR [MemoUpdated]
GO
ALTER TABLE [dbo].[MemoTag]  WITH CHECK ADD FOREIGN KEY([MemoID])
REFERENCES [dbo].[Memo] ([MemoID])
GO
ALTER TABLE [dbo].[MemoTag]  WITH CHECK ADD FOREIGN KEY([TagID])
REFERENCES [dbo].[Tag] ([TagID])
GO
USE [master]
GO
ALTER DATABASE [DiaryNote] SET  READ_WRITE 
GO
