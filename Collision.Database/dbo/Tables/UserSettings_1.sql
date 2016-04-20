CREATE TABLE [dbo].[UserSettings] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [UserId]   NVARCHAR (128) NOT NULL,
    [Distance] NVARCHAR (10)  NOT NULL,
    CONSTRAINT [PK_UserSettings] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserSettings_AspNetUsers] FOREIGN KEY ([UserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
);

