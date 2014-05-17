CREATE TABLE [dbo].[References] (
    [ReferenceIndex] UNIQUEIDENTIFIER NOT NULL,
    [Parent]         UNIQUEIDENTIFIER NOT NULL,
    [Target]         UNIQUEIDENTIFIER NOT NULL,
    [Type]           UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_References] PRIMARY KEY CLUSTERED ([ReferenceIndex] ASC),
    CONSTRAINT [FK_References_NodeDesign_Parent] FOREIGN KEY ([Parent]) REFERENCES [dbo].[NodeDesign] ([NodeDesignIndex]),
    CONSTRAINT [FK_References_NodeDesign_Target] FOREIGN KEY ([Target]) REFERENCES [dbo].[NodeDesign] ([NodeDesignIndex]),
    CONSTRAINT [FK_References_NodeDesign_Type] FOREIGN KEY ([Type]) REFERENCES [dbo].[NodeDesign] ([NodeDesignIndex])
);

