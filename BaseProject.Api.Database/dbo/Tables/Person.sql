CREATE TABLE [dbo].[Person] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [FirstName]   NVARCHAR (20) NOT NULL,
    [LastName]    NVARCHAR (20) NOT NULL,
    [DateOfBirth] DATETIME      NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

