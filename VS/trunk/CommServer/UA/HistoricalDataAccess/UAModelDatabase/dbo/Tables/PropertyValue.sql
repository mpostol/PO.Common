CREATE TABLE [dbo].[PropertyValue] (
    [NodeIndex] UNIQUEIDENTIFIER NOT NULL,
    [Value]     SQL_VARIANT      NULL,
    [TimeStamp] ROWVERSION       NULL,
    CONSTRAINT [FK_PropertyValue_VariableDesign] FOREIGN KEY ([NodeIndex]) REFERENCES [dbo].[VariableDesign] ([NodeIndex])
);

