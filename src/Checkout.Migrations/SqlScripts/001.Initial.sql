CREATE TABLE [dbo].[EventStoreEvents](
	[EventId] [uniqueidentifier] NOT NULL,
	[EventData] [nvarchar](max) NOT NULL,
	[EventType] [varchar](300) NOT NULL,
	[CorrelationId] [uniqueidentifier] NULL,
	[StreamId] [varchar](200) NOT NULL,
	[StreamVersion] int NOT NULL
 CONSTRAINT [PK_EventStoreEvents] PRIMARY KEY NONCLUSTERED([EventId] ASC)
);

CREATE CLUSTERED INDEX [IX_EventStoreEvents_StreamId] 
	ON [dbo].[EventStoreEvents](StreamId);

CREATE UNIQUE NONCLUSTERED INDEX [IX_EventStoreEvents_StreamId_StreamVersion] 
	ON [dbo].[EventStoreEvents](StreamId, StreamVersion); 

CREATE TYPE dbo.NewEventStoreEvents AS TABLE (
        OrderNo             INT IDENTITY                            NOT NULL,
        EventId             UNIQUEIDENTIFIER                        NOT NULL,
        EventData           NVARCHAR(max)                           NOT NULL,
        EventType           VARCHAR(300)                            NOT NULL,
		CorrelationId       UNIQUEIDENTIFIER                        NULL
);


CREATE TABLE [dbo].[Goods](
	[GoodId] [uniqueidentifier] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](50) NULL,
	[Stock] [int] NOT NULL,
	[Price] [decimal] NOT NULL,
 CONSTRAINT [PK_Goods] PRIMARY KEY NONCLUSTERED([GoodId] ASC)
);


INSERT INTO dbo.Goods
VALUES (newid(), 'Samsung','Samsung Description',1,12.1);
INSERT INTO dbo.Goods
VALUES (newid(), 'Apple','Apple Description',2,16.5);
INSERT INTO dbo.Goods
VALUES (newid(), 'TV','TV Description',3,20.67);
INSERT INTO dbo.Goods
VALUES (newid(), 'PC','PC Description',10,12.9);