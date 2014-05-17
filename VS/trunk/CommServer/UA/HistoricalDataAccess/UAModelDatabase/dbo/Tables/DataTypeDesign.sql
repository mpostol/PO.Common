CREATE TABLE [dbo].[DataTypeDesign] (
    [DataTypeDesignIndex] UNIQUEIDENTIFIER NOT NULL,
    [NoArraysAllowed]     BIT              NULL,
    [NotInAddressSpace]   BIT              NULL,
    CONSTRAINT [PK_DataTypeDesign] PRIMARY KEY CLUSTERED ([DataTypeDesignIndex] ASC),
    CONSTRAINT [FK_DataTypeDesign_TypeDesign] FOREIGN KEY ([DataTypeDesignIndex]) REFERENCES [dbo].[TypeDesign] ([NodeIndex])
);

