USE DeviceManagementDB;
GO

IF NOT EXISTS (SELECT * FROM Users WHERE Email = 'john@example.com')
BEGIN
    INSERT INTO Users (Name, Email, Password, Role, Location) 
    VALUES ('John Doe', 'john@example.com', 'Pass123!', 'Software Engineer', 'London');
END

IF NOT EXISTS (SELECT * FROM Users WHERE Email = 'jane@example.com')
BEGIN
    INSERT INTO Users (Name, Email, Password, Role, Location) 
    VALUES ('Jane Smith', 'jane@example.com', 'Pass123!', 'Product Manager', 'New York');
END

/*SEED DEVICES*/

IF NOT EXISTS (SELECT * FROM Devices WHERE Name = 'iPhone 15 Pro')
BEGIN
    INSERT INTO Devices (Name, Manufacturer, Type, OS, OSVersion, Processor, RAMAmount, Description, AssignedUserId)
    VALUES ('iPhone 15 Pro', 'Apple', 'phone', 'iOS', '17.0', 'A17 Pro', '8GB', 'Company flagship phone.', 
            (SELECT TOP 1 Id FROM Users WHERE Email = 'john@example.com')); 
END

IF NOT EXISTS (SELECT * FROM Devices WHERE Name = 'Galaxy Tab S9')
BEGIN
    INSERT INTO Devices (Name, Manufacturer, Type, OS, OSVersion, Processor, RAMAmount, Description, AssignedUserId)
    VALUES ('Galaxy Tab S9', 'Samsung', 'tablet', 'Android', '14', 'Snapdragon 8 Gen 2', '12GB', 'High-end tablet for design.', NULL);
END