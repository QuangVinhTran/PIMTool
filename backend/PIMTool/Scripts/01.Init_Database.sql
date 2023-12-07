-- CREATE TABLES

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'PIMUser')
BEGIN
    
CREATE TABLE [PIMUser] (
    [Id]                          UNIQUEIDENTIFIER                PRIMARY KEY                 DEFAULT NEWID(),
    [FirstName]                   NVARCHAR(50)                    NOT NULL,
    [LastName]                    NVARCHAR(50)                    NULL,
    [BirthDate]                   DATE                            NULL,
    [Email]                       VARCHAR(50)                     NOT NULL,
    [Password]                    VARCHAR(255)                    NOT NULL,
    [Role]                        VARCHAR(50)                     NOT NULL                    DEFAULT 'Employee',
    [Version]                     INT                             NOT NULL                    DEFAULT 0,
    [IsDeleted]                   BIT                             NOT NULL                    DEFAULT 0,
    [CreatedAt]                   DATE                            NOT NULL,
    [CreatedBy]                   UNIQUEIDENTIFIER                NULL,
    [UpdatedAt]                   DATE                            NULL,
    [UpdatedBy]                   UNIQUEIDENTIFIER                NULL
)
       
END


IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'RefreshToken')
BEGIN
    
CREATE TABLE [RefreshToken] (
    [Token]                       UNIQUEIDENTIFIER                PRIMARY KEY                 DEFAULT NEWID(),
    [UserId]                      UNIQUEIDENTIFIER                NULL,
    [IPAddress]                   VARCHAR(20)                     NULL,
    [ValidUntil]                  DATETIME                        NOT NULL,
    [CreatedAt]                   DATETIME                        NOT NULL,
    [UpdatedAt]                   DATETIME                        NULL,
)

END



IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Employee')
BEGIN
    
CREATE TABLE [Employee] (
    [Id]                          UNIQUEIDENTIFIER                PRIMARY KEY                 DEFAULT NEWID(),
    [Visa]                        CHAR(3)                         NOT NULL,
    [FirstName]                   VARCHAR(50)                     NOT NULL,
    [LastName]                    VARCHAR(50)                     NOT NULL,
    [BirthDate]                   DATE                            NOT NULL,
    [Version]                     INT                             NOT NULL,
    [IsDeleted]                   BIT                             NOT NULL                    DEFAULT 0,
    [CreatedAt]                   DATE                            NOT NULL,
    [CreatedBy]                   UNIQUEIDENTIFIER                NULL,
    [UpdatedAt]                   DATE                            NULL,
    [UpdatedBy]                   UNIQUEIDENTIFIER                NULL
)

END



IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Project')
BEGIN
    
CREATE TABLE [Project] (
    [Id]                          UNIQUEIDENTIFIER                PRIMARY KEY                 DEFAULT NEWID(),
    [GroupId]                     UNIQUEIDENTIFIER                NOT NULL,
    [ProjectNumber]               INT                             NOT NULL,
    [Name]                        VARCHAR(50)                     NOT NULL,
    [Customer]                    VARCHAR(50)                     NOT NULL,
    [Status]                      CHAR(3)                         NOT NULL,
    [StartDate]                   DATE                            NOT NULL,
    [EndDate]                     DATE                            NULL,
    [Version]                     INT                             NOT NULL,
    [IsDeleted]                   BIT                             NOT NULL                    DEFAULT 0,
    [CreatedAt]                   DATE                            NOT NULL,
    [CreatedBy]                   UNIQUEIDENTIFIER                NULL,
    [UpdatedAt]                   DATE                            NULL,
    [UpdatedBy]                   UNIQUEIDENTIFIER                NULL
)

END



IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Group')
BEGIN
    
CREATE TABLE [Group] (
    [Id]                          UNIQUEIDENTIFIER                PRIMARY KEY                 DEFAULT NEWID(),
    [LeaderId]                    UNIQUEIDENTIFIER                NOT NULL,
    [Version]                     INT                             NOT NULL,
    [IsDeleted]                   BIT                             NOT NULL                    DEFAULT 0,
    [CreatedAt]                   DATE                            NOT NULL,
    [CreatedBy]                   UNIQUEIDENTIFIER                NULL,
    [UpdatedAt]                   DATE                            NULL,
    [UpdatedBy]                   UNIQUEIDENTIFIER                NULL
)

END



IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Project_Employee')
BEGIN
    
CREATE TABLE [Project_Employee] (
    [ProjectId]                   UNIQUEIDENTIFIER                NOT NULL,
    [EmployeeId]                  UNIQUEIDENTIFIER                NOT NULL
)

END 

-- END CREATE TABLES

-- ADD FOREIGN KEYS

ALTER TABLE [RefreshToken]
    ADD FOREIGN KEY (UserId) REFERENCES [PIMUser](Id)

ALTER TABLE [Project]
    ADD FOREIGN KEY (GroupId) REFERENCES [Group](Id)

ALTER TABLE [Group]
    ADD FOREIGN KEY (LeaderId) REFERENCES [Employee](Id)

ALTER TABLE [Project_Employee]
    ADD FOREIGN KEY (ProjectId) REFERENCES [Project](Id)
ALTER TABLE [Project_Employee]
    ADD FOREIGN KEY (EmployeeId) REFERENCES [Employee](Id)

-- END ADD FOREIGN KEY

-- ADD CONSTRAINTS

ALTER TABLE [Project_Employee]
    ADD CONSTRAINT "PK_Project_Employee" PRIMARY KEY ("ProjectId", "EmployeeId")

-- END ADD CONSTRAINTS
