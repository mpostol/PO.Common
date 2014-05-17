CREATE TABLE [dbo].[MethodDesign] (
    [NodeIndex]     UNIQUEIDENTIFIER NOT NULL,
    [NonExecutable] BIT              NULL,
    CONSTRAINT [PK_MethodDesign] PRIMARY KEY CLUSTERED ([NodeIndex] ASC)
);

