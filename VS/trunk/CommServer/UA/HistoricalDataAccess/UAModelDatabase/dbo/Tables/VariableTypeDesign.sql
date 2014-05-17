CREATE TABLE [dbo].[VariableTypeDesign] (
    [NodeIndex]               UNIQUEIDENTIFIER NOT NULL,
    [DefaultValue]            UNIQUEIDENTIFIER NULL,
    [DataTypeDesignIndex]     UNIQUEIDENTIFIER NULL,
    [ValueRank]               INT              NULL,
    [ArrayDimensions]         NCHAR (10)       NULL,
    [AccessLevel]             INT              NULL,
    [MinimumSamplingInterval] INT              NULL,
    [Historizing]             BIT              NULL,
    [ExposesItsChildren]      BIT              NULL,
    CONSTRAINT [PK_VariableTypeDesign] PRIMARY KEY CLUSTERED ([NodeIndex] ASC),
    CONSTRAINT [FK_VariableTypeDesign_DataTypeDesign] FOREIGN KEY ([DataTypeDesignIndex]) REFERENCES [dbo].[DataTypeDesign] ([DataTypeDesignIndex])
);

