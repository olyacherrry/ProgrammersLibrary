CREATE TABLE [dbo].[Orders] (
    [OrderId]      INT            IDENTITY (1, 1) NOT NULL,
	[BookId]       INT			  NOT NULL,
    [UserId]       NVARCHAR (128) NOT NULL,
    [DateStarting] DATETIME       NOT NULL,
    [DateEnding]   DATETIME       NOT NULL,
    [Phone]        NVARCHAR (MAX) NULL,
    [Adress]       NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED ([OrderId] ASC)
);

