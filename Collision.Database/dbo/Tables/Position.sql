CREATE TABLE [dbo].[Position](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](500) NULL,
	[AircraftId] [int] NOT NULL,
	[FlightId] [nvarchar](500) NULL,
	[FlightIdentity] [nvarchar](500) NULL,
	[Temp1Latitude] [decimal](9, 6) NOT NULL,
	[Temp1Longitude] [decimal](9, 6) NOT NULL,
	[Temp1Altitude] [int] NOT NULL,
	[Temp1Speed] [int] NOT NULL,
	[Temp1Heading] [int] NOT NULL,
	[Temp1TimeStamp] [datetime] NOT NULL,
	[Temp1EpochTimeStamp] [bigint] NULL,
	[Temp2Latitude] [decimal](9, 6) NULL,
	[Temp2Longitude] [decimal](9, 6) NULL,
	[Temp2Altitude] [int] NULL,
	[Temp2Speed] [int] NULL,
	[Temp2Heading] [int] NULL,
	[Temp2TimeStamp] [datetime] NULL,
	[Temp2EpochTimeStamp] [bigint] NULL,
	[X1] [decimal](9, 6) NULL,
	[Y1] [decimal](9, 6) NULL,
	[Z1] [int] NULL,
	[X2] [decimal](9, 6) NULL,
	[Y2] [decimal](9, 6) NULL,
	[Z2] [int] NULL,
	[X3] [decimal](9, 6) NULL,
	[Y3] [decimal](9, 6) NULL,
	[Z3] [int] NULL,
	[X4] [decimal](9, 6) NULL,
	[Y4] [decimal](9, 6) NULL,
	[Z4] [int] NULL,
	[X5] [decimal](9, 6) NULL,
	[Y5] [decimal](9, 6) NULL,
	[Z5] [int] NULL,
	[X6] [decimal](9, 6) NULL,
	[Y6] [decimal](9, 6) NULL,
	[Z6] [int] NULL,
	[X7] [decimal](9, 6) NULL,
	[Y7] [decimal](9, 6) NULL,
	[Z7] [int] NULL,
	[X8] [decimal](9, 6) NULL,
	[Y8] [decimal](9, 6) NULL,
	[Z8] [int] NULL,
	[CreatedAtTimeStamp] [datetime] NULL,
	[IsInFlight] [bit] NOT NULL,
 CONSTRAINT [PK_Position] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Position] ADD  CONSTRAINT [DF_Position_IsInFlight]  DEFAULT ((0)) FOR [IsInFlight]
GO

ALTER TABLE [dbo].[Position]  ADD  CONSTRAINT [FK_Position_Aircraft] FOREIGN KEY([AircraftId])
REFERENCES [dbo].[Aircraft] ([Id])
GO

ALTER TABLE [dbo].[Position] CHECK CONSTRAINT [FK_Position_Aircraft]