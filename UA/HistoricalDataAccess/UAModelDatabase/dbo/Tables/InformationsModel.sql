CREATE TABLE [dbo].[InformationsModel] (
    [InformationsModelIndex] UNIQUEIDENTIFIER NOT NULL,
    [Name]                   NCHAR (10)       NOT NULL,
    [NameNS]                 NCHAR (10)       NOT NULL,
    CONSTRAINT [PK_InformationsModels] PRIMARY KEY CLUSTERED ([InformationsModelIndex] ASC)
);

