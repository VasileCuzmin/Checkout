IF NOT EXISTS(SELECT 1 FROM sys.tables WHERE [name] = '__CheckoutMigration') 
	CREATE TABLE dbo.[__CheckoutMigration] (
		ScriptsVersion [int] NOT NULL
	)

INSERT INTO dbo.[__CheckoutMigration] (ScriptsVersion) 
	SELECT 0 
	WHERE NOT EXISTS(SELECT 1 FROM dbo.[__CheckoutMigration]) 