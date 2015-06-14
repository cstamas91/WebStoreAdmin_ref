
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/22/2015 00:36:50
-- Generated from EDMX file: D:\Elte\waf\ITStore_incompatible\ITStore\ITStore\Models\ITStoreEntities.edmx
-- --------------------------------------------------

CREATE DATABASE ITStore;
GO


SET QUOTED_IDENTIFIER OFF;
GO
USE [ITStore];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ConnectionToProduct]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PurchaseToProduct] DROP CONSTRAINT [FK_ConnectionToProduct];
GO
IF OBJECT_ID(N'[dbo].[FK_ConnectionToPurchase]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PurchaseToProduct] DROP CONSTRAINT [FK_ConnectionToPurchase];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductToCategory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Product] DROP CONSTRAINT [FK_ProductToCategory];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Category]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Category];
GO
IF OBJECT_ID(N'[dbo].[Product]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Product];
GO
IF OBJECT_ID(N'[dbo].[Purchases]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Purchases];
GO
IF OBJECT_ID(N'[dbo].[PurchaseToProduct]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PurchaseToProduct];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Category'
CREATE TABLE [dbo].[Category] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(40)  NOT NULL
);
GO

-- Creating table 'Product'
CREATE TABLE [dbo].[Product] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [CategoryID] int  NOT NULL,
    [Manufacturer] varchar(40)  NOT NULL,
    [Model] varchar(40)  NOT NULL,
    [Descr] varchar(1000)  NOT NULL,
    [Price] int  NOT NULL,
    [Stock] int  NOT NULL,
    [IsActive] bit  NOT NULL
);
GO

-- Creating table 'Purchases'
CREATE TABLE [dbo].[Purchases] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] varchar(40)  NOT NULL,
    [Addr] varchar(40)  NOT NULL,
    [Phone] varchar(20)  NOT NULL,
    [Email] varchar(40)  NOT NULL,
    [Completion] bit  NOT NULL
);
GO

-- Creating table 'PurchaseToProduct'
CREATE TABLE [dbo].[PurchaseToProduct] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ProductID] int  NOT NULL,
    [PurchaseID] int  NOT NULL,
    [Quantity] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'Category'
ALTER TABLE [dbo].[Category]
ADD CONSTRAINT [PK_Category]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Product'
ALTER TABLE [dbo].[Product]
ADD CONSTRAINT [PK_Product]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Purchases'
ALTER TABLE [dbo].[Purchases]
ADD CONSTRAINT [PK_Purchases]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'PurchaseToProduct'
ALTER TABLE [dbo].[PurchaseToProduct]
ADD CONSTRAINT [PK_PurchaseToProduct]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [CategoryID] in table 'Product'
ALTER TABLE [dbo].[Product]
ADD CONSTRAINT [FK_ProductToCategory]
    FOREIGN KEY ([CategoryID])
    REFERENCES [dbo].[Category]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductToCategory'
CREATE INDEX [IX_FK_ProductToCategory]
ON [dbo].[Product]
    ([CategoryID]);
GO

-- Creating foreign key on [ProductID] in table 'PurchaseToProduct'
ALTER TABLE [dbo].[PurchaseToProduct]
ADD CONSTRAINT [FK_ConnectionToProduct]
    FOREIGN KEY ([ProductID])
    REFERENCES [dbo].[Product]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ConnectionToProduct'
CREATE INDEX [IX_FK_ConnectionToProduct]
ON [dbo].[PurchaseToProduct]
    ([ProductID]);
GO

-- Creating foreign key on [PurchaseID] in table 'PurchaseToProduct'
ALTER TABLE [dbo].[PurchaseToProduct]
ADD CONSTRAINT [FK_ConnectionToPurchase]
    FOREIGN KEY ([PurchaseID])
    REFERENCES [dbo].[Purchases]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ConnectionToPurchase'
CREATE INDEX [IX_FK_ConnectionToPurchase]
ON [dbo].[PurchaseToProduct]
    ([PurchaseID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------