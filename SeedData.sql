--Users
IF NOT EXISTS (SELECT * FROM Users WHERE Name = 'John Doe')
BEGIN
    INSERT INTO Users (Name, Role, Location) 
    VALUES ('John Doe', 'Software Engineer', 'London');
END

IF NOT EXISTS (SELECT * FROM Users WHERE Name = 'Jane Smith')
BEGIN
    INSERT INTO Users (Name, Role, Location) 
    VALUES ('Jane Smith', 'Product Manager', 'New York');
END

--Devices 
IF NOT EXISTS (SELECT * FROM Devices WHERE Name = 'iPhone 15 Pro')
BEGIN
    INSERT INTO Devices (Name, Manufacturer, Type, OS, OSVersion, Processor, RAMAmount, Description, AssignedUserId)
    VALUES ('iPhone 15 Pro', 'Apple', 'phone', 'iOS', '17.0', 'A17 Pro', '8GB', 'Company flagship phone.', 1);
END

IF NOT EXISTS (SELECT * FROM Devices WHERE Name = 'Galaxy Tab S9')
BEGIN
    INSERT INTO Devices (Name, Manufacturer, Type, OS, OSVersion, Processor, RAMAmount, Description, AssignedUserId)
    VALUES ('Galaxy Tab S9', 'Samsung', 'tablet', 'Android', '14', 'Snapdragon 8 Gen 2', '12GB', 'High-end tablet for design.', NULL);
END