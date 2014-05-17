CREATE TABLE [dbo].[ListOfChildren] (
    [NodeDesignIndex] UNIQUEIDENTIFIER NOT NULL,
    [Parent]          UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_ListOfChildren] PRIMARY KEY CLUSTERED ([NodeDesignIndex] ASC),
    CONSTRAINT [FK_ListOfChildren_NodeDesign1] FOREIGN KEY ([Parent]) REFERENCES [dbo].[NodeDesign] ([NodeDesignIndex])
);

