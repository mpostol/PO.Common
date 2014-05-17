CREATE TABLE [dbo].[TopLevelObjects] (
    [InformationsModelIndex] UNIQUEIDENTIFIER NOT NULL,
    [NodeDesignIndex]        UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_TopLevelObjects] PRIMARY KEY CLUSTERED ([NodeDesignIndex] ASC),
    CONSTRAINT [FK_TopLevelObjects_InformationsModel] FOREIGN KEY ([InformationsModelIndex]) REFERENCES [dbo].[InformationsModel] ([InformationsModelIndex]),
    CONSTRAINT [FK_TopLevelObjects_NodeDesign] FOREIGN KEY ([NodeDesignIndex]) REFERENCES [dbo].[NodeDesign] ([NodeDesignIndex])
);

