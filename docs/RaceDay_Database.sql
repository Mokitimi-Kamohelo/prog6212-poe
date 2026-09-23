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

SELECT *
FROM Roles;

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

SELECT *
FROM EventTypes;

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

SELECT *
FROM Users;

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

SELECT *
FROM Events;

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

INSERT INTO Categories
(
    EventId,
    CategoryName,
    CategoryType,
    CategoryValue
)
VALUES

(
    (SELECT EventId FROM Events WHERE EventName = 'Johannesburg City Run'),
    'Under 20',
    'Age',
    'Under 20'
),
(
    (SELECT EventId FROM Events WHERE EventName = 'Johannesburg City Run'),
    'Senior',
    'Age',
    'Senior'
),
(
    (SELECT EventId FROM Events WHERE EventName = 'Johannesburg City Run'),
    '10km',
    'Distance',
    '10'
),

(
    (SELECT EventId FROM Events WHERE EventName = 'Soweto Community Walk'),
    'Under 20',
    'Age',
    'Under 20'
),
(
    (SELECT EventId FROM Events WHERE EventName = 'Soweto Community Walk'),
    'Senior',
    'Age',
    'Senior'
),
(
    (SELECT EventId FROM Events WHERE EventName = 'Soweto Community Walk'),
    '5km',
    'Distance',
    '5'
),

(
    (SELECT EventId FROM Events WHERE EventName = 'Midrand Cycle Challenge'),
    'Junior',
    'Age',
    'Junior'
),
(
    (SELECT EventId FROM Events WHERE EventName = 'Midrand Cycle Challenge'),
    'Senior',
    'Age',
    'Senior'
),
(
    (SELECT EventId FROM Events WHERE EventName = 'Midrand Cycle Challenge'),
    '21km',
    'Distance',
    '21'
);
GO

SELECT *
FROM Categories;

CREATE TABLE Enrolments
(
    EnrolmentId INT IDENTITY(1,1) NOT NULL,
    ParticipantId INT NOT NULL,
    EventId INT NOT NULL,
    CategoryId INT NOT NULL,
    EnrolmentDate DATETIME2 NOT NULL
        CONSTRAINT DF_Enrolments_EnrolmentDate
        DEFAULT SYSDATETIME(),
    Status NVARCHAR(20) NOT NULL
        CONSTRAINT DF_Enrolments_Status
        DEFAULT 'Registered',

    CONSTRAINT PK_Enrolments
        PRIMARY KEY (EnrolmentId),

    CONSTRAINT FK_Enrolments_Participant
        FOREIGN KEY (ParticipantId)
        REFERENCES Users(UserId),

    CONSTRAINT FK_Enrolments_Event
        FOREIGN KEY (EventId)
        REFERENCES Events(EventId),

    CONSTRAINT FK_Enrolments_Category
        FOREIGN KEY (CategoryId)
        REFERENCES Categories(CategoryId),

    CONSTRAINT CK_Enrolments_Status
        CHECK (Status IN ('Registered', 'Cancelled', 'Completed')),

    CONSTRAINT UQ_Enrolments_Participant_Event
        UNIQUE (ParticipantId, EventId)
);
GO

INSERT INTO Enrolments
(
    ParticipantId,
    EventId,
    CategoryId,
    Status
)
VALUES
(
    (SELECT UserId FROM Users WHERE Email = 'sipho@example.com'),
    (SELECT EventId FROM Events WHERE EventName = 'Johannesburg City Run'),
    (
        SELECT CategoryId
        FROM Categories
        WHERE EventId =
            (SELECT EventId FROM Events
             WHERE EventName = 'Johannesburg City Run')
        AND CategoryName = '10km'
    ),
    'Registered'
),
(
    (SELECT UserId FROM Users WHERE Email = 'amahle@example.com'),
    (SELECT EventId FROM Events WHERE EventName = 'Johannesburg City Run'),
    (
        SELECT CategoryId
        FROM Categories
        WHERE EventId =
            (SELECT EventId FROM Events
             WHERE EventName = 'Johannesburg City Run')
        AND CategoryName = 'Senior'
    ),
    'Registered'
),
(
    (SELECT UserId FROM Users WHERE Email = 'sipho@example.com'),
    (SELECT EventId FROM Events WHERE EventName = 'Soweto Community Walk'),
    (
        SELECT CategoryId
        FROM Categories
        WHERE EventId =
            (SELECT EventId FROM Events
             WHERE EventName = 'Soweto Community Walk')
        AND CategoryName = '5km'
    ),
    'Registered'
);
GO

SELECT *
FROM Enrolments;

CREATE TABLE Results
(
    ResultId INT IDENTITY(1,1) NOT NULL,
    EnrolmentId INT NOT NULL,
    FinishTime TIME NOT NULL,
    FinishPosition INT NOT NULL,

    CONSTRAINT PK_Results
        PRIMARY KEY (ResultId),

    CONSTRAINT FK_Results_Enrolments
        FOREIGN KEY (EnrolmentId)
        REFERENCES Enrolments(EnrolmentId),

    CONSTRAINT UQ_Results_Enrolment
        UNIQUE (EnrolmentId),

    CONSTRAINT CK_Results_Position
        CHECK (FinishPosition > 0)
);
GO

INSERT INTO Results
(
    EnrolmentId,
    FinishTime,
    FinishPosition
)
VALUES
(
    (
        SELECT EnrolmentId
        FROM Enrolments
        WHERE ParticipantId =
            (SELECT UserId FROM Users
             WHERE Email = 'sipho@example.com')
        AND EventId =
            (SELECT EventId FROM Events
             WHERE EventName = 'Johannesburg City Run')
    ),
    '00:52:34',
    18
),
(
    (
        SELECT EnrolmentId
        FROM Enrolments
        WHERE ParticipantId =
            (SELECT UserId FROM Users
             WHERE Email = 'amahle@example.com')
        AND EventId =
            (SELECT EventId FROM Events
             WHERE EventName = 'Johannesburg City Run')
    ),
    '00:58:21',
    27
);
GO

SELECT *
FROM Results;

SELECT
    u.FirstName + ' ' + u.LastName AS Participant,
    e.EventName,
    c.CategoryName,
    et.TypeName AS EventType,
    en.Status,
    r.FinishTime,
    r.FinishPosition
FROM Enrolments en
INNER JOIN Users u
    ON en.ParticipantId = u.UserId
INNER JOIN Events e
    ON en.EventId = e.EventId
INNER JOIN Categories c
    ON en.CategoryId = c.CategoryId
INNER JOIN EventTypes et
    ON e.EventTypeId = et.EventTypeId
LEFT JOIN Results r
    ON en.EnrolmentId = r.EnrolmentId
ORDER BY e.EventName, r.FinishPosition;