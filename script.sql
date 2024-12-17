IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
CREATE TABLE [AuctionItems] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(200) NOT NULL,
    [StartingPrice] int NOT NULL,
    [CurrentPrice] int NOT NULL,
    [AuctionStart] datetime2 NOT NULL,
    [AuctionEnd] datetime2 NOT NULL,
    CONSTRAINT [PK_AuctionItems] PRIMARY KEY ([Id])
);

CREATE TABLE [Bids] (
    [Id] int NOT NULL IDENTITY,
    [FirstName] nvarchar(100) NOT NULL,
    [LastName] nvarchar(100) NOT NULL,
    [Email] nvarchar(200) NOT NULL,
    [Phone] nvarchar(20) NOT NULL,
    [BidAmount] int NOT NULL,
    [AuctionItemId] int NOT NULL,
    [BidTime] datetime2 NOT NULL,
    CONSTRAINT [PK_Bids] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Bids_AuctionItems_AuctionItemId] FOREIGN KEY ([AuctionItemId]) REFERENCES [AuctionItems] ([Id]) ON DELETE NO ACTION
);

CREATE INDEX [IX_Bids_AuctionItemId] ON [Bids] ([AuctionItemId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20241215113427_Initial', N'9.0.0');

COMMIT;
GO

