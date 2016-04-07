CREATE TABLE [dbo].[Aircraft] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [Iata]         NVARCHAR (50) NULL,
    [Icao]         NVARCHAR (50) NULL,
    [Carrier]      NVARCHAR (50) NULL,
    [CarrierName]  NVARCHAR (50) NULL,
    [FlightNumber] NVARCHAR (50) NULL,
    [PlaneType]    NVARCHAR (50) NULL,
    [IsActive]     BIT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

