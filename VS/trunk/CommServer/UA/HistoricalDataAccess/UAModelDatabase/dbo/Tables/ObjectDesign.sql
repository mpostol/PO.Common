CREATE TABLE [dbo].[ObjectDesign] (
    [NodeIndex]      UNIQUEIDENTIFIER NOT NULL,
    [SupportsEvents] BIT              NULL,
    CONSTRAINT [PK_ObjectDesign] PRIMARY KEY CLUSTERED ([NodeIndex] ASC),
    CONSTRAINT [FK_ObjectDesign_InstanceDesign] FOREIGN KEY ([NodeIndex]) REFERENCES [dbo].[InstanceDesign] ([NodeIndex])
);

