CREATE SCHEMA NotifyApp
GO

IF EXISTS(SELECT * FROM NotifyApp.Users)
    DROP TABLE NotifyApp.Users
GO

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

