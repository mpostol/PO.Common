CREATE TABLE [dbo].[InputArguments] (
    [ParameterIndex]    UNIQUEIDENTIFIER NOT NULL,
    [MethodDesignIndex] UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_InputArguments] PRIMARY KEY CLUSTERED ([ParameterIndex] ASC),
    CONSTRAINT [FK_InputArguments_MethodDesign] FOREIGN KEY ([MethodDesignIndex]) REFERENCES [dbo].[MethodDesign] ([NodeIndex])
);

