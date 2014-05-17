CREATE TABLE [dbo].[Parameter] (
    [ParameterIndex]      UNIQUEIDENTIFIER NOT NULL,
    [Description]         NCHAR (10)       NULL,
    [DescriptionKey]      NCHAR (10)       NULL,
    [Name]                NCHAR (10)       NULL,
    [Identifier]          INT              NULL,
    [DataTypeDesignIndex] UNIQUEIDENTIFIER NOT NULL,
    [ValueRank]           INT              NULL,
    CONSTRAINT [PK_Parameter] PRIMARY KEY CLUSTERED ([ParameterIndex] ASC),
    CONSTRAINT [FK_Parameter_DataTypeDesign] FOREIGN KEY ([DataTypeDesignIndex]) REFERENCES [dbo].[DataTypeDesign] ([DataTypeDesignIndex]),
    CONSTRAINT [FK_Parameter_InputArguments] FOREIGN KEY ([ParameterIndex]) REFERENCES [dbo].[InputArguments] ([ParameterIndex]),
    CONSTRAINT [FK_Parameter_ListOfFields] FOREIGN KEY ([ParameterIndex]) REFERENCES [dbo].[ListOfFields] ([ParameterIndex])
);

