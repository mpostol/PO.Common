CREATE TABLE [dbo].[VariableValue] (
    [NodeIndex] UNIQUEIDENTIFIER NOT NULL,
    [Value]     SQL_VARIANT      NULL,
    [TimeStamp] ROWVERSION       NULL,
    [Quality]   FLOAT (53)       NULL,
    CONSTRAINT [FK_VariableValue_VariableDesign] FOREIGN KEY ([NodeIndex]) REFERENCES [dbo].[VariableDesign] ([NodeIndex])
);

