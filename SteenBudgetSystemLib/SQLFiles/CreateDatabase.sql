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
        PasswordSalt NVARCHAR(100),
        roles varchar(20) not null,
        FirstLogin BIT DEFAULT 1,
        CreatedBy VARCHAR(50) NOT NULL,
        CreatedTime DATETIME NOT NULL,
        LastUpdatedTime DATETIME
    )
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Income' and xtype='U')
BEGIN
    CREATE TABLE Income (
        Id INT IDENTITY (1, 1) PRIMARY KEY,
        PersoId CHAR(36),
        MainIncome DECIMAL(10, 2),
        SideIncome DECIMAL(10, 2),
        FOREIGN KEY (PersoId) REFERENCES Users(PersoId)
    )
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Partner' and xtype='U')
BEGIN
    CREATE TABLE Partner (
        Id INT IDENTITY (1, 1) PRIMARY KEY,
        PersoId CHAR(36),
        Name VARCHAR(255),
        
    )
END

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='PartnerIncome' and xtype='U')
BEGIN
    CREATE TABLE PartnerIncome (
        Id INT IDENTITY (1, 1) PRIMARY KEY,
        PartnerId INT,
        MainIncome DECIMAL(10, 2),
        SideIncome DECIMAL(10, 2),
        FOREIGN KEY (PartnerId) REFERENCES Partner(Id)
    );
END



--insert into Users values('1', 'Linus', 'Steen', 'njur@steen.se','1', 'hemligt', 'salt','1', '1', 'admin', getdate(), getdate())


