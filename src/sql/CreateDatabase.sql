CREATE SCHEMA NotifyApp
GO

DROP TABLE NotifyApp.UserGroups
DROP TABLE NotifyApp.Groups
DROP TABLE NotifyApp.Users
DROP TABLE NotifyApp.Outlets

CREATE TABLE NotifyApp.Users (
  UserId INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
  Email varchar(256) NOT NULL,
  Password varchar(128) NOT NULL,
  IsDisabled BIT DEFAULT 0
)
GO

CREATE TABLE NotifyApp.Groups (
  GroupId INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
  GroupDisplayName varchar(256) NOT NULL
)
GO

CREATE TABLE NotifyApp.UserGroups (
  UserGroupDbId INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
  UserId INT NOT NULL,
  GroupId INT NOT NULL,
  FOREIGN KEY (UserID) REFERENCES Users(UserID),
  FOREIGN KEY (GroupID) REFERENCES Groups(GroupID)
)
GO

CREATE TABLE NotifyApp.Outlets (
  OutletId INT IDENTITY(1, 1) NOT NULL PRIMARY KEY,
  GroupId INT NOT NULL,
  OutletType varchar(128) NOT NULL,
  Target varchar(256) NOT NULL,
  Hash varchar(512) NOT NULL
)
GO
