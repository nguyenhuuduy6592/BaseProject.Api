CREATE TABLE [dbo].[DeploymentMetadata] (
    [Code]          VARCHAR (128)  NOT NULL,
    [CreatedDate]   DATETIME       NOT NULL,
    [By]            NVARCHAR (128) DEFAULT (original_login()) NOT NULL,
    [As]            NVARCHAR (128) DEFAULT (suser_sname()) NOT NULL,
    [CompletedDate] DATETIME       DEFAULT (getdate()) NOT NULL,
    [With]          NVARCHAR (128) DEFAULT (app_name()) NOT NULL,
    PRIMARY KEY CLUSTERED ([Code] ASC)
);

