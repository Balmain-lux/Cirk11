USE [master]
GO
/****** Object:  Database [CircusManagement]    Script Date: 13.06.2025 18:08:22 ******/
CREATE DATABASE [CircusManagement]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CircusManagement', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\CircusManagement.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'CircusManagement_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\CircusManagement_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [CircusManagement] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CircusManagement].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CircusManagement] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CircusManagement] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CircusManagement] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CircusManagement] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CircusManagement] SET ARITHABORT OFF 
GO
ALTER DATABASE [CircusManagement] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [CircusManagement] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CircusManagement] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CircusManagement] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CircusManagement] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CircusManagement] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CircusManagement] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CircusManagement] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CircusManagement] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CircusManagement] SET  ENABLE_BROKER 
GO
ALTER DATABASE [CircusManagement] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CircusManagement] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CircusManagement] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CircusManagement] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CircusManagement] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CircusManagement] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CircusManagement] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CircusManagement] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [CircusManagement] SET  MULTI_USER 
GO
ALTER DATABASE [CircusManagement] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CircusManagement] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CircusManagement] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CircusManagement] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [CircusManagement] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [CircusManagement] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [CircusManagement] SET QUERY_STORE = ON
GO
ALTER DATABASE [CircusManagement] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [CircusManagement]
GO
/****** Object:  Table [dbo].[Events]    Script Date: 13.06.2025 18:08:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Events](
	[EventID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[EventDate] [datetime] NOT NULL,
	[EventType] [nvarchar](20) NOT NULL,
	[Profit] [decimal](15, 2) NULL,
	[Expenses] [decimal](15, 2) NULL,
	[Prepayment] [decimal](15, 2) NULL,
	[OrganizingCompany] [nvarchar](100) NULL,
	[IsCompleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[EventID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[MonthlyReports]    Script Date: 13.06.2025 18:08:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Create a view for monthly reports
CREATE VIEW [dbo].[MonthlyReports] AS
SELECT 
    MONTH(EventDate) AS Month,
    YEAR(EventDate) AS Year,
    COUNT(*) AS TotalEvents,
    SUM(CASE WHEN EventType = 'Private' THEN 1 ELSE 0 END) AS PrivateEvents,
    SUM(CASE WHEN EventType = 'Guest' THEN 1 ELSE 0 END) AS GuestEvents,
    SUM(Profit) AS TotalProfit,
    SUM(Expenses) AS TotalExpenses,
    SUM(Profit - Expenses) AS NetIncome
FROM Events
WHERE IsCompleted = 1
GROUP BY MONTH(EventDate), YEAR(EventDate);
GO
/****** Object:  Table [dbo].[Animals]    Script Date: 13.06.2025 18:08:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Animals](
	[AnimalID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Species] [nvarchar](50) NOT NULL,
	[Age] [int] NOT NULL,
	[Gender] [nvarchar](10) NOT NULL,
	[Weight] [decimal](10, 2) NOT NULL,
	[FoodPreferences] [nvarchar](255) NULL,
	[CareRecommendations] [nvarchar](255) NULL,
	[TrainerID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[AnimalID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Artists]    Script Date: 13.06.2025 18:08:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Artists](
	[ArtistID] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[SuccessfulPerformances] [int] NOT NULL,
	[Type] [nvarchar](20) NOT NULL,
	[DressingRoom] [nvarchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[ArtistID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Employees]    Script Date: 13.06.2025 18:08:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employees](
	[EmployeeID] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Position] [nvarchar](50) NOT NULL,
	[Department] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[EmployeeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Holograms]    Script Date: 13.06.2025 18:08:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Holograms](
	[HologramID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[DevelopmentStage] [nvarchar](50) NOT NULL,
	[ResponsibleID] [int] NOT NULL,
	[CabinetNumber] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[HologramID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Trainers]    Script Date: 13.06.2025 18:08:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Trainers](
	[TrainerID] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Specialization] [nvarchar](100) NULL,
	[ExperienceYears] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[TrainerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Animals] ON 

INSERT [dbo].[Animals] ([AnimalID], [Name], [Species], [Age], [Gender], [Weight], [FoodPreferences], [CareRecommendations], [TrainerID]) VALUES (1, N'Рекс', N'Лев', 5, N'Мужской', CAST(190.50 AS Decimal(10, 2)), N'Мясо, курица', N'Ежедневные тренировки, регулярный осмотр ветеринара', 1)
INSERT [dbo].[Animals] ([AnimalID], [Name], [Species], [Age], [Gender], [Weight], [FoodPreferences], [CareRecommendations], [TrainerID]) VALUES (2, N'Лола', N'Обезьяна', 3, N'Женский', CAST(15.20 AS Decimal(10, 2)), N'Фрукты, орехи', N'Социальное животное, требует общения', 2)
INSERT [dbo].[Animals] ([AnimalID], [Name], [Species], [Age], [Gender], [Weight], [FoodPreferences], [CareRecommendations], [TrainerID]) VALUES (3, N'Кеша', N'Попугай', 10, N'Мужской', CAST(0.80 AS Decimal(10, 2)), N'Зерно, фрукты', N'Регулярные полеты, общение', 3)
INSERT [dbo].[Animals] ([AnimalID], [Name], [Species], [Age], [Gender], [Weight], [FoodPreferences], [CareRecommendations], [TrainerID]) VALUES (4, N'Немо', N'Дельфин', 7, N'Мужской', CAST(180.00 AS Decimal(10, 2)), N'Рыба', N'Бассейн должен быть всегда чистым', 4)
INSERT [dbo].[Animals] ([AnimalID], [Name], [Species], [Age], [Gender], [Weight], [FoodPreferences], [CareRecommendations], [TrainerID]) VALUES (5, N'Гена', N'Крокодил', 12, N'Мужской', CAST(250.00 AS Decimal(10, 2)), N'Мясо, рыба', N'Осторожно, агрессивен во время кормления', 5)
SET IDENTITY_INSERT [dbo].[Animals] OFF
GO
SET IDENTITY_INSERT [dbo].[Artists] ON 

INSERT [dbo].[Artists] ([ArtistID], [FullName], [SuccessfulPerformances], [Type], [DressingRoom]) VALUES (1, N'Смирнов Алексей Викторович', 5, N'Beginner', NULL)
INSERT [dbo].[Artists] ([ArtistID], [FullName], [SuccessfulPerformances], [Type], [DressingRoom]) VALUES (2, N'Ковалева Ольга Игоревна', 15, N'Promoting', NULL)
INSERT [dbo].[Artists] ([ArtistID], [FullName], [SuccessfulPerformances], [Type], [DressingRoom]) VALUES (3, N'Жуков Павел Сергеевич', 35, N'VIP', N'VIP-1')
INSERT [dbo].[Artists] ([ArtistID], [FullName], [SuccessfulPerformances], [Type], [DressingRoom]) VALUES (4, N'Николаева Анна Дмитриевна', 25, N'Promoting', NULL)
INSERT [dbo].[Artists] ([ArtistID], [FullName], [SuccessfulPerformances], [Type], [DressingRoom]) VALUES (5, N'Федоров Михаил Александрович', 42, N'VIP', N'VIP-2')
SET IDENTITY_INSERT [dbo].[Artists] OFF
GO
SET IDENTITY_INSERT [dbo].[Employees] ON 

INSERT [dbo].[Employees] ([EmployeeID], [FullName], [Position], [Department]) VALUES (1, N'Волков Артем Геннадьевич', N'Инженер голограмм', N'Голограммы')
INSERT [dbo].[Employees] ([EmployeeID], [FullName], [Position], [Department]) VALUES (2, N'Белова Ирина Васильевна', N'Техник', N'Голограммы')
INSERT [dbo].[Employees] ([EmployeeID], [FullName], [Position], [Department]) VALUES (3, N'Соколов Денис Олегович', N'Разработчик', N'Голограммы')
INSERT [dbo].[Employees] ([EmployeeID], [FullName], [Position], [Department]) VALUES (4, N'Павлова Виктория Сергеевна', N'Дизайнер', N'Голограммы')
INSERT [dbo].[Employees] ([EmployeeID], [FullName], [Position], [Department]) VALUES (5, N'Козлов Андрей Игоревич', N'Технический директор', N'Голограммы')
SET IDENTITY_INSERT [dbo].[Employees] OFF
GO
SET IDENTITY_INSERT [dbo].[Events] ON 

INSERT [dbo].[Events] ([EventID], [Name], [EventDate], [EventType], [Profit], [Expenses], [Prepayment], [OrganizingCompany], [IsCompleted]) VALUES (9, N'Звезды цирка', CAST(N'2023-05-15T19:00:00.000' AS DateTime), N'Private', CAST(500000.00 AS Decimal(15, 2)), CAST(200000.00 AS Decimal(15, 2)), CAST(0.00 AS Decimal(15, 2)), NULL, 1)
INSERT [dbo].[Events] ([EventID], [Name], [EventDate], [EventType], [Profit], [Expenses], [Prepayment], [OrganizingCompany], [IsCompleted]) VALUES (10, N'Сафари-шоу', CAST(N'2023-05-22T18:30:00.000' AS DateTime), N'Guest', CAST(750000.00 AS Decimal(15, 2)), CAST(300000.00 AS Decimal(15, 2)), CAST(0.00 AS Decimal(15, 2)), NULL, 1)
INSERT [dbo].[Events] ([EventID], [Name], [EventDate], [EventType], [Profit], [Expenses], [Prepayment], [OrganizingCompany], [IsCompleted]) VALUES (11, N'Голографическое чудо', CAST(N'2023-06-01T20:00:00.000' AS DateTime), N'Private', CAST(600000.00 AS Decimal(15, 2)), CAST(250000.00 AS Decimal(15, 2)), CAST(0.00 AS Decimal(15, 2)), NULL, 1)
INSERT [dbo].[Events] ([EventID], [Name], [EventDate], [EventType], [Profit], [Expenses], [Prepayment], [OrganizingCompany], [IsCompleted]) VALUES (12, N'Цирк будущего', CAST(N'2023-06-10T19:30:00.000' AS DateTime), N'Guest', CAST(900000.00 AS Decimal(15, 2)), CAST(400000.00 AS Decimal(15, 2)), CAST(0.00 AS Decimal(15, 2)), NULL, 1)
INSERT [dbo].[Events] ([EventID], [Name], [EventDate], [EventType], [Profit], [Expenses], [Prepayment], [OrganizingCompany], [IsCompleted]) VALUES (13, N'Ночные хищники', CAST(N'2023-06-18T21:00:00.000' AS DateTime), N'Private', CAST(550000.00 AS Decimal(15, 2)), CAST(220000.00 AS Decimal(15, 2)), CAST(0.00 AS Decimal(15, 2)), NULL, 1)
INSERT [dbo].[Events] ([EventID], [Name], [EventDate], [EventType], [Profit], [Expenses], [Prepayment], [OrganizingCompany], [IsCompleted]) VALUES (14, N'Морские глубины', CAST(N'2023-07-05T19:00:00.000' AS DateTime), N'Guest', NULL, NULL, CAST(200000.00 AS Decimal(15, 2)), N'ООО "Морские шоу"', 0)
INSERT [dbo].[Events] ([EventID], [Name], [EventDate], [EventType], [Profit], [Expenses], [Prepayment], [OrganizingCompany], [IsCompleted]) VALUES (15, N'Воздушные акробаты', CAST(N'2023-07-12T18:30:00.000' AS DateTime), N'Private', NULL, NULL, CAST(0.00 AS Decimal(15, 2)), NULL, 0)
INSERT [dbo].[Events] ([EventID], [Name], [EventDate], [EventType], [Profit], [Expenses], [Prepayment], [OrganizingCompany], [IsCompleted]) VALUES (16, N'Голографический зоопарк', CAST(N'2023-07-20T20:00:00.000' AS DateTime), N'Guest', NULL, NULL, CAST(150000.00 AS Decimal(15, 2)), N'ЗАО "ГолоМир"', 0)
INSERT [dbo].[Events] ([EventID], [Name], [EventDate], [EventType], [Profit], [Expenses], [Prepayment], [OrganizingCompany], [IsCompleted]) VALUES (17, N'Юбилейный концерт', CAST(N'2023-07-28T19:30:00.000' AS DateTime), N'Private', NULL, NULL, CAST(0.00 AS Decimal(15, 2)), NULL, 0)
INSERT [dbo].[Events] ([EventID], [Name], [EventDate], [EventType], [Profit], [Expenses], [Prepayment], [OrganizingCompany], [IsCompleted]) VALUES (18, N'Цирк на льду', CAST(N'2023-08-05T18:00:00.000' AS DateTime), N'Guest', NULL, NULL, CAST(300000.00 AS Decimal(15, 2)), N'ООО "Ледовые представления"', 0)
SET IDENTITY_INSERT [dbo].[Events] OFF
GO
SET IDENTITY_INSERT [dbo].[Holograms] ON 

INSERT [dbo].[Holograms] ([HologramID], [Name], [DevelopmentStage], [ResponsibleID], [CabinetNumber]) VALUES (1, N'Космическое шоу', N'Завершено', 1, 101)
INSERT [dbo].[Holograms] ([HologramID], [Name], [DevelopmentStage], [ResponsibleID], [CabinetNumber]) VALUES (2, N'Подводный мир', N'Тестирование', 2, 102)
INSERT [dbo].[Holograms] ([HologramID], [Name], [DevelopmentStage], [ResponsibleID], [CabinetNumber]) VALUES (3, N'Динозавры', N'Разработка', 3, 103)
INSERT [dbo].[Holograms] ([HologramID], [Name], [DevelopmentStage], [ResponsibleID], [CabinetNumber]) VALUES (4, N'Сказочные существа', N'Планирование', 4, 104)
INSERT [dbo].[Holograms] ([HologramID], [Name], [DevelopmentStage], [ResponsibleID], [CabinetNumber]) VALUES (5, N'Галактический цирк', N'Завершено', 5, 105)
SET IDENTITY_INSERT [dbo].[Holograms] OFF
GO
SET IDENTITY_INSERT [dbo].[Trainers] ON 

INSERT [dbo].[Trainers] ([TrainerID], [FullName], [Specialization], [ExperienceYears]) VALUES (1, N'Иванов Иван Иванович', N'Хищники', 10)
INSERT [dbo].[Trainers] ([TrainerID], [FullName], [Specialization], [ExperienceYears]) VALUES (2, N'Петрова Анна Сергеевна', N'Приматы', 7)
INSERT [dbo].[Trainers] ([TrainerID], [FullName], [Specialization], [ExperienceYears]) VALUES (3, N'Сидоров Алексей Владимирович', N'Птицы', 5)
INSERT [dbo].[Trainers] ([TrainerID], [FullName], [Specialization], [ExperienceYears]) VALUES (4, N'Кузнецова Елена Дмитриевна', N'Морские животные', 12)
INSERT [dbo].[Trainers] ([TrainerID], [FullName], [Specialization], [ExperienceYears]) VALUES (5, N'Морозов Дмитрий Петрович', N'Рептилии', 8)
SET IDENTITY_INSERT [dbo].[Trainers] OFF
GO
ALTER TABLE [dbo].[Artists] ADD  DEFAULT ((0)) FOR [SuccessfulPerformances]
GO
ALTER TABLE [dbo].[Artists] ADD  DEFAULT ('Beginner') FOR [Type]
GO
ALTER TABLE [dbo].[Events] ADD  DEFAULT ((0)) FOR [Prepayment]
GO
ALTER TABLE [dbo].[Events] ADD  DEFAULT ((0)) FOR [IsCompleted]
GO
ALTER TABLE [dbo].[Animals]  WITH CHECK ADD  CONSTRAINT [FK_Animals_Trainers] FOREIGN KEY([TrainerID])
REFERENCES [dbo].[Trainers] ([TrainerID])
GO
ALTER TABLE [dbo].[Animals] CHECK CONSTRAINT [FK_Animals_Trainers]
GO
ALTER TABLE [dbo].[Holograms]  WITH CHECK ADD  CONSTRAINT [FK_Holograms_Employees] FOREIGN KEY([ResponsibleID])
REFERENCES [dbo].[Employees] ([EmployeeID])
GO
ALTER TABLE [dbo].[Holograms] CHECK CONSTRAINT [FK_Holograms_Employees]
GO
/****** Object:  StoredProcedure [dbo].[UpdateArtistTypes]    Script Date: 13.06.2025 18:08:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- Create a stored procedure for artist type update
CREATE PROCEDURE [dbo].[UpdateArtistTypes]
AS
BEGIN
    UPDATE Artists
    SET Type = CASE 
        WHEN SuccessfulPerformances BETWEEN 0 AND 10 THEN 'Beginner'
        WHEN SuccessfulPerformances BETWEEN 11 AND 30 THEN 'Promoting'
        ELSE 'VIP'
    END,
    DressingRoom = CASE 
        WHEN SuccessfulPerformances > 30 THEN CONCAT('VIP-', ArtistID)
        ELSE NULL
    END
END;
GO
USE [master]
GO
ALTER DATABASE [CircusManagement] SET  READ_WRITE 
GO
