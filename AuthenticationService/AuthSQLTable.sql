USE [DTAuthentication]
GO

/****** Object:  Table [dbo].[AuthenticationUser]    Script Date: 30/07/2023 13:23:25 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AuthenticationUser](
	[username] [nvarchar](50) NOT NULL,
	[password] [varbinary](32) NOT NULL,
	[salt] [nvarchar](8) NOT NULL,
	[temp_token] [uniqueidentifier] NULL,
	[temp_token_date] [datetime2](7) NULL,
 CONSTRAINT [PK_AuthenticationUser] PRIMARY KEY CLUSTERED 
(
	[username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


