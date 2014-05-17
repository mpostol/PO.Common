CREATE TABLE [dbo].[ModelDesign] (
    [ModelDesignIndex]       UNIQUEIDENTIFIER NOT NULL,
    [InformationsModelIndex] UNIQUEIDENTIFIER NOT NULL,
    [TargetNamespace]        NCHAR (10)       NOT NULL,
    [TargetXmlNamespace]     NCHAR (10)       NULL,
    [DefaultLocale]          NCHAR (10)       NULL,
    CONSTRAINT [PK_ModelDesign] PRIMARY KEY CLUSTERED ([ModelDesignIndex] ASC),
    CONSTRAINT [FK_ModelDesign_InformationsModels] FOREIGN KEY ([InformationsModelIndex]) REFERENCES [dbo].[InformationsModel] ([InformationsModelIndex])
);

