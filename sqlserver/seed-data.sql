-- Create a bare minimum sql file that creates a PCS_v2_at database from scratch with minimum schema and data to support gitlab pipeline ATs
-- Created by amalgamating PCS_v2_Schema.sql, PCS_v2_Static_Data.sql & db\Environments\dev\ConfigurationData.sql files and stripping out configuration for unnecessary configuration
-- 'sa' is the only user configured on the SQL Server container so 'sa' will be set as the owner of the database - no other users or roles required
-- Removed GRANT statements from the original schema
-- Removed all USE statements except the ones at the top of the script
-- Removed GO statements after each INSERT - kept one GO at the end of each table data insertion
-- Included merchants are 1, 13, 25, 256, 275, and 999 which is a pipeline only merchant similar to merchant 256

USE [master]
GO

IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = 'PCS_v2_at')
BEGIN
	ALTER DATABASE [PCS_v2_at] SET RESTRICTED_USER WITH ROLLBACK IMMEDIATE
END

DROP DATABASE IF EXISTS [PCS_v2_at]
GO

CREATE DATABASE [PCS_v2_at]
GO
PRINT 'Database created'

ALTER DATABASE [PCS_v2_at] SET RECOVERY SIMPLE
GO

USE [PCS_v2_at]
GO

sp_changedbowner 'sa'
GO

--- Schema
-- Types
CREATE TYPE dbo.Description FROM varchar(100)
GO
PRINT 'Description user defined data type created'

CREATE TYPE dbo.EntityName FROM nvarchar(100)
GO
PRINT 'Entity name user defined data type created'

CREATE TYPE dbo.ExchangeRate FROM decimal(14, 7)
GO
PRINT 'ExchangeRate user defined data type created'

CREATE TYPE dbo.Percentage FROM decimal(8, 6)
GO
PRINT 'Percentage user defined data type created'

CREATE TYPE dbo.UserName FROM nvarchar(50)
GO
PRINT 'UserName user defined data type created'



CREATE TABLE dbo.Lookup(
	Id															uniqueidentifier NOT NULL,
	ContinuumOrderIdentifier									varchar(50) NOT NULL,
	MerchantOrderIdentifier										nvarchar(50) NULL,
	MerchantId													int NOT NULL,
	SaleCurrencyId												int NOT NULL,
	SaleValue													money NOT NULL,
	ResultCodeId												int NOT NULL,
	CreationTimestamp											datetime NOT NULL,
	VersionSequence												int	NOT NULL,
	OrderSessionId												nvarchar(300) NULL
) ON [PRIMARY]
ALTER TABLE dbo.Lookup ADD CONSTRAINT PK_Lookup PRIMARY KEY CLUSTERED (Id)
ALTER TABLE dbo.Lookup ADD CONSTRAINT IX_Lookup_ContinuumOrderIdentifier UNIQUE NONCLUSTERED (ContinuumOrderIdentifier)
CREATE NONCLUSTERED INDEX IX_Lookup_CreationTimestamp ON dbo.Lookup (CreationTimestamp)
PRINT 'Lookup table created'


-- CREATE TABLE dbo.LookupEntry(
-- 	Id															uniqueidentifier NOT NULL,
-- 	LookupId													uniqueidentifier NOT NULL,
-- 	MessageTypeId												int NOT NULL,
-- 	MerchantOrderIdentifier										nvarchar(50) NULL,
-- 	PSPRequestIdentifier										nvarchar(50) NULL,
-- 	Reason														nvarchar(200) NULL,
-- 	SaleCurrencyId												int NOT NULL,
-- 	SaleValue													money NOT NULL,
-- 	SaleMarginValue												money NOT NULL,
-- 	PaymentCurrencyId											int NOT NULL,
-- 	PaymentValue												money NOT NULL,
-- 	PaymentMarginValue											money NOT NULL,
-- 	ActionTimestamp												datetime NOT NULL,
-- 	AuthorisationCode											varchar(8) NULL,
-- 	ActionReversedId											uniqueidentifier NULL,
-- 	RequestSupplementaryData									nvarchar(1000) NULL,
-- 	ResponseSupplementaryData									nvarchar(1000) NULL,
-- 	CreatedBy													dbo.UserName NOT NULL,
-- 	CreationTimestamp											datetime NOT NULL,
-- 	Sequence													int NOT NULL,
-- 	PaymentMethod												nvarchar(50) NULL,
-- 	-- NOTE: Nonce datatype is NULL as database must initially allow for historical data contaning NULLs. Once archiving has removed historical data
-- 	-- the database schema can be changed to NOT NULL and PCS code updated so as to make Nonce field non-nullable.
-- 	Nonce														uniqueidentifier NULL,
-- 	OriginalBillIdentifier										nvarchar(50) NULL,
-- 	CardBIN														int NULL
-- ) ON [PRIMARY]
-- ALTER TABLE dbo.LookupEntry ADD CONSTRAINT PK_LookupEntry PRIMARY KEY CLUSTERED (Id)
-- ALTER TABLE dbo.LookupEntry ADD CONSTRAINT FK_LookupEntry_Lookup FOREIGN KEY(LookupId) REFERENCES dbo.Lookup (Id)
-- CREATE INDEX IX_LookupEntry_LookupId ON dbo.LookupEntry (LookupId)
-- CREATE NONCLUSTERED INDEX IX_LookupEntry_CreationTimestamp ON dbo.LookupEntry (CreationTimestamp)
-- PRINT 'LookupEntry table created'

CREATE TABLE dbo.OutboxMessages(
	Id															uniqueidentifier NOT NULL,
	MessageType													varchar(50) NOT NULL,
	MessageJSON													nvarchar(MAX) NULL,
	OccurredTimestamp											datetime NOT NULL,
	ProcessedTimestamp											datetime NULL,
	VersionSequence												int	NOT NULL,
	Error														nvarchar(300) NULL
) ON [PRIMARY]
ALTER TABLE dbo.OutboxMessages ADD CONSTRAINT PK_OutboxMessages PRIMARY KEY CLUSTERED (Id)
-- ALTER TABLE dbo.Lookup ADD CONSTRAINT IX_Lookup_ContinuumOrderIdentifier UNIQUE NONCLUSTERED (ContinuumOrderIdentifier)
-- CREATE NONCLUSTERED INDEX IX_Lookup_CreationTimestamp ON dbo.Lookup (CreationTimestamp)
-- PRINT 'Lookup table created'

