CREATE TABLE [dbo].[ObjectTypeDesign] (
    [NodeIndex]      UNIQUEIDENTIFIER NOT NULL,
    [SupportsEvents] BIT              NULL,
    CONSTRAINT [PK_ObjectTypeDesign] PRIMARY KEY CLUSTERED ([NodeIndex] ASC),
    CONSTRAINT [FK_ObjectTypeDesign_TypeDesign] FOREIGN KEY ([NodeIndex]) REFERENCES [dbo].[TypeDesign] ([NodeIndex])
);

