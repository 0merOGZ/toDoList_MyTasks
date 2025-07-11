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
CREATE TABLE [Categories] (
    [categoryId] nvarchar(450) NOT NULL,
    [categoryName] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY ([categoryId])
);

CREATE TABLE [Statuses] (
    [statusId] nvarchar(450) NOT NULL,
    [statusName] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Statuses] PRIMARY KEY ([statusId])
);

CREATE TABLE [Todos] (
    [todId] int NOT NULL IDENTITY,
    [todName] nvarchar(max) NOT NULL,
    [todDescription] nvarchar(max) NOT NULL,
    [dueDate] datetime2 NOT NULL,
    [categoryId] nvarchar(450) NOT NULL,
    [statusId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_Todos] PRIMARY KEY ([todId]),
    CONSTRAINT [FK_Todos_Categories_categoryId] FOREIGN KEY ([categoryId]) REFERENCES [Categories] ([categoryId]) ON DELETE CASCADE,
    CONSTRAINT [FK_Todos_Statuses_statusId] FOREIGN KEY ([statusId]) REFERENCES [Statuses] ([statusId]) ON DELETE CASCADE
);

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'categoryId', N'categoryName') AND [object_id] = OBJECT_ID(N'[Categories]'))
    SET IDENTITY_INSERT [Categories] ON;
INSERT INTO [Categories] ([categoryId], [categoryName])
VALUES (N'finance', N'Finance'),
(N'health', N'Health'),
(N'other', N'Other'),
(N'personal', N'Personal'),
(N'relationship', N'Relationship'),
(N'school', N'School'),
(N'work', N'Work');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'categoryId', N'categoryName') AND [object_id] = OBJECT_ID(N'[Categories]'))
    SET IDENTITY_INSERT [Categories] OFF;

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'statusId', N'statusName') AND [object_id] = OBJECT_ID(N'[Statuses]'))
    SET IDENTITY_INSERT [Statuses] ON;
INSERT INTO [Statuses] ([statusId], [statusName])
VALUES (N'completed', N'Completed'),
(N'pending', N'Pending');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'statusId', N'statusName') AND [object_id] = OBJECT_ID(N'[Statuses]'))
    SET IDENTITY_INSERT [Statuses] OFF;

CREATE INDEX [IX_Todos_categoryId] ON [Todos] ([categoryId]);

CREATE INDEX [IX_Todos_statusId] ON [Todos] ([statusId]);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250625075121_Initial', N'9.0.6');

ALTER TABLE [Todos] ADD [userId] int NULL;

CREATE TABLE [Teams] (
    [teamId] int NOT NULL IDENTITY,
    [teamName] nvarchar(max) NOT NULL,
    [teamInvitationCode] nvarchar(max) NOT NULL,
    [teamLeader] int NOT NULL,
    CONSTRAINT [PK_Teams] PRIMARY KEY ([teamId])
);

CREATE TABLE [Users] (
    [userId] int NOT NULL IDENTITY,
    [userName] nvarchar(max) NOT NULL,
    [userMail] nvarchar(max) NOT NULL,
    [userPassword] nvarchar(max) NOT NULL,
    [userRole] nvarchar(max) NOT NULL,
    [teamId] int NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([userId]),
    CONSTRAINT [FK_Users_Teams_teamId] FOREIGN KEY ([teamId]) REFERENCES [Teams] ([teamId]) ON DELETE SET NULL
);

CREATE INDEX [IX_Todos_userId] ON [Todos] ([userId]);

CREATE INDEX [IX_Teams_teamLeader] ON [Teams] ([teamLeader]);

CREATE INDEX [IX_Users_teamId] ON [Users] ([teamId]);

ALTER TABLE [Todos] ADD CONSTRAINT [FK_Todos_Users_userId] FOREIGN KEY ([userId]) REFERENCES [Users] ([userId]);

ALTER TABLE [Teams] ADD CONSTRAINT [FK_Teams_Users_teamLeader] FOREIGN KEY ([teamLeader]) REFERENCES [Users] ([userId]) ON DELETE NO ACTION;

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250704115843_AddUserAndTeamTables', N'9.0.6');

ALTER TABLE [Todos] ADD [ArchivedDate] datetime2 NULL;

ALTER TABLE [Todos] ADD [IsArchived] bit NOT NULL DEFAULT CAST(0 AS bit);

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250708204205_ArchiveSupport', N'9.0.6');

ALTER TABLE [Todos] ADD [urgency] nvarchar(max) NOT NULL DEFAULT N'';

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250709062717_SyncModelWithDb', N'9.0.6');

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20250709204127_AddProgressToTodo', N'9.0.6');

COMMIT;
GO

