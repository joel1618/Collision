CREATE TABLE [dbo].[Conflict] (
    [Id]                     INT      IDENTITY (1, 1) NOT NULL,
    [PositionId1]            INT      NOT NULL,
    [PositionId2]            INT      NOT NULL,
    [ModifiedAtUtcTimeStamp] DATETIME NULL,
    [CreatedAtUtcTimeStamp]  DATETIME NOT NULL,
    [IsActive]               BIT      NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Conflict_Position1] FOREIGN KEY ([PositionId1]) REFERENCES [dbo].[Position] ([Id]),
    CONSTRAINT [FK_Conflict_Position2] FOREIGN KEY ([PositionId2]) REFERENCES [dbo].[Position] ([Id])
);

