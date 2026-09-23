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

CREATE TABLE Events
(
    EventId INT IDENTITY(1,1) NOT NULL,
    OrganiserId INT NOT NULL,
    EventTypeId INT NOT NULL,
    EventName NVARCHAR(150) NOT NULL,
    Description NVARCHAR(1000) NOT NULL,
    EventDate DATE NOT NULL,
    Location NVARCHAR(200) NOT NULL,
    Distance DECIMAL(6,2) NOT NULL,
    CreatedAt DATETIME2 NOT NULL
        CONSTRAINT DF_Events_CreatedAt
        DEFAULT SYSDATETIME(),

    CONSTRAINT PK_Events
        PRIMARY KEY (EventId),

    CONSTRAINT FK_Events_Organiser
        FOREIGN KEY (OrganiserId)
        REFERENCES Users(UserId),

    CONSTRAINT FK_Events_EventType
        FOREIGN KEY (EventTypeId)
        REFERENCES EventTypes(EventTypeId),

    CONSTRAINT CK_Events_Distance
        CHECK (Distance > 0)
);
GO

INSERT INTO Events
(
    OrganiserId,
    EventTypeId,
    EventName,
    Description,
    EventDate,
    Location,
    Distance
)
VALUES
(
    (SELECT UserId FROM Users WHERE Email = 'thabo@raceday.co.za'),
    (SELECT EventTypeId FROM EventTypes WHERE TypeName = 'Run'),
    'Johannesburg City Run',
    'A community road running event through Johannesburg.',
    '2027-02-14',
    'Johannesburg, Gauteng',
    10.00
),
(
    (SELECT UserId FROM Users WHERE Email = 'thabo@raceday.co.za'),
    (SELECT EventTypeId FROM EventTypes WHERE TypeName = 'Walk'),
    'Soweto Community Walk',
    'A community walking event celebrating health and participation.',
    '2027-03-06',
    'Soweto, Gauteng',
    5.00
),
(
    (SELECT UserId FROM Users WHERE Email = 'lerato@raceday.co.za'),
    (SELECT EventTypeId FROM EventTypes WHERE TypeName = 'Cycle'),
    'Midrand Cycle Challenge',
    'A road cycling event for recreational and competitive cyclists.',
    '2027-04-18',
    'Midrand, Gauteng',
    21.00
);
GO

CREATE TABLE Categories
(
    CategoryId INT IDENTITY(1,1) NOT NULL,
    EventId INT NOT NULL,
    CategoryName NVARCHAR(100) NOT NULL,
    CategoryType NVARCHAR(20) NOT NULL,
    CategoryValue NVARCHAR(50) NULL,

    CONSTRAINT PK_Categories
        PRIMARY KEY (CategoryId),

    CONSTRAINT FK_Categories_Events
        FOREIGN KEY (EventId)
        REFERENCES Events(EventId),

    CONSTRAINT CK_Categories_Type
        CHECK (CategoryType IN ('Age', 'Distance'))
);
GO

