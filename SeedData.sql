USE DeviceManagementDB;
GO

/* SEED USERS */

IF NOT EXISTS (SELECT * FROM Users WHERE Email = 'darius@example.com')
BEGIN
    INSERT INTO Users (Name, Email, Password, Role, Location) 
    VALUES ('Administrator', 'admin@example.com', 'admin', 'Admin', 'Headquarters');
END

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

IF NOT EXISTS (SELECT * FROM Users WHERE Email = 'alice@example.com')
BEGIN
    INSERT INTO Users (Name, Email, Password, Role, Location) 
    VALUES ('Alice Johnson', 'alice@example.com', 'Pass123!', 'DevOps Lead', 'Berlin');
END

IF NOT EXISTS (SELECT * FROM Users WHERE Email = 'bob@example.com')
BEGIN
    INSERT INTO Users (Name, Email, Password, Role, Location) 
    VALUES ('Bob Wilson', 'bob@example.com', 'Pass123!', 'QA Engineer', 'Austin');
END

/* SEED DEVICES */
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

IF NOT EXISTS (SELECT * FROM Devices WHERE Name = 'MacBook Air M3')
BEGIN
    INSERT INTO Devices (Name, Manufacturer, Type, OS, OSVersion, Processor, RAMAmount, Description, AssignedUserId)
    VALUES ('MacBook Air M3', 'Apple', 'laptop', 'macOS', '14.0', 'M3', '16GB', 'Lightweight work laptop.', 
    (SELECT TOP 1 Id FROM Users WHERE Email = 'alice@example.com'));
END

IF NOT EXISTS (SELECT * FROM Devices WHERE Name = 'ThinkPad X1 Carbon')
BEGIN
    INSERT INTO Devices (Name, Manufacturer, Type, OS, OSVersion, Processor, RAMAmount, Description, AssignedUserId)
    VALUES ('ThinkPad X1 Carbon', 'Lenovo', 'laptop', 'Windows', '11', 'Core i7', '32GB', 'Business flagship.', NULL);
END

IF NOT EXISTS (SELECT * FROM Devices WHERE Name = 'Surface Pro 9')
BEGIN
    INSERT INTO Devices (Name, Manufacturer, Type, OS, OSVersion, Processor, RAMAmount, Description, AssignedUserId)
    VALUES ('Surface Pro 9', 'Microsoft', 'tablet', 'Windows', '11', 'Core i5', '8GB', '2-in-1 device.', 
    (SELECT TOP 1 Id FROM Users WHERE Email = 'bob@example.com'));
END

IF NOT EXISTS (SELECT * FROM Devices WHERE Name = 'Pixel 8 Pro')
BEGIN
    INSERT INTO Devices (Name, Manufacturer, Type, OS, OSVersion, Processor, RAMAmount, Description, AssignedUserId)
    VALUES ('Pixel 8 Pro', 'Google', 'phone', 'Android', '14', 'Tensor G3', '12GB', 'Testing phone.', NULL);
END

IF NOT EXISTS (SELECT * FROM Devices WHERE Name = 'Dell XPS 15')
BEGIN
    INSERT INTO Devices (Name, Manufacturer, Type, OS, OSVersion, Processor, RAMAmount, Description, AssignedUserId)
    VALUES ('Dell XPS 15', 'Dell', 'laptop', 'Windows', '11', 'Core i9', '32GB', 'High performance workstation.', 
    (SELECT TOP 1 Id FROM Users WHERE Email = 'jane@example.com'));
END

IF NOT EXISTS (SELECT * FROM Devices WHERE Name = 'iPad Pro 12.9')
BEGIN
    INSERT INTO Devices (Name, Manufacturer, Type, OS, OSVersion, Processor, RAMAmount, Description, AssignedUserId)
    VALUES ('iPad Pro 12.9', 'Apple', 'tablet', 'iPadOS', '17', 'M2', '8GB', 'Creative work tablet.', NULL);
END

IF NOT EXISTS (SELECT * FROM Devices WHERE Name = 'HP EliteBook')
BEGIN
    INSERT INTO Devices (Name, Manufacturer, Type, OS, OSVersion, Processor, RAMAmount, Description, AssignedUserId)
    VALUES ('HP EliteBook', 'HP', 'laptop', 'Windows', '10', 'Core i5', '16GB', 'Standard office laptop.', NULL);
END

IF NOT EXISTS (SELECT * FROM Devices WHERE Name = 'ROG Zephyrus G14')
BEGIN
    INSERT INTO Devices (Name, Manufacturer, Type, OS, OSVersion, Processor, RAMAmount, Description, AssignedUserId)
    VALUES ('ROG Zephyrus G14', 'ASUS', 'laptop', 'Windows', '11', 'Ryzen 9', '16GB', 'High-end development laptop.', NULL);
END

IF NOT EXISTS (SELECT * FROM Devices WHERE Name = 'MacBook Pro 16')
BEGIN
    INSERT INTO Devices (Name, Manufacturer, Type, OS, OSVersion, Processor, RAMAmount, Description, AssignedUserId)
    VALUES ('MacBook Pro 16', 'Apple', 'laptop', 'macOS', '14.2', 'M3 Max', '64GB', 'Power user machine.', 
    (SELECT TOP 1 Id FROM Users WHERE Email = 'john@example.com'));
END

IF NOT EXISTS (SELECT * FROM Devices WHERE Name = 'Surface Laptop 5')
BEGIN
    INSERT INTO Devices (Name, Manufacturer, Type, OS, OSVersion, Processor, RAMAmount, Description, AssignedUserId)
    VALUES ('Surface Laptop 5', 'Microsoft', 'laptop', 'Windows', '11', 'Core i7', '16GB', 'Executive laptop.', NULL);
END