IF OBJECT_ID(N'[dbo].[Person]') IS NOT NULL
BEGIN
	ALTER TABLE [dbo].[Person] ADD CreatedById INT NOT NULL;
	ALTER TABLE [dbo].[Person] ADD ModifiedById INT NOT NULL;
	ALTER TABLE [dbo].[Person] ADD CreatedDateTimeUtc DATETIME NOT NULL;
	ALTER TABLE [dbo].[Person] ADD ModifiedDateTimeUtc DATETIME NOT NULL;
END
GO