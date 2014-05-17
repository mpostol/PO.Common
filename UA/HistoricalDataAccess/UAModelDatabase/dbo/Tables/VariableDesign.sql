CREATE TABLE [dbo].[VariableDesign] (
    [NodeIndex]               UNIQUEIDENTIFIER NOT NULL,
    [DefaultValueIndex]       UNIQUEIDENTIFIER NULL,
    [DataTypeDesignIndex]     UNIQUEIDENTIFIER NULL,
    [ValueRank]               INT              NULL,
    [ArrayDimensions]         CHAR (10)        NULL,
    [AccessLevel]             INT              NULL,
    [MinimumSamplingInterval] INT              NULL,
    [Historizing]             BIT              NULL,
    [IsProperty]              BIT              NOT NULL,
    CONSTRAINT [PK_VariableDesign] PRIMARY KEY CLUSTERED ([NodeIndex] ASC),
    CONSTRAINT [FK_VariableDesign_DataTypeDesign] FOREIGN KEY ([DataTypeDesignIndex]) REFERENCES [dbo].[DataTypeDesign] ([DataTypeDesignIndex]),
    CONSTRAINT [FK_VariableDesign_DefaultValue] FOREIGN KEY ([DefaultValueIndex]) REFERENCES [dbo].[DefaultValue] ([DefaultValueIndex])
);

