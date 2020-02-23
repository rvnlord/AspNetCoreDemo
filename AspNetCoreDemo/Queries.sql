SELECT * FROM Employees
SELECT * FROM __EFMigrationsHistory

SELECT * FROM AspNetUsers
SELECT * FROM AspNetRoles
SELECT * FROM AspNetUserRoles
SELECT * FROM AspNetUserClaims
SELECT * FROM AspNetUserLogins

DELETE FROM AspNetUserLogins WHERE UserId = '08024b46-03c2-4b78-8594-44753c49f909';
DELETE FROM AspNetUserRoles WHERE UserId = '08024b46-03c2-4b78-8594-44753c49f909';
DELETE FROM AspNetUserClaims WHERE UserId = '08024b46-03c2-4b78-8594-44753c49f909';
DELETE FROM AspNetUsers WHERE Id = '08024b46-03c2-4b78-8594-44753c49f909';

DECLARE @userId NVARCHAR(450)
SET @userId = (SELECT Id FROM AspNetUsers WHERE UserName = 'rvnlord@gmail.com')
DECLARE @roleId NVARCHAR(450)
SET @roleId = (SELECT Id FROM AspNetRoles WHERE Name = 'Admin')
INSERT INTO AspNetUserRoles (UserId, Roleid) VALUES (@userId, @roleId)

UPDATE AspNetUsers SET PasswordHash = NULL WHERE UserName = 'rvnlord@gmail.com'

