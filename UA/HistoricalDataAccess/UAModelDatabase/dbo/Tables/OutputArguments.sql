CREATE TABLE [dbo].[OutputArguments] (
    [ParameterIndex]    UNIQUEIDENTIFIER NOT NULL,
    [MethodDesignIndex] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_OutputArguments] PRIMARY KEY CLUSTERED ([ParameterIndex] ASC),
    CONSTRAINT [FK_OutputArguments_MethodDesign] FOREIGN KEY ([MethodDesignIndex]) REFERENCES [dbo].[MethodDesign] ([NodeIndex]),
    CONSTRAINT [FK_OutputArguments_Parameter] FOREIGN KEY ([ParameterIndex]) REFERENCES [dbo].[Parameter] ([ParameterIndex])
);

