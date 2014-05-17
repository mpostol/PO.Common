CREATE TABLE [dbo].[TypeDesign] (
    [NodeIndex] UNIQUEIDENTIFIER NOT NULL,
    [ClassName] SQL_VARIANT      NULL,
    [BaseType]  UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_TypeDesign] PRIMARY KEY CLUSTERED ([NodeIndex] ASC),
    CONSTRAINT [FK_TypeDesign_NodeDesign_BaseType] FOREIGN KEY ([BaseType]) REFERENCES [dbo].[NodeDesign] ([NodeDesignIndex]),
    CONSTRAINT [FK_TypeDesign_ReferenceTypeDesign] FOREIGN KEY ([NodeIndex]) REFERENCES [dbo].[ReferenceTypeDesign] ([ReferenceTypeDesignIndex]),
    CONSTRAINT [FK_TypeDesign_VariableTypeDesign] FOREIGN KEY ([NodeIndex]) REFERENCES [dbo].[VariableTypeDesign] ([NodeIndex])
);

