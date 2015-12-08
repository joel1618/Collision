CREATE TABLE [dbo].[Aircraft](
	[Id] [int] NOT NULL,
	[Carrier] [nvarchar](50) NULL,
	[CarrierName] [nvarchar](50) NULL,
	[FlightNumber] [nvarchar](50) NULL,
	[PlaneType] [nvarchar](50) NULL,
 CONSTRAINT [PK_Aircraft] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]