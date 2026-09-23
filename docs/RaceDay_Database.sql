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


-- Users
CREATE TABLE Users
(
    ...
);
GO