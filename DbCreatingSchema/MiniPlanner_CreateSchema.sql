-- Creating ExtremeSportDatabase
USE master;
IF DB_ID(N'MiniPlannerDb') IS NOT NULL
	DROP DATABASE MiniPlannerDb;
CREATE DATABASE MiniPlannerDb ON
(
	NAME = MiniPlannerDb,
	-- Change Your path and correct: SELECT filename FROM sys.sysaltfiles;
	FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\MiniPlannerDb.mdf',
	SIZE = 5,
	MAXSIZE = UNLIMITED,
	FILEGROWTH = 1
)
LOG ON
(
	NAME = MiniPlannerDbLog,
	-- Change Your path and correct: SELECT filename FROM sys.sysaltfiles;
	FILENAME = 'C:\Program Files\Microsoft SQL Server\MSSQL13.MSSQLSERVER\MSSQL\DATA\MiniPlannerDbLog.ldf',
	SIZE = 1,
	MAXSIZE = UNLIMITED,
	FILEGROWTH = 10%
);

USE MiniPlannerDb;

IF OBJECT_ID(N'dbo.Preferences', N'U') IS NOT NULL
	DROP TABLE dbo.Preferences;
IF OBJECT_ID(N'dbo.Users', N'U') IS NOT NULL
	DROP TABLE dbo.Users;
IF OBJECT_ID(N'dbo.Courses', N'U') IS NOT NULL
	DROP TABLE dbo.Courses;
IF OBJECT_ID(N'dbo.Registration', N'U') IS NOT NULL
	DROP TABLE dbo.Registration;


CREATE TABLE dbo.Users
(
	id						INT IDENTITY(1, 1)	NOT NULL PRIMARY KEY,
	login					NVARCHAR(20)		NOT NULL UNIQUE,
	cardId					NVARCHAR(10)		NOT NULL UNIQUE,
	password				NVARCHAR(1000)		NOT	NULL,
	averageScore			FLOAT				NOT NULL,
	isAdmin					BIT					NOT NULL
);

CREATE TABLE dbo.Courses
(
	id						INT IDENTITY(1, 1)	NOT NULL PRIMARY KEY,
	name					NVARCHAR(100)		NOT NULL,
	day						NVARCHAR(20)		NOT NULL,
	startHour				INT					NOT NULL,
	endHour					INT					NOT NULL,
	limit					INT					NOT NULL,
	UNIQUE(name, day, startHour, endHour)
);

CREATE TABLE dbo.Preferences
(
	id						INT IDENTITY(1, 1)	NOT NULL PRIMARY KEY,
	courseId				INT					NOT NULL FOREIGN KEY REFERENCES dbo.Courses(id),
	userId					INT					NOT NULL FOREIGN KEY REFERENCES dbo.Users(id),
	scheduleId				INT					NOT NULL
);

CREATE TABLE dbo.Registration
(
	id		INT IDENTITY(1, 1)	NOT NULL PRIMARY KEY,
	status	NVARCHAR(100)		NOT NULL
);

INSERT INTO dbo.Registration VALUES ('Otwarta');

INSERT INTO dbo.Courses VALUES
	(N'Projekt Indywidualny', N'Środa', 16, 19, 15),
	(N'Projekt Indywidualny', N'Czwartek', 14, 17, 15),
	(N'Projekt Indywidualny', N'Czwartek', 17, 20, 15),
	(N'Projekt Indywidualny', N'Piątek', 8, 11, 15),
	(N'Projekt Indywidualny', N'Piątek', 11, 14, 15),
	(N'Projekt Indywidualny', N'Piątek', 14, 17, 15),
	(N'Inżynieria Oprogramowania 2', N'Poniedziałek', 8, 11, 15),
	(N'Inżynieria Oprogramowania 2', N'Poniedziałek', 14, 17, 15),
	(N'Inżynieria Oprogramowania 2', N'Czwartek', 14, 17, 15),
	(N'Inżynieria Oprogramowania 2', N'Czwartek', 17, 20, 15),
	(N'Inżynieria Oprogramowania 2', N'Piątek', 14, 17, 15),
	(N'Inżynieria Oprogramowania 2', N'Piątek', 17, 20, 15),
	(N'Metody Sztucznej Inteligencji', N'Wtorek', 18, 20, 15),
	(N'Metody Sztucznej Inteligencji', N'Środa', 10, 12, 15),
	(N'Metody Sztucznej Inteligencji', N'Środa', 14, 16, 15),
	(N'Metody Sztucznej Inteligencji', N'Środa', 16, 18, 15),
	(N'Metody Sztucznej Inteligencji', N'Piątek', 14, 16, 15),
	(N'Metody Sztucznej Inteligencji', N'Piątek', 18, 20, 15),
	(N'UNIX', N'Wtorek', 12, 14, 36),
	(N'UNIX', N'Środa', 14, 16, 36),
	(N'Programowanie FPGA', N'Poniedziałek', 12, 14, 7),
	(N'Programowanie FPGA', N'Poniedziałek', 14, 16, 7),
	(N'Wstęp do Systemów Wbudowanych', N'Piątek', 11, 14, 16),
	(N'Wstęp do Systemów Wbudowanych', N'Piątek', 14, 17, 16),
	(N'Linux w Systemach Wbudowanych', N'Czwartek', 18, 20, 20),
	(N'Linux w Systemach Wbudowanych', N'Piątek', 16, 18, 20),
	(N'Laboratorium CAD/CAM', N'Poniedziałek', 11, 13, 15),
	(N'Laboratorium CAD/CAM', N'Piątek', 8, 10, 15),
	(N'Programowanie GPU', N'Poniedziałek', 14, 16, 15),
	(N'Programowanie GPU', N'Środa', 8, 10, 15),
	(N'Android', N'Czwartek', 14, 16, 15),
	(N'Android', N'Piątek', 8, 10, 15),
	(N'Android', N'Piątek', 10, 12, 15),
	(N'Android', N'Piątek',12, 14, 15);

/*
	SELECT * FROM dbo.Users;
	SELECT * FROM dbo.Courses;
	SELECT * FROM dbo.Preferences;
	SELECT * FROM dbo.Registration;
*/