CREATE TABLE [dbo].[Namespace] (
    [ModelDesignIndex] UNIQUEIDENTIFIER NULL,
    [Name]             NCHAR (10)       NULL,
    [Prefix]           NCHAR (10)       NULL,
    [InternalPrefix]   NCHAR (10)       NULL,
    [XmlNamespace]     NCHAR (10)       NULL,
    [XmlPrefix]        NCHAR (10)       NULL,
    [FilePath]         NCHAR (10)       NULL,
    CONSTRAINT [FK_Namespace_ModelDesign] FOREIGN KEY ([ModelDesignIndex]) REFERENCES [dbo].[ModelDesign] ([ModelDesignIndex])
);

