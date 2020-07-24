IF OBJECT_ID(N'[dbo].[Person]') IS NULL
BEGIN
	CREATE TABLE [dbo].[Person](
		[Id] [INT] IDENTITY(1,1) NOT NULL,
		[FirstName] [NVARCHAR](20) NOT NULL,
		[LastName] [NVARCHAR](20) NOT NULL,
		[DateOfBirth] [DATETIME] NOT NULL,
	PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]
END
GO