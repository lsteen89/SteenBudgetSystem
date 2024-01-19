IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'SteenBudgetSystem')
BEGIN
CREATE DATABASE SteenBudgetSystem
END
GO
    USE SteenBudgetSystem
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' and xtype='U')
BEGIN
    CREATE TABLE Users (
        Id INT IDENTITY (1, 1) PRIMARY KEY,
        Persoid CHAR(36) NOT NULL UNIQUE,
        Firstname VARCHAR(50) NOT NULL,
        LastName VARCHAR(100) NOT NULL,
        Email VARCHAR(100) NOT NULL UNIQUE,
        EmailConfirmed INT,
        Password NVARCHAR(100) NOT NULL,
        PasswordSalt NVARCHAR(100), -- Added this column for storing password salt
        CreatedBy VARCHAR(50) NOT NULL,
        CreatedTime DATETIME NOT NULL,
        LastUpdatedTime DATETIME
    )
END

--insert into Users values('1', 'Linus', 'Steen', 'njur@steen.se','1', 'hemligt', '1', 'admin', getdate(), getdate())


