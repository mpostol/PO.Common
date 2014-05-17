CREATE TABLE [dbo].[ReferenceTypeDesign] (
    [ReferenceTypeDesignIndex] UNIQUEIDENTIFIER NOT NULL,
    [InverseName]              NCHAR (10)       NULL,
    [InverseNameKey]           NCHAR (10)       NULL,
    [Symmetric]                BIT              NULL,
    CONSTRAINT [PK_ReferenceTypeDesign] PRIMARY KEY CLUSTERED ([ReferenceTypeDesignIndex] ASC)
);

