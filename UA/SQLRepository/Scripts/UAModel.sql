USE [UAModel]
GO
/****** Object:  Table [dbo].[InformationsModel]    Script Date: 05/16/2014 18:13:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InformationsModel](
	[InformationsModelIndex] [uniqueidentifier] NOT NULL,
	[Name] [nchar](10) NOT NULL,
	[NameNS] [nchar](10) NOT NULL,
 CONSTRAINT [PK_InformationsModels] PRIMARY KEY CLUSTERED 
(
	[InformationsModelIndex] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReferenceTypeDesign]    Script Date: 05/16/2014 18:13:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReferenceTypeDesign](
	[ReferenceTypeDesignIndex] [uniqueidentifier] NOT NULL,
	[InverseName] [nchar](10) NULL,
	[InverseNameKey] [nchar](10) NULL,
	[Symmetric] [bit] NULL,
 CONSTRAINT [PK_ReferenceTypeDesign] PRIMARY KEY CLUSTERED 
(
	[ReferenceTypeDesignIndex] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DefaultValue]    Script Date: 05/16/2014 18:13:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DefaultValue](
	[DefaultValueIndex] [uniqueidentifier] NOT NULL,
	[Value] [sql_variant] NULL,
 CONSTRAINT [PK_DefaultValue] PRIMARY KEY CLUSTERED 
(
	[DefaultValueIndex] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MethodDesign]    Script Date: 05/16/2014 18:13:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MethodDesign](
	[NodeIndex] [uniqueidentifier] NOT NULL,
	[NonExecutable] [bit] NULL,
 CONSTRAINT [PK_MethodDesign] PRIMARY KEY CLUSTERED 
(
	[NodeIndex] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DataTypeDesign]    Script Date: 05/16/2014 18:13:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DataTypeDesign](
	[DataTypeDesignIndex] [uniqueidentifier] NOT NULL,
	[NoArraysAllowed] [bit] NULL,
	[NotInAddressSpace] [bit] NULL,
 CONSTRAINT [PK_DataTypeDesign] PRIMARY KEY CLUSTERED 
(
	[DataTypeDesignIndex] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InputArguments]    Script Date: 05/16/2014 18:13:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InputArguments](
	[ParameterIndex] [uniqueidentifier] NOT NULL,
	[MethodDesignIndex] [uniqueidentifier] NULL,
 CONSTRAINT [PK_InputArguments] PRIMARY KEY CLUSTERED 
(
	[ParameterIndex] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ListOfFields]    Script Date: 05/16/2014 18:13:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ListOfFields](
	[DataTypeDesignIndex] [uniqueidentifier] NULL,
	[ParameterIndex] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ListOfFields] PRIMARY KEY CLUSTERED 
(
	[ParameterIndex] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Parameter]    Script Date: 05/16/2014 18:13:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Parameter](
	[ParameterIndex] [uniqueidentifier] NOT NULL,
	[Description] [nchar](10) NULL,
	[DescriptionKey] [nchar](10) NULL,
	[Name] [nchar](10) NULL,
	[Identifier] [int] NULL,
	[DataTypeDesignIndex] [uniqueidentifier] NOT NULL,
	[ValueRank] [int] NULL,
 CONSTRAINT [PK_Parameter] PRIMARY KEY CLUSTERED 
(
	[ParameterIndex] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OutputArguments]    Script Date: 05/16/2014 18:13:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OutputArguments](
	[ParameterIndex] [uniqueidentifier] NOT NULL,
	[MethodDesignIndex] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_OutputArguments] PRIMARY KEY CLUSTERED 
(
	[ParameterIndex] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ModelDesign]    Script Date: 05/16/2014 18:13:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ModelDesign](
	[ModelDesignIndex] [uniqueidentifier] NOT NULL,
	[InformationsModelIndex] [uniqueidentifier] NOT NULL,
	[TargetNamespace] [nchar](10) NOT NULL,
	[TargetXmlNamespace] [nchar](10) NULL,
	[DefaultLocale] [nchar](10) NULL,
 CONSTRAINT [PK_ModelDesign] PRIMARY KEY CLUSTERED 
(
	[ModelDesignIndex] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VariableDesign]    Script Date: 05/16/2014 18:13:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VariableDesign](
	[NodeIndex] [uniqueidentifier] NOT NULL,
	[DefaultValueIndex] [uniqueidentifier] NULL,
	[DataTypeDesignIndex] [uniqueidentifier] NULL,
	[ValueRank] [int] NULL,
	[ArrayDimensions] [char](10) NULL,
	[AccessLevel] [int] NULL,
	[MinimumSamplingInterval] [int] NULL,
	[Historizing] [bit] NULL,
	[IsProperty] [bit] NOT NULL,
 CONSTRAINT [PK_VariableDesign] PRIMARY KEY CLUSTERED 
(
	[NodeIndex] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[InstanceDesign]    Script Date: 05/16/2014 18:13:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InstanceDesign](
	[NodeIndex] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_InstanceDesign] PRIMARY KEY CLUSTERED 
(
	[NodeIndex] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VariableTypeDesign]    Script Date: 05/16/2014 18:13:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VariableTypeDesign](
	[NodeIndex] [uniqueidentifier] NOT NULL,
	[DefaultValue] [uniqueidentifier] NULL,
	[DataTypeDesignIndex] [uniqueidentifier] NULL,
	[ValueRank] [int] NULL,
	[ArrayDimensions] [nchar](10) NULL,
	[AccessLevel] [int] NULL,
	[MinimumSamplingInterval] [int] NULL,
	[Historizing] [bit] NULL,
	[ExposesItsChildren] [bit] NULL,
 CONSTRAINT [PK_VariableTypeDesign] PRIMARY KEY CLUSTERED 
(
	[NodeIndex] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TypeDesign]    Script Date: 05/16/2014 18:13:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TypeDesign](
	[NodeIndex] [uniqueidentifier] NOT NULL,
	[ClassName] [sql_variant] NULL,
	[BaseType] [uniqueidentifier] NULL,
 CONSTRAINT [PK_TypeDesign] PRIMARY KEY CLUSTERED 
(
	[NodeIndex] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[NodeDesign]    Script Date: 05/16/2014 18:13:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NodeDesign](
	[BrowseName] [nchar](10) NOT NULL,
	[DisplayName] [nchar](10) NULL,
	[DisplayNameKey] [nchar](10) NULL,
	[Description] [nchar](10) NULL,
	[DescriptionKey] [nchar](10) NULL,
	[NodeDesignIndex] [uniqueidentifier] NOT NULL,
	[SymbolicName] [nchar](10) NULL,
	[SymbolicNameNS] [nchar](10) NULL,
	[SymbolicId] [nchar](10) NULL,
	[SymbolicIdNS] [nchar](10) NULL,
	[IsDeclaration] [bit] NULL,
	[NumericId] [int] NULL,
	[WriteAccess] [int] NULL,
	[ModelDesignIndex] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_NodeDesign] PRIMARY KEY CLUSTERED 
(
	[NodeDesignIndex] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TopLevelObjects]    Script Date: 05/16/2014 18:13:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TopLevelObjects](
	[InformationsModelIndex] [uniqueidentifier] NOT NULL,
	[NodeDesignIndex] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_TopLevelObjects] PRIMARY KEY CLUSTERED 
(
	[NodeDesignIndex] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ListOfChildren]    Script Date: 05/16/2014 18:13:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ListOfChildren](
	[NodeDesignIndex] [uniqueidentifier] NOT NULL,
	[Parent] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_ListOfChildren] PRIMARY KEY CLUSTERED 
(
	[NodeDesignIndex] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[References]    Script Date: 05/16/2014 18:13:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[References](
	[ReferenceIndex] [uniqueidentifier] NOT NULL,
	[Parent] [uniqueidentifier] NOT NULL,
	[Target] [uniqueidentifier] NOT NULL,
	[Type] [uniqueidentifier] NULL,
 CONSTRAINT [PK_References] PRIMARY KEY CLUSTERED 
(
	[ReferenceIndex] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Namespace]    Script Date: 05/16/2014 18:13:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Namespace](
	[ModelDesignIndex] [uniqueidentifier] NULL,
	[Name] [nchar](10) NULL,
	[Prefix] [nchar](10) NULL,
	[InternalPrefix] [nchar](10) NULL,
	[XmlNamespace] [nchar](10) NULL,
	[XmlPrefix] [nchar](10) NULL,
	[FilePath] [nchar](10) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ObjectDesign]    Script Date: 05/16/2014 18:13:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ObjectDesign](
	[NodeIndex] [uniqueidentifier] NOT NULL,
	[SupportsEvents] [bit] NULL,
 CONSTRAINT [PK_ObjectDesign] PRIMARY KEY CLUSTERED 
(
	[NodeIndex] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ObjectTypeDesign]    Script Date: 05/16/2014 18:13:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ObjectTypeDesign](
	[NodeIndex] [uniqueidentifier] NOT NULL,
	[SupportsEvents] [bit] NULL,
 CONSTRAINT [PK_ObjectTypeDesign] PRIMARY KEY CLUSTERED 
(
	[NodeIndex] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ListOfEncodings]    Script Date: 05/16/2014 18:13:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ListOfEncodings](
	[DataTypeDesignIndex] [uniqueidentifier] NOT NULL,
	[NodeIndex] [uniqueidentifier] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PropertyValue]    Script Date: 05/16/2014 18:13:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PropertyValue](
	[NodeIndex] [uniqueidentifier] NOT NULL,
	[Value] [sql_variant] NULL,
	[TimeStamp] [timestamp] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VariableValue]    Script Date: 05/16/2014 18:13:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VariableValue](
	[NodeIndex] [uniqueidentifier] NOT NULL,
	[Value] [sql_variant] NULL,
	[TimeStamp] [timestamp] NULL,
	[Quality] [float] NULL
) ON [PRIMARY]
GO
/****** Object:  ForeignKey [FK_DataTypeDesign_TypeDesign]    Script Date: 05/16/2014 18:13:23 ******/
ALTER TABLE [dbo].[DataTypeDesign]  WITH CHECK ADD  CONSTRAINT [FK_DataTypeDesign_TypeDesign] FOREIGN KEY([DataTypeDesignIndex])
REFERENCES [dbo].[TypeDesign] ([NodeIndex])
GO
ALTER TABLE [dbo].[DataTypeDesign] CHECK CONSTRAINT [FK_DataTypeDesign_TypeDesign]
GO
/****** Object:  ForeignKey [FK_InputArguments_MethodDesign]    Script Date: 05/16/2014 18:13:23 ******/
ALTER TABLE [dbo].[InputArguments]  WITH CHECK ADD  CONSTRAINT [FK_InputArguments_MethodDesign] FOREIGN KEY([MethodDesignIndex])
REFERENCES [dbo].[MethodDesign] ([NodeIndex])
GO
ALTER TABLE [dbo].[InputArguments] CHECK CONSTRAINT [FK_InputArguments_MethodDesign]
GO
/****** Object:  ForeignKey [FK_InstanceDesign_MethodDesign]    Script Date: 05/16/2014 18:13:23 ******/
ALTER TABLE [dbo].[InstanceDesign]  WITH CHECK ADD  CONSTRAINT [FK_InstanceDesign_MethodDesign] FOREIGN KEY([NodeIndex])
REFERENCES [dbo].[MethodDesign] ([NodeIndex])
GO
ALTER TABLE [dbo].[InstanceDesign] CHECK CONSTRAINT [FK_InstanceDesign_MethodDesign]
GO
/****** Object:  ForeignKey [FK_InstanceDesign_VariableDesign]    Script Date: 05/16/2014 18:13:23 ******/
ALTER TABLE [dbo].[InstanceDesign]  WITH CHECK ADD  CONSTRAINT [FK_InstanceDesign_VariableDesign] FOREIGN KEY([NodeIndex])
REFERENCES [dbo].[VariableDesign] ([NodeIndex])
GO
ALTER TABLE [dbo].[InstanceDesign] CHECK CONSTRAINT [FK_InstanceDesign_VariableDesign]
GO
/****** Object:  ForeignKey [FK_ListOfChildren_NodeDesign1]    Script Date: 05/16/2014 18:13:23 ******/
ALTER TABLE [dbo].[ListOfChildren]  WITH CHECK ADD  CONSTRAINT [FK_ListOfChildren_NodeDesign1] FOREIGN KEY([Parent])
REFERENCES [dbo].[NodeDesign] ([NodeDesignIndex])
GO
ALTER TABLE [dbo].[ListOfChildren] CHECK CONSTRAINT [FK_ListOfChildren_NodeDesign1]
GO
/****** Object:  ForeignKey [FK_ListOfEncodings_DataTypeDesign]    Script Date: 05/16/2014 18:13:23 ******/
ALTER TABLE [dbo].[ListOfEncodings]  WITH CHECK ADD  CONSTRAINT [FK_ListOfEncodings_DataTypeDesign] FOREIGN KEY([DataTypeDesignIndex])
REFERENCES [dbo].[DataTypeDesign] ([DataTypeDesignIndex])
GO
ALTER TABLE [dbo].[ListOfEncodings] CHECK CONSTRAINT [FK_ListOfEncodings_DataTypeDesign]
GO
/****** Object:  ForeignKey [FK_ListOfEncodings_ObjectDesign]    Script Date: 05/16/2014 18:13:23 ******/
ALTER TABLE [dbo].[ListOfEncodings]  WITH CHECK ADD  CONSTRAINT [FK_ListOfEncodings_ObjectDesign] FOREIGN KEY([NodeIndex])
REFERENCES [dbo].[ObjectDesign] ([NodeIndex])
GO
ALTER TABLE [dbo].[ListOfEncodings] CHECK CONSTRAINT [FK_ListOfEncodings_ObjectDesign]
GO
/****** Object:  ForeignKey [FK_ListOfFields_DataTypeDesign]    Script Date: 05/16/2014 18:13:23 ******/
ALTER TABLE [dbo].[ListOfFields]  WITH CHECK ADD  CONSTRAINT [FK_ListOfFields_DataTypeDesign] FOREIGN KEY([DataTypeDesignIndex])
REFERENCES [dbo].[DataTypeDesign] ([DataTypeDesignIndex])
GO
ALTER TABLE [dbo].[ListOfFields] CHECK CONSTRAINT [FK_ListOfFields_DataTypeDesign]
GO
/****** Object:  ForeignKey [FK_ModelDesign_InformationsModels]    Script Date: 05/16/2014 18:13:23 ******/
ALTER TABLE [dbo].[ModelDesign]  WITH CHECK ADD  CONSTRAINT [FK_ModelDesign_InformationsModels] FOREIGN KEY([InformationsModelIndex])
REFERENCES [dbo].[InformationsModel] ([InformationsModelIndex])
GO
ALTER TABLE [dbo].[ModelDesign] CHECK CONSTRAINT [FK_ModelDesign_InformationsModels]
GO
/****** Object:  ForeignKey [FK_Namespace_ModelDesign]    Script Date: 05/16/2014 18:13:23 ******/
ALTER TABLE [dbo].[Namespace]  WITH CHECK ADD  CONSTRAINT [FK_Namespace_ModelDesign] FOREIGN KEY([ModelDesignIndex])
REFERENCES [dbo].[ModelDesign] ([ModelDesignIndex])
GO
ALTER TABLE [dbo].[Namespace] CHECK CONSTRAINT [FK_Namespace_ModelDesign]
GO
/****** Object:  ForeignKey [FK_NodeDesign_InstanceDesign]    Script Date: 05/16/2014 18:13:23 ******/
ALTER TABLE [dbo].[NodeDesign]  WITH CHECK ADD  CONSTRAINT [FK_NodeDesign_InstanceDesign] FOREIGN KEY([NodeDesignIndex])
REFERENCES [dbo].[InstanceDesign] ([NodeIndex])
GO
ALTER TABLE [dbo].[NodeDesign] CHECK CONSTRAINT [FK_NodeDesign_InstanceDesign]
GO
/****** Object:  ForeignKey [FK_NodeDesign_ListOfChildren]    Script Date: 05/16/2014 18:13:23 ******/
ALTER TABLE [dbo].[NodeDesign]  WITH CHECK ADD  CONSTRAINT [FK_NodeDesign_ListOfChildren] FOREIGN KEY([NodeDesignIndex])
REFERENCES [dbo].[ListOfChildren] ([NodeDesignIndex])
GO
ALTER TABLE [dbo].[NodeDesign] CHECK CONSTRAINT [FK_NodeDesign_ListOfChildren]
GO
/****** Object:  ForeignKey [FK_NodeDesign_ModelDesign]    Script Date: 05/16/2014 18:13:23 ******/
ALTER TABLE [dbo].[NodeDesign]  WITH CHECK ADD  CONSTRAINT [FK_NodeDesign_ModelDesign] FOREIGN KEY([ModelDesignIndex])
REFERENCES [dbo].[ModelDesign] ([ModelDesignIndex])
GO
ALTER TABLE [dbo].[NodeDesign] CHECK CONSTRAINT [FK_NodeDesign_ModelDesign]
GO
/****** Object:  ForeignKey [FK_NodeDesign_TypeDesign]    Script Date: 05/16/2014 18:13:23 ******/
ALTER TABLE [dbo].[NodeDesign]  WITH CHECK ADD  CONSTRAINT [FK_NodeDesign_TypeDesign] FOREIGN KEY([NodeDesignIndex])
REFERENCES [dbo].[TypeDesign] ([NodeIndex])
GO
ALTER TABLE [dbo].[NodeDesign] CHECK CONSTRAINT [FK_NodeDesign_TypeDesign]
GO
/****** Object:  ForeignKey [FK_ObjectDesign_InstanceDesign]    Script Date: 05/16/2014 18:13:23 ******/
ALTER TABLE [dbo].[ObjectDesign]  WITH CHECK ADD  CONSTRAINT [FK_ObjectDesign_InstanceDesign] FOREIGN KEY([NodeIndex])
REFERENCES [dbo].[InstanceDesign] ([NodeIndex])
GO
ALTER TABLE [dbo].[ObjectDesign] CHECK CONSTRAINT [FK_ObjectDesign_InstanceDesign]
GO
/****** Object:  ForeignKey [FK_ObjectTypeDesign_TypeDesign]    Script Date: 05/16/2014 18:13:23 ******/
ALTER TABLE [dbo].[ObjectTypeDesign]  WITH CHECK ADD  CONSTRAINT [FK_ObjectTypeDesign_TypeDesign] FOREIGN KEY([NodeIndex])
REFERENCES [dbo].[TypeDesign] ([NodeIndex])
GO
ALTER TABLE [dbo].[ObjectTypeDesign] CHECK CONSTRAINT [FK_ObjectTypeDesign_TypeDesign]
GO
/****** Object:  ForeignKey [FK_OutputArguments_MethodDesign]    Script Date: 05/16/2014 18:13:23 ******/
ALTER TABLE [dbo].[OutputArguments]  WITH CHECK ADD  CONSTRAINT [FK_OutputArguments_MethodDesign] FOREIGN KEY([MethodDesignIndex])
REFERENCES [dbo].[MethodDesign] ([NodeIndex])
GO
ALTER TABLE [dbo].[OutputArguments] CHECK CONSTRAINT [FK_OutputArguments_MethodDesign]
GO
/****** Object:  ForeignKey [FK_OutputArguments_Parameter]    Script Date: 05/16/2014 18:13:23 ******/
ALTER TABLE [dbo].[OutputArguments]  WITH CHECK ADD  CONSTRAINT [FK_OutputArguments_Parameter] FOREIGN KEY([ParameterIndex])
REFERENCES [dbo].[Parameter] ([ParameterIndex])
GO
ALTER TABLE [dbo].[OutputArguments] CHECK CONSTRAINT [FK_OutputArguments_Parameter]
GO
/****** Object:  ForeignKey [FK_Parameter_DataTypeDesign]    Script Date: 05/16/2014 18:13:24 ******/
ALTER TABLE [dbo].[Parameter]  WITH CHECK ADD  CONSTRAINT [FK_Parameter_DataTypeDesign] FOREIGN KEY([DataTypeDesignIndex])
REFERENCES [dbo].[DataTypeDesign] ([DataTypeDesignIndex])
GO
ALTER TABLE [dbo].[Parameter] CHECK CONSTRAINT [FK_Parameter_DataTypeDesign]
GO
/****** Object:  ForeignKey [FK_Parameter_InputArguments]    Script Date: 05/16/2014 18:13:24 ******/
ALTER TABLE [dbo].[Parameter]  WITH CHECK ADD  CONSTRAINT [FK_Parameter_InputArguments] FOREIGN KEY([ParameterIndex])
REFERENCES [dbo].[InputArguments] ([ParameterIndex])
GO
ALTER TABLE [dbo].[Parameter] CHECK CONSTRAINT [FK_Parameter_InputArguments]
GO
/****** Object:  ForeignKey [FK_Parameter_ListOfFields]    Script Date: 05/16/2014 18:13:24 ******/
ALTER TABLE [dbo].[Parameter]  WITH CHECK ADD  CONSTRAINT [FK_Parameter_ListOfFields] FOREIGN KEY([ParameterIndex])
REFERENCES [dbo].[ListOfFields] ([ParameterIndex])
GO
ALTER TABLE [dbo].[Parameter] CHECK CONSTRAINT [FK_Parameter_ListOfFields]
GO
/****** Object:  ForeignKey [FK_PropertyValue_VariableDesign]    Script Date: 05/16/2014 18:13:24 ******/
ALTER TABLE [dbo].[PropertyValue]  WITH CHECK ADD  CONSTRAINT [FK_PropertyValue_VariableDesign] FOREIGN KEY([NodeIndex])
REFERENCES [dbo].[VariableDesign] ([NodeIndex])
GO
ALTER TABLE [dbo].[PropertyValue] CHECK CONSTRAINT [FK_PropertyValue_VariableDesign]
GO
/****** Object:  ForeignKey [FK_References_NodeDesign_Parent]    Script Date: 05/16/2014 18:13:24 ******/
ALTER TABLE [dbo].[References]  WITH CHECK ADD  CONSTRAINT [FK_References_NodeDesign_Parent] FOREIGN KEY([Parent])
REFERENCES [dbo].[NodeDesign] ([NodeDesignIndex])
GO
ALTER TABLE [dbo].[References] CHECK CONSTRAINT [FK_References_NodeDesign_Parent]
GO
/****** Object:  ForeignKey [FK_References_NodeDesign_Target]    Script Date: 05/16/2014 18:13:24 ******/
ALTER TABLE [dbo].[References]  WITH CHECK ADD  CONSTRAINT [FK_References_NodeDesign_Target] FOREIGN KEY([Target])
REFERENCES [dbo].[NodeDesign] ([NodeDesignIndex])
GO
ALTER TABLE [dbo].[References] CHECK CONSTRAINT [FK_References_NodeDesign_Target]
GO
/****** Object:  ForeignKey [FK_References_NodeDesign_Type]    Script Date: 05/16/2014 18:13:24 ******/
ALTER TABLE [dbo].[References]  WITH CHECK ADD  CONSTRAINT [FK_References_NodeDesign_Type] FOREIGN KEY([Type])
REFERENCES [dbo].[NodeDesign] ([NodeDesignIndex])
GO
ALTER TABLE [dbo].[References] CHECK CONSTRAINT [FK_References_NodeDesign_Type]
GO
/****** Object:  ForeignKey [FK_TopLevelObjects_InformationsModel]    Script Date: 05/16/2014 18:13:24 ******/
ALTER TABLE [dbo].[TopLevelObjects]  WITH CHECK ADD  CONSTRAINT [FK_TopLevelObjects_InformationsModel] FOREIGN KEY([InformationsModelIndex])
REFERENCES [dbo].[InformationsModel] ([InformationsModelIndex])
GO
ALTER TABLE [dbo].[TopLevelObjects] CHECK CONSTRAINT [FK_TopLevelObjects_InformationsModel]
GO
/****** Object:  ForeignKey [FK_TopLevelObjects_NodeDesign]    Script Date: 05/16/2014 18:13:24 ******/
ALTER TABLE [dbo].[TopLevelObjects]  WITH CHECK ADD  CONSTRAINT [FK_TopLevelObjects_NodeDesign] FOREIGN KEY([NodeDesignIndex])
REFERENCES [dbo].[NodeDesign] ([NodeDesignIndex])
GO
ALTER TABLE [dbo].[TopLevelObjects] CHECK CONSTRAINT [FK_TopLevelObjects_NodeDesign]
GO
/****** Object:  ForeignKey [FK_TypeDesign_NodeDesign_BaseType]    Script Date: 05/16/2014 18:13:24 ******/
ALTER TABLE [dbo].[TypeDesign]  WITH CHECK ADD  CONSTRAINT [FK_TypeDesign_NodeDesign_BaseType] FOREIGN KEY([BaseType])
REFERENCES [dbo].[NodeDesign] ([NodeDesignIndex])
GO
ALTER TABLE [dbo].[TypeDesign] CHECK CONSTRAINT [FK_TypeDesign_NodeDesign_BaseType]
GO
/****** Object:  ForeignKey [FK_TypeDesign_ReferenceTypeDesign]    Script Date: 05/16/2014 18:13:24 ******/
ALTER TABLE [dbo].[TypeDesign]  WITH CHECK ADD  CONSTRAINT [FK_TypeDesign_ReferenceTypeDesign] FOREIGN KEY([NodeIndex])
REFERENCES [dbo].[ReferenceTypeDesign] ([ReferenceTypeDesignIndex])
GO
ALTER TABLE [dbo].[TypeDesign] CHECK CONSTRAINT [FK_TypeDesign_ReferenceTypeDesign]
GO
/****** Object:  ForeignKey [FK_TypeDesign_VariableTypeDesign]    Script Date: 05/16/2014 18:13:24 ******/
ALTER TABLE [dbo].[TypeDesign]  WITH CHECK ADD  CONSTRAINT [FK_TypeDesign_VariableTypeDesign] FOREIGN KEY([NodeIndex])
REFERENCES [dbo].[VariableTypeDesign] ([NodeIndex])
GO
ALTER TABLE [dbo].[TypeDesign] CHECK CONSTRAINT [FK_TypeDesign_VariableTypeDesign]
GO
/****** Object:  ForeignKey [FK_VariableDesign_DataTypeDesign]    Script Date: 05/16/2014 18:13:24 ******/
ALTER TABLE [dbo].[VariableDesign]  WITH CHECK ADD  CONSTRAINT [FK_VariableDesign_DataTypeDesign] FOREIGN KEY([DataTypeDesignIndex])
REFERENCES [dbo].[DataTypeDesign] ([DataTypeDesignIndex])
GO
ALTER TABLE [dbo].[VariableDesign] CHECK CONSTRAINT [FK_VariableDesign_DataTypeDesign]
GO
/****** Object:  ForeignKey [FK_VariableDesign_DefaultValue]    Script Date: 05/16/2014 18:13:24 ******/
ALTER TABLE [dbo].[VariableDesign]  WITH CHECK ADD  CONSTRAINT [FK_VariableDesign_DefaultValue] FOREIGN KEY([DefaultValueIndex])
REFERENCES [dbo].[DefaultValue] ([DefaultValueIndex])
GO
ALTER TABLE [dbo].[VariableDesign] CHECK CONSTRAINT [FK_VariableDesign_DefaultValue]
GO
/****** Object:  ForeignKey [FK_VariableTypeDesign_DataTypeDesign]    Script Date: 05/16/2014 18:13:24 ******/
ALTER TABLE [dbo].[VariableTypeDesign]  WITH CHECK ADD  CONSTRAINT [FK_VariableTypeDesign_DataTypeDesign] FOREIGN KEY([DataTypeDesignIndex])
REFERENCES [dbo].[DataTypeDesign] ([DataTypeDesignIndex])
GO
ALTER TABLE [dbo].[VariableTypeDesign] CHECK CONSTRAINT [FK_VariableTypeDesign_DataTypeDesign]
GO
/****** Object:  ForeignKey [FK_VariableValue_VariableDesign]    Script Date: 05/16/2014 18:13:24 ******/
ALTER TABLE [dbo].[VariableValue]  WITH CHECK ADD  CONSTRAINT [FK_VariableValue_VariableDesign] FOREIGN KEY([NodeIndex])
REFERENCES [dbo].[VariableDesign] ([NodeIndex])
GO
ALTER TABLE [dbo].[VariableValue] CHECK CONSTRAINT [FK_VariableValue_VariableDesign]
GO
