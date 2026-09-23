-- =============================================
-- RaceDay Database
-- Part 1 - Database Script
-- =============================================

CREATE DATABASE RaceDay;
GO

USE RaceDay;
GO

CREATE TABLE Roles
(
    RoleId INT IDENTITY(1,1) NOT NULL,
    RoleName NVARCHAR(20) NOT NULL,

    CONSTRAINT PK_Roles
        PRIMARY KEY (RoleId),

    CONSTRAINT UQ_Roles_RoleName
        UNIQUE (RoleName)
);
GO

INSERT INTO Roles (RoleName)
VALUES
    ('Organiser'),
    ('Participant');
GO

CREATE TABLE EventTypes
(
    EventTypeId INT IDENTITY(1,1) NOT NULL,
    TypeName NVARCHAR(20) NOT NULL,

    CONSTRAINT PK_EventTypes
        PRIMARY KEY (EventTypeId),

    CONSTRAINT UQ_EventTypes_TypeName
        UNIQUE (TypeName)
);
GO

INSERT INTO EventTypes (TypeName)
VALUES
    ('Run'),
    ('Walk'),
    ('Cycle');
GO

CREATE TABLE Users
(
    UserId INT IDENTITY(1,1) NOT NULL,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    RoleId INT NOT NULL,
    Phone NVARCHAR(20) NULL,
    CreatedAt DATETIME2 NOT NULL
        CONSTRAINT DF_Users_CreatedAt
        DEFAULT SYSDATETIME(),

    CONSTRAINT PK_Users
        PRIMARY KEY (UserId),

    CONSTRAINT UQ_Users_Email
        UNIQUE (Email),

    CONSTRAINT FK_Users_Roles
        FOREIGN KEY (RoleId)
        REFERENCES Roles(RoleId)
);
GO

INSERT INTO Users
(
    FirstName,
    LastName,
    Email,
    PasswordHash,
    RoleId,
    Phone
)
VALUES
(
    'Thabo',
    'Mokoena',
    'thabo@raceday.co.za',
    'HASHED_PASSWORD_SAMPLE_001',
    (SELECT RoleId FROM Roles WHERE RoleName = 'Organiser'),
    '0821112233'
),
(
    'Lerato',
    'Dlamini',
    'lerato@raceday.co.za',
    'HASHED_PASSWORD_SAMPLE_002',
    (SELECT RoleId FROM Roles WHERE RoleName = 'Organiser'),
    '0832223344'
),
(
    'Sipho',
    'Nkosi',
    'sipho@example.com',
    'HASHED_PASSWORD_SAMPLE_003',
    (SELECT RoleId FROM Roles WHERE RoleName = 'Participant'),
    '0843334455'
),
(
    'Amahle',
    'Ndlovu',
    'amahle@example.com',
    'HASHED_PASSWORD_SAMPLE_004',
    (SELECT RoleId FROM Roles WHERE RoleName = 'Participant'),
    '0854445566'
);
GO