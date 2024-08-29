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
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240828122209_db'
)
BEGIN
    CREATE TABLE [Drawings] (
        [Name] nvarchar(450) NOT NULL,
        [UserName] nvarchar(max) NOT NULL,
        [CreatedAt] datetime2 NOT NULL,
        CONSTRAINT [PK_Drawings] PRIMARY KEY ([Name])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240828122209_db'
)
BEGIN
    CREATE TABLE [DrawingStates] (
        [Id] uniqueidentifier NOT NULL,
        [Value] nvarchar(max) NOT NULL,
        [DrawingName] nvarchar(450) NOT NULL,
        CONSTRAINT [PK_DrawingStates] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_DrawingStates_Drawings_DrawingName] FOREIGN KEY ([DrawingName]) REFERENCES [Drawings] ([Name]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240828122209_db'
)
BEGIN
    CREATE INDEX [IX_DrawingStates_DrawingName] ON [DrawingStates] ([DrawingName]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20240828122209_db'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20240828122209_db', N'8.0.7');
END;
GO

COMMIT;
GO

