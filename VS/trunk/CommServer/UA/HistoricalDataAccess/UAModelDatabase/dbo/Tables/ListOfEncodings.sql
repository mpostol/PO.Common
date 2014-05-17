CREATE TABLE [dbo].[ListOfEncodings] (
    [DataTypeDesignIndex] UNIQUEIDENTIFIER NOT NULL,
    [NodeIndex]           UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [FK_ListOfEncodings_DataTypeDesign] FOREIGN KEY ([DataTypeDesignIndex]) REFERENCES [dbo].[DataTypeDesign] ([DataTypeDesignIndex]),
    CONSTRAINT [FK_ListOfEncodings_ObjectDesign] FOREIGN KEY ([NodeIndex]) REFERENCES [dbo].[ObjectDesign] ([NodeIndex])
);

