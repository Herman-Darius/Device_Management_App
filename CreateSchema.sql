IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
BEGIN
    CREATE TABLE Users (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Name NVARCHAR(100) NOT NULL,
        Role NVARCHAR(100) NOT NULL,
        Location NVARCHAR(100) NOT NULL
    );
END

IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Devices')
BEGIN
    CREATE TABLE Devices (
        Id INT PRIMARY KEY IDENTITY(1,1),
        Name NVARCHAR(100) NOT NULL,
        Manufacturer NVARCHAR(100) NOT NULL,
        Type NVARCHAR(50) NOT NULL,
        OS NVARCHAR(50) NOT NULL,
        OSVersion NVARCHAR(50) NOT NULL,
        Processor NVARCHAR(100) NOT NULL,
        RAMAmount NVARCHAR(50) NOT NULL,
        Description NVARCHAR(MAX) NULL,
        AssignedUserId INT NULL,
        CONSTRAINT FK_Devices_Users FOREIGN KEY (AssignedUserId) 
            REFERENCES Users(Id) ON DELETE SET NULL
    );
END