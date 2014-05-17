CREATE TABLE [dbo].[InstanceDesign] (
    [NodeIndex] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_InstanceDesign] PRIMARY KEY CLUSTERED ([NodeIndex] ASC),
    CONSTRAINT [FK_InstanceDesign_MethodDesign] FOREIGN KEY ([NodeIndex]) REFERENCES [dbo].[MethodDesign] ([NodeIndex]),
    CONSTRAINT [FK_InstanceDesign_VariableDesign] FOREIGN KEY ([NodeIndex]) REFERENCES [dbo].[VariableDesign] ([NodeIndex])
);

