CREATE TABLE [dbo].[ListOfFields] (
    [DataTypeDesignIndex] UNIQUEIDENTIFIER NULL,
    [ParameterIndex]      UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_ListOfFields] PRIMARY KEY CLUSTERED ([ParameterIndex] ASC),
    CONSTRAINT [FK_ListOfFields_DataTypeDesign] FOREIGN KEY ([DataTypeDesignIndex]) REFERENCES [dbo].[DataTypeDesign] ([DataTypeDesignIndex])
);

