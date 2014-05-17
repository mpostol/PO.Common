CREATE TABLE [dbo].[NodeDesign] (
    [BrowseName]       NCHAR (10)       NOT NULL,
    [DisplayName]      NCHAR (10)       NULL,
    [DisplayNameKey]   NCHAR (10)       NULL,
    [Description]      NCHAR (10)       NULL,
    [DescriptionKey]   NCHAR (10)       NULL,
    [NodeDesignIndex]  UNIQUEIDENTIFIER NOT NULL,
    [SymbolicName]     NCHAR (10)       NULL,
    [SymbolicNameNS]   NCHAR (10)       NULL,
    [SymbolicId]       NCHAR (10)       NULL,
    [SymbolicIdNS]     NCHAR (10)       NULL,
    [IsDeclaration]    BIT              NULL,
    [NumericId]        INT              NULL,
    [WriteAccess]      INT              NULL,
    [ModelDesignIndex] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_NodeDesign] PRIMARY KEY CLUSTERED ([NodeDesignIndex] ASC),
    CONSTRAINT [FK_NodeDesign_InstanceDesign] FOREIGN KEY ([NodeDesignIndex]) REFERENCES [dbo].[InstanceDesign] ([NodeIndex]),
    CONSTRAINT [FK_NodeDesign_ListOfChildren] FOREIGN KEY ([NodeDesignIndex]) REFERENCES [dbo].[ListOfChildren] ([NodeDesignIndex]),
    CONSTRAINT [FK_NodeDesign_ModelDesign] FOREIGN KEY ([ModelDesignIndex]) REFERENCES [dbo].[ModelDesign] ([ModelDesignIndex]),
    CONSTRAINT [FK_NodeDesign_TypeDesign] FOREIGN KEY ([NodeDesignIndex]) REFERENCES [dbo].[TypeDesign] ([NodeIndex])
);

