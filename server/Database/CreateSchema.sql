-- ReminderApp Database Schema
-- Run this script to create the database schema for the application

-- Create the database if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'ReminderAppDb')
BEGIN
    CREATE DATABASE ReminderAppDb;
END
GO

USE ReminderAppDb;
GO

-- Create Users table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Users')
BEGIN
    CREATE TABLE Users (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        Email NVARCHAR(256) NOT NULL,
        PasswordHash NVARCHAR(256) NOT NULL,
        CreatedAt DATETIME2 NOT NULL,
        CONSTRAINT UQ_Users_Email UNIQUE (Email)
    );
    
    CREATE UNIQUE INDEX IX_Users_Email ON Users(Email);
END
GO

-- Create Reminders table
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Reminders')
BEGIN
    CREATE TABLE Reminders (
        Id UNIQUEIDENTIFIER PRIMARY KEY,
        UserId UNIQUEIDENTIFIER NOT NULL,
        Text NVARCHAR(500) NOT NULL,
        ScheduledAt DATETIMEOFFSET NOT NULL,
        CreatedAt DATETIMEOFFSET NOT NULL,
        CONSTRAINT FK_Reminders_Users_UserId 
            FOREIGN KEY (UserId) REFERENCES Users(Id) 
            ON DELETE CASCADE
    );
    
    CREATE INDEX IX_Reminders_UserId ON Reminders(UserId);
END
GO

PRINT 'Database schema created successfully!';
