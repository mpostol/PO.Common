CREATE TABLE [dbo].[DefaultValue] (
    [DefaultValueIndex] UNIQUEIDENTIFIER NOT NULL,
    [Value]             SQL_VARIANT      NULL,
    CONSTRAINT [PK_DefaultValue] PRIMARY KEY CLUSTERED ([DefaultValueIndex] ASC)
);

