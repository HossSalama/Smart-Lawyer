
USE master;
GO
CREATE DATABASE LegalCaseManagementDB
ON PRIMARY (
    NAME        = 'LegalCaseManagementDB_Data',
    FILENAME    = 'D:\Full Stack .Net ITI\Smart Lawyer C# Widows Form Project\LegalCaseManagementDB.mdf',
    SIZE        = 128MB,
    MAXSIZE     = UNLIMITED,
    FILEGROWTH  = 64MB
)
log ON (
    NAME        = 'LegalCaseManagementDB_Log',
    FILENAME    = 'D:\Full Stack .Net ITI\Smart Lawyer C# Widows Form Project\LegalCaseManagementDB.ldf',
    SIZE        = 64MB,
    MAXSIZE     = 1024MB,
    FILEGROWTH  = 32MB
);
GO
USE [LegalCaseManagementDB] ;
GO
ALTER DATABASE LegalCaseManagementDB SET RECOVERY FULL;
ALTER DATABASE LegalCaseManagementDB SET AUTO_SHRINK OFF;
ALTER DATABASE LegalCaseManagementDB SET AUTO_CREATE_STATISTICS ON;
ALTER DATABASE LegalCaseManagementDB SET AUTO_UPDATE_STATISTICS ON;
ALTER DATABASE LegalCaseManagementDB COLLATE Arabic_CI_AI; 
GO
 


 
CREATE SCHEMA Core AUTHORIZATION dbo;
GO
 
CREATE SCHEMA Legal AUTHORIZATION dbo;
GO
 
CREATE SCHEMA Finance AUTHORIZATION dbo;
GO
 
CREATE SCHEMA Docs AUTHORIZATION dbo;
GO
 
CREATE SCHEMA Lookup AUTHORIZATION dbo;
GO
CREATE SCHEMA [admin] AUTHORIZATION dbo;
GO

CREATE TABLE [Core].[Roles] (
    [Id]          INT             PRIMARY KEY ,
    [RoleName]    NVARCHAR(10)    NOT NULL UNIQUE default N'محامي' ,
    [Permissions] NVARCHAR(MAX)   NULL,    
    CONSTRAINT [CK_Roles_RoleType] CHECK ([RoleName] IN (N'مدير', N'محامي', N'سكرتاريه')),
    CONSTRAINT [CK_Roles_RoleName] CHECK (LEN(LTRIM(RTRIM([RoleName]))) > 0)
) ON [PRIMARY]; 

CREATE TABLE [Lookup].[CaseTypes] (
    [Id]          INT             PRIMARY KEY, 
    [TypeName]    NVARCHAR(15)   NOT NULL UNIQUE ,     
    CONSTRAINT [CK_CaseTypes_TypeName] CHECK (LEN(LTRIM(RTRIM([TypeName]))) > 0)
) ON [PRIMARY];
GO
GO



CREATE TABLE [Lookup].[CaseStatuses] (
    [Id]          INT             PRIMARY KEY, 
    [StatusName]  NVARCHAR(15)   NOT NULL UNIQUE Default N'مفتوحة',
    [Color]       VARCHAR(7)     NOT NULL DEFAULT '#1D9E75', 

    CONSTRAINT [CK_CaseStatuses_Color] 
    CHECK ([Color] LIKE '#[0-9A-Fa-f][0-9A-Fa-f][0-9A-Fa-f][0-9A-Fa-f][0-9A-Fa-f][0-9A-Fa-f]')
) ON [PRIMARY];

CREATE TABLE [Lookup].[Courts] (
    [Id]          INT             PRIMARY KEY IDENTITY(1,1),
    [CourtName]   NVARCHAR(100)   NOT NULL UNIQUE,
    [Location]    NVARCHAR(100)   NULL,
    [Address]     NVARCHAR(100)   NULL,
    [Phone]       VARCHAR(11)     NULL,
    CONSTRAINT [CK_Courts_CourtName] CHECK (LEN(LTRIM(RTRIM([CourtName]))) > 0)
) ON [PRIMARY];

CREATE TABLE [Lookup].[Departments] (
    [Id]             INT             PRIMARY KEY IDENTITY(1,1),
    [DeptName]       NVARCHAR(100)   NOT NULL, 
    [CourtId]        INT             NOT NULL,
    [JudgeName]      NVARCHAR(100)   NULL,
    constraint [UQ_Departments] UNIQUE ([DeptName], [CourtId]),
    CONSTRAINT [FK_Departments_Courts] FOREIGN KEY ([CourtId]) REFERENCES [Lookup].[Courts]([Id]),
    CONSTRAINT [CK_Departments_DeptName] CHECK (LEN(LTRIM(RTRIM([DeptName]))) > 0)
) ON [PRIMARY];


CREATE TABLE [Core].[Users] (
    [Id]              INT             PRIMARY KEY IDENTITY(1,1),
    [FullName]        NVARCHAR(100)  NOT NULL,
    [Email]           VARCHAR(200)   NOT NULL UNIQUE,
    [PasswordHash]    VARCHAR(500)   NOT NULL, 
    [PhoneNumber]     CHAR(11)       NOT NULL UNIQUE, 
    [SecondNumber]    CHAR(11)       NULL, 
    [NationalId]      CHAR(14)       NULL UNIQUE, 
    [RoleId]          INT            NOT NULL,
    [IsActive]        BIT            NOT NULL DEFAULT 1, 
    [LastLoginAt]     DATETIME       NULL,    
    CONSTRAINT [FK_Users_Roles] FOREIGN KEY ([RoleId]) REFERENCES [Core].[Roles]([Id]),
    CONSTRAINT [CK_Users_Email] CHECK ([Email] LIKE '%@%.%'), 
    CONSTRAINT [CK_Users_FullName] CHECK (LEN(LTRIM(RTRIM([FullName]))) > 0),
    CONSTRAINT [CK_Users_Phone] CHECK ([PhoneNumber] LIKE '01[0-2,5]%'),
        CONSTRAINT [CK_Users_PassHash] CHECK (LEN([PasswordHash]) >= 20)
) ON [PRIMARY];
GO

CREATE TABLE [Legal].[Clients] (
    [Id]              INT               PRIMARY KEY IDENTITY(1,1),
    [FullName]        NVARCHAR(100)     NOT NULL,
    [NationalId]      CHAR(14)         NULL UNIQUE, 
    [CommercialReg]   VARCHAR(50)      NULL UNIQUE, 
    [Phone]           CHAR(11)      NOT NULL, 
    [SecondaryPhone]  CHAR(11)      NULL,
    [Address]         NVARCHAR(100)     NOT NULL,
    [JobTitle]        NVARCHAR(50)     NULL,
    [Gender]          NVARCHAR(10)      NULL,
    [Email]           VARCHAR(100)      NULL,
    [ClientType]      NVARCHAR(10)      NOT NULL DEFAULT N'فرد',
    [IsActive]        BIT               NOT NULL DEFAULT 1,
	[CreatedAt]     DATETIME2         NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [CK_Clients_NationalId] CHECK ([NationalId] IS NULL OR [NationalId] LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
    CONSTRAINT [CK_Clients_Identification] CHECK (
        ([ClientType] = N'فرد' AND [NationalId] IS NOT NULL) OR 
        ([ClientType] = N'شركة' AND [CommercialReg] IS NOT NULL) OR
        ([ClientType] NOT IN (N'فرد', N'شركة'))
    ),

    CONSTRAINT [CK_Clients_ClientType] CHECK ([ClientType] IN (N'فرد', N'شركة', N'جهة حكومية', N'أخرى')),
    CONSTRAINT [CK_Clients_Gender]     CHECK ([Gender] IN (N'ذكر', N'أنثى')),
    CONSTRAINT [CK_Clients_FullName]   CHECK (LEN(LTRIM(RTRIM([FullName]))) > 0),
    CONSTRAINT [CK_Clients_Email]      CHECK ([Email] IS NULL OR [Email] LIKE '%@%.%')
) ON [PRIMARY];
GO

CREATE TABLE [Legal].[Opponents] (
    [Id]            INT             PRIMARY KEY IDENTITY(1,1),
    [FullName]      NVARCHAR(100)   NOT NULL,
    [NationalId]    CHAR(14)       NULL UNIQUE, 
    [Phone]         CHAR(11)    NULL,
    [Address]       NVARCHAR(100)   NOT NULL,  
    [OpponentLawyerName]  NVARCHAR(200)   NULL,    
    [OpponentLawyerPhone] CHAR(11)    NULL,   
    [CreatedAt]     DATETIME        NOT NULL DEFAULT GETDATE(),

    CONSTRAINT [CK_Opponents_FullName] CHECK (LEN(LTRIM(RTRIM([FullName]))) > 0)
) ON [PRIMARY];

GO
CREATE TABLE [Legal].Cases (
    Id                  INT             PRIMARY KEY IDENTITY(1,1),
    CaseNumber          NVARCHAR(100)    NOT NULL ,
    Title               NVARCHAR(500)   NOT NULL,
    OpenDate            DATE            NOT NULL DEFAULT CAST(GETDATE() AS DATE),
    CloseDate           DATE            NULL,
    Stage               NVARCHAR(100)   NULL,
    IsArchived          BIT             NOT NULL DEFAULT 0,
    ArchivedAt          DATETIME        NULL,
    ArchivedBy          INT             NULL,
    ArchiveNote         NVARCHAR(500)   NULL,
    UpdatedAt           DATETIME        NOT NULL DEFAULT GETDATE(),
    ClientId            INT             NOT NULL,
    CaseTypeId          INT             NOT NULL,
    CourtId             INT             NOT NULL,
    [DeptId]            INT             NULL,    
    StatusId            INT             NOT NULL,
    AssignedLawyerId    INT             NOT NULL,  


    CONSTRAINT FK_Cases_Client          FOREIGN KEY (ClientId)          REFERENCES [Legal].Clients(Id),
    CONSTRAINT FK_Cases_CaseType        FOREIGN KEY (CaseTypeId)        REFERENCES [Lookup].CaseTypes(Id),
    CONSTRAINT FK_Cases_Court           FOREIGN KEY (CourtId)           REFERENCES [Lookup].Courts(Id),
    CONSTRAINT FK_Cases_Status          FOREIGN KEY (StatusId)          REFERENCES [Lookup].CaseStatuses(Id),
    CONSTRAINT FK_Cases_Lawyer          FOREIGN KEY (AssignedLawyerId)  REFERENCES [Core].Users(Id),
    CONSTRAINT FK_Cases_ArchivedBy      FOREIGN KEY (ArchivedBy)        REFERENCES [Core].Users(Id),
    CONSTRAINT [FK_Cases_Dept]          FOREIGN KEY ([DeptId])          REFERENCES [Lookup].[Departments]([Id]),
    CONSTRAINT CK_Cases_Title           CHECK (LEN(LTRIM(RTRIM(Title))) > 0),
    CONSTRAINT CK_Cases_CaseNumber      CHECK (LEN(LTRIM(RTRIM(CaseNumber))) > 0),
    CONSTRAINT CK_Cases_Dates           CHECK (CloseDate IS NULL OR CloseDate >= OpenDate),
    CONSTRAINT CK_Cases_Archive         CHECK (
        (IsArchived = 0 AND ArchivedAt IS NULL AND ArchivedBy IS NULL) OR
        (IsArchived = 1 AND ArchivedAt IS NOT NULL AND ArchivedBy IS NOT NULL)
    )
)ON [PRIMARY];



CREATE TABLE [Legal].CaseOpponents (
    Id          INT         PRIMARY KEY IDENTITY(1,1),
    CaseId      INT         NOT NULL,
    OpponentId  INT         NOT NULL,
    AddedAt     DATETIME    NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_CaseOpponents_Case        FOREIGN KEY (CaseId)     REFERENCES [Legal].Cases(Id)     ON DELETE CASCADE,
    CONSTRAINT FK_CaseOpponents_Opponent    FOREIGN KEY (OpponentId) REFERENCES [Legal].Opponents(Id),
    CONSTRAINT UQ_CaseOpponents             UNIQUE (CaseId, OpponentId),

)ON [PRIMARY];

CREATE TABLE [Legal].[CaseLawyers] (
    [Id]          INT             PRIMARY KEY IDENTITY(1,1),
    [CaseId]      INT             NOT NULL,
    [UserId]      INT             NOT NULL, 
    [Role]        NVARCHAR(100)   NOT NULL DEFAULT N'محامي مساعد',
    [AssignedAt]  DATETIME        NOT NULL DEFAULT GETDATE(),
    [RemovedAt]   DATETIME        NULL, 
    [IsActive]    BIT             NOT NULL DEFAULT 1,

    CONSTRAINT [FK_CaseLawyers_Case] FOREIGN KEY ([CaseId]) REFERENCES [Legal].[Cases]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_CaseLawyers_User] FOREIGN KEY ([UserId]) REFERENCES [Core].[Users]([Id]),
    CONSTRAINT [UQ_CaseLawyers]      UNIQUE ([CaseId], [UserId]),
    
    CONSTRAINT [CK_CaseLawyers_Dates] CHECK ([RemovedAt] IS NULL OR [RemovedAt] >= [AssignedAt]),
    CONSTRAINT [CK_CaseLawyers_Role]  CHECK (LEN(LTRIM(RTRIM([Role]))) > 0)
) ON [PRIMARY];
GO


CREATE TABLE [Legal].[Hearings] (
    [Id]               INT             PRIMARY KEY IDENTITY(1,1),
    [CaseId]           INT             NOT NULL,
    [CourtId]          INT             NOT NULL,
    [DeptId]           INT             not NULL,  
    [HearingType]      NVARCHAR(50)    NOT NULL DEFAULT N'جلسة',
    [HearingDateTime]  DATETIME        NOT NULL, 
    [JudgeName]        NVARCHAR(100)   not NULL,
    [Period] AS (CASE WHEN DATEPART(HOUR, [HearingDateTime]) < 12 THEN N'صباحي' ELSE N'مسائي' END), 
    [AttendanceStatus] NVARCHAR(50)    NOT NULL DEFAULT N'قادم',
    
    [Result]           NVARCHAR(MAX)   not  NULL,     
    [NextHearingDate]  DATE            not NULL ,  
    [NextHearingPeriod]NVARCHAR(20)    null ,
    [CreatedAt]        DATETIME        NOT NULL DEFAULT GETDATE(),
    [CreatedBy]        INT             NOT NULL,

    CONSTRAINT [FK_Hearings_Case]      FOREIGN KEY ([CaseId])    REFERENCES [Legal].[Cases]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Hearings_Court]     FOREIGN KEY ([CourtId])   REFERENCES [Lookup].[Courts]([Id]),
    CONSTRAINT [FK_Hearings_Dept]      FOREIGN KEY ([DeptId])    REFERENCES [Lookup].[Departments]([Id]),
    CONSTRAINT [FK_Hearings_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users]([Id]),
    
    CONSTRAINT [CK_Hearings_Type]       CHECK ([HearingType] IN (N'جلسة', N'تحقيق',N'خبرة', N'أخرى')),
    CONSTRAINT [CK_Hearings_Period]     CHECK ([NextHearingPeriod] IN (N'صباحي', N'مسائي')),
    CONSTRAINT [CK_Hearings_Attendance]   CHECK ([AttendanceStatus] IN (N'قادم', N'غاب', N'أُجّل')),
    CONSTRAINT [CK_Hearings_NextDate]     CHECK ([NextHearingDate] IS NULL OR [NextHearingDate] > CAST([HearingDateTime] AS DATE))
) ON [PRIMARY];
GO


CREATE TABLE [Docs].[Documents] (
    [Id]            INT             PRIMARY KEY IDENTITY(1,1),
    [CaseId]        INT             NOT NULL,
    [Title]         NVARCHAR(500)   NOT NULL,       
    [DocType]       NVARCHAR(50)   NOT NULL,      
    [FilePath]      VARCHAR(500)  NOT NULL,       
    [MimeType]      VARCHAR(10)   Not NULL,  --enum       
    [UploadedBy]    INT             NOT NULL,     
    [UploadedAt]    DATETIME        NOT NULL DEFAULT GETDATE(),
    [IsArchived]    BIT             NOT NULL DEFAULT 0,
    [ArchivedAt]    DATE       NULL,
    [ArchivedBy]    INT             NULL,
    [Notes]         NVARCHAR(MAX)   NULL,
    CONSTRAINT [FK_Documents_Case]        FOREIGN KEY ([CaseId])     REFERENCES [Legal].[Cases]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Documents_UploadedBy]  FOREIGN KEY ([UploadedBy]) REFERENCES [Core].[Users]([Id]),
    CONSTRAINT [FK_Documents_ArchivedBy]  FOREIGN KEY ([ArchivedBy]) REFERENCES [Core].[Users]([Id]),

    CONSTRAINT [CK_Documents_DocType]     CHECK ([DocType] IN (N'مذكرة', N'توكيل', N'حكم', N'عقد', N'صورة', N'تقرير', N'إيصال', N'أخرى')),
    CONSTRAINT [CK_Documents_Title]       CHECK (LEN(LTRIM(RTRIM([Title]))) > 0),
    CONSTRAINT [CK_Documents_Archive]     CHECK (
        ([IsArchived] = 0 AND [ArchivedAt] IS NULL) OR
        ([IsArchived] = 1 AND [ArchivedAt] IS NOT NULL AND [ArchivedBy] IS NOT NULL)
    )
) ON [PRIMARY];
GO

CREATE TABLE [Docs].[LegalLibrary] (
    [Id]            INT             PRIMARY KEY IDENTITY(1,1),
    [Title]         NVARCHAR(500)   NOT NULL,        
    [Category]      NVARCHAR(100)   NOT NULL,  --3num  
    [MimeType]      VARCHAR(10)   Not NULL,  --enum       
    
    [FilePath]      VARCHAR(500)  NOT NULL,       
    [Description]   NVARCHAR(MAX)   NULL,           
    [AddedBy]       INT             NOT NULL,
    CONSTRAINT [FK_LegalLibrary_AddedBy] FOREIGN KEY ([AddedBy]) REFERENCES [Core].[Users]([Id]),
    CONSTRAINT [CK_LegalLibrary_Title]    CHECK (LEN(LTRIM(RTRIM([Title]))) > 0) ,
    CONSTRAINT [CK_LegalLibrary_Cat]      CHECK ([Category] IN (N'مدني', N'جنائي', N'أحوال شخصية', N'تجاري', N'أخرى'))
) ON [PRIMARY];
GO
CREATE TABLE [Docs].[DocumentTemplates] (
    [Id]        INT             PRIMARY KEY IDENTITY(1,1),
    [Title]     NVARCHAR(500)   NOT NULL,
    [MimeType]  VARCHAR(10)   NOT NULL, --enum
    [FilePath]  NVARCHAR(500)  NOT NULL,
    [AddedBy]   INT             NOT NULL,
    CONSTRAINT [FK_DocumentTemplates_AddedBy] FOREIGN KEY ([AddedBy]) REFERENCES [Core].[Users]([Id]),
    CONSTRAINT [CK_DocumentTemplates_Title]   CHECK (LEN(LTRIM(RTRIM([Title]))) > 0)
) ON [PRIMARY];
GO

CREATE TABLE [Legal].[Notes] (
    [Id]            INT             PRIMARY KEY IDENTITY(1,1),
    [Content]       NVARCHAR(MAX)   NOT NULL,
    [NoteType]      NVARCHAR(50)    NOT NULL DEFAULT N'عامة',
    [RelatedTable]  NVARCHAR(100)   NOT NULL, 
    [RelatedId]     INT             NOT NULL, 
    [UserId]        INT             NOT NULL, 
    [CreatedAt]     DATETIME        NOT NULL DEFAULT GETDATE(),

    CONSTRAINT [FK_Notes_User]      FOREIGN KEY ([UserId]) REFERENCES [Core].[Users]([Id]),
    CONSTRAINT [CK_Notes_Content]   CHECK (LEN(LTRIM(RTRIM([Content]))) > 0),
    CONSTRAINT [CK_Notes_NoteType]  CHECK ([NoteType] IN (N'عامة', N'قانونية', N'داخلية', N'تحذير', N'متابعة')),
    CONSTRAINT [CK_Notes_RelTable]  CHECK ([RelatedTable] IN (N'Cases', N'Hearings', N'Documents', N'Fees', N'Appeals', N'Clients'))
) ON [PRIMARY];
Go

CREATE TABLE [Legal].[Appeals] (
    [Id]               INT             PRIMARY KEY IDENTITY(1,1),
    [CaseId]           INT             NOT NULL,       
    [CourtId]          INT             NOT NULL,     
    [AppealNumber]     VARCHAR(100)   NULL UNIQUE, 
    [AppealType]       NVARCHAR(100)   NOT NULL,       
    [AppealDate]       DATE            NOT NULL,     
    [StatusId]         INT             NOT NULL,     
    [Grounds]          NVARCHAR(MAX)   NULL,          
    [Result]           NVARCHAR(MAX)   NULL,        
    [ResultDate]       DATE            NULL,       
    [AssignedLawyerId] INT             NOT NULL,   
    [Notes]            NVARCHAR(MAX)   NULL,
    [CreatedAt]        DATETIME        NOT NULL DEFAULT GETDATE(),

    CONSTRAINT [FK_Appeals_Case]      FOREIGN KEY ([CaseId])           REFERENCES [Legal].[Cases]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Appeals_Court]     FOREIGN KEY ([CourtId])          REFERENCES [Lookup].[Courts]([Id]),
    CONSTRAINT [FK_Appeals_Status]    FOREIGN KEY ([StatusId])         REFERENCES [Lookup].[CaseStatuses]([Id]),
    CONSTRAINT [FK_Appeals_Lawyer]    FOREIGN KEY ([AssignedLawyerId]) REFERENCES [Core].[Users]([Id]),
    CONSTRAINT [CK_Appeals_Type]       CHECK ([AppealType] IN (N'استئناف', N'طعن بالنقض', N'إعادة نظر', N'أخرى')),
    CONSTRAINT [CK_Appeals_Dates]      CHECK ([ResultDate] IS NULL OR [ResultDate] >= [AppealDate])
) ON [PRIMARY];
GO


CREATE TABLE [Finance].[Fees] (
    [Id]              INT             PRIMARY KEY IDENTITY(1,1),
    [CaseId]          INT             NOT NULL,
    [ClientId]        INT             NOT NULL,
    [TotalAmount]     DECIMAL(18,2)   NOT NULL,
    [DueDate]         DATE            NOT NULL,
    [Notes]           NVARCHAR(MAX)   NULL,
    [CreatedAt]       DATETIME        NOT NULL DEFAULT GETDATE(),
    [CreatedBy]       INT             NOT NULL,

    CONSTRAINT [FK_Fees_Case]      FOREIGN KEY ([CaseId])    REFERENCES [Legal].[Cases]([Id]) ON DELETE no action,
    CONSTRAINT [FK_Fees_Client]    FOREIGN KEY ([ClientId])  REFERENCES [Legal].[Clients]([Id]),
    CONSTRAINT [FK_Fees_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Core].[Users]([Id]),
    CONSTRAINT [CK_Fees_TotalAmount] CHECK ([TotalAmount] > 0)
) ON [PRIMARY];

CREATE TABLE [Finance].[PaymentSchedule] (
    [Id]                INT             PRIMARY KEY IDENTITY(1,1),
    [FeeId]             INT             NOT NULL,
    [InstallmentNumber] INT             NOT NULL,
    [PlannedAmount]     DECIMAL(18,2)   NOT NULL,
    [DueDate]           DATE            NOT NULL,
    [Status]            NVARCHAR(50)    NOT NULL DEFAULT N'معلق',
    [Notes]             NVARCHAR(500)   NULL,

    CONSTRAINT [FK_PaySched_Fee]      FOREIGN KEY ([FeeId]) REFERENCES [Finance].[Fees]([Id]) ON DELETE no action,
    CONSTRAINT [UQ_PaySched_Inst]     UNIQUE ([FeeId], [InstallmentNumber]),
    CONSTRAINT [CK_PaySched_Status]   CHECK ([Status] IN (N'معلق', N'مدفوع', N'متأخر', N'ملغي'))
) ON [PRIMARY];

CREATE TABLE [Finance].[ActualPayments] (
    [Id]              INT             PRIMARY KEY IDENTITY(1,1),
    [FeeId]           INT             NOT NULL,
    [Amount]          DECIMAL(18,2)   NOT NULL,
    [PaymentDate]     DATE            NOT NULL DEFAULT CAST(GETDATE() AS DATE),
    [Method]          NVARCHAR(50)    NOT NULL DEFAULT N'كاش',
    [ReceiptNumber]   NVARCHAR(100)   NULL UNIQUE,
    [ReceivedBy]      INT             NOT NULL,
    [Notes]           NVARCHAR(MAX)   NULL,
    [CreatedAt]       DATE       NOT NULL DEFAULT GETDATE(),
    [InstallmentId]   INT             NULL,  
    CONSTRAINT [FK_ActPay_Fee]        FOREIGN KEY ([FeeId])      REFERENCES [Finance].[Fees]([Id]) ON DELETE no action,
    CONSTRAINT [FK_ActPay_RecBy]      FOREIGN KEY ([ReceivedBy]) REFERENCES [Core].[Users]([Id]),
    CONSTRAINT [FK_ActPay_Installment] FOREIGN KEY ([InstallmentId]) REFERENCES [Finance].[PaymentSchedule]([Id]),
    CONSTRAINT [CK_ActPay_Method]     CHECK ([Method] IN (N'كاش', N'تحويل بنكي', N'شيك', N'بطاقة', N'أخرى')),
    CONSTRAINT [CK_ActPay_Date]       CHECK ([PaymentDate] <= CAST(GETDATE() AS DATE))
) ON [PRIMARY];

CREATE TABLE [Finance].[AdminExpenses] (
    [Id]              INT             PRIMARY KEY IDENTITY(1,1),
    [CaseId]          INT             NOT NULL,
    [Description]     NVARCHAR(500)   NOT NULL,
    [Amount]          DECIMAL(18,2)   NOT NULL,
    [ExpenseDate]     DATE            NOT NULL DEFAULT CAST(GETDATE() AS DATE),
    [PaidBy]          INT             NOT NULL,
    [ReceiptPath]     NVARCHAR(1000)  NULL, 
    [Notes]           NVARCHAR(MAX)   NULL,
    [CreatedAt]       DATE      NOT NULL DEFAULT GETDATE(),

    CONSTRAINT [FK_AdminExp_Case]     FOREIGN KEY ([CaseId]) REFERENCES [Legal].[Cases]([Id]) ON DELETE no action,
    CONSTRAINT [FK_AdminExp_PaidBy]   FOREIGN KEY ([PaidBy]) REFERENCES [Core].[Users]([Id]),
    CONSTRAINT [CK_AdminExp_Date]     CHECK ([ExpenseDate] <= CAST(GETDATE() AS DATE))
) ON [PRIMARY];

CREATE TABLE [Core].[Reports] (
    [Id]            INT             PRIMARY KEY IDENTITY(1,1),
    [ReportType]    NVARCHAR(100)   NOT NULL,
    [Title]         NVARCHAR(500)   NOT NULL,        
    [GeneratedBy]   INT             NOT NULL,     
    [GeneratedAt]   DATETIME        NOT NULL DEFAULT GETDATE(),
    [Parameters]    NVARCHAR(MAX)   NULL,           
    [FilePath]      NVARCHAR(1000)  NULL,          

    CONSTRAINT [FK_Reports_GeneratedBy] FOREIGN KEY ([GeneratedBy]) REFERENCES [Core].[Users]([Id]),
    
    CONSTRAINT [CK_Reports_Type]  CHECK ([ReportType] IN (
        N'القضايا', N'الجلسات', N'المالية', N'الأتعاب',
        N'الاستئنافات', N'أداء المحامي', N'الإشعارات', N'أخرى'
    )),
    CONSTRAINT [CK_Reports_Title] CHECK (LEN(LTRIM(RTRIM([Title]))) > 0)
) ON [PRIMARY];
GO





CREATE INDEX IX_Cases_ClientId        ON [Legal].Cases(ClientId);
CREATE INDEX IX_Cases_StatusId        ON [Legal].Cases(StatusId);
CREATE INDEX IX_Cases_IsArchived      ON [Legal].Cases(IsArchived);
CREATE INDEX IX_Cases_CaseNumber      ON [Legal].Cases(CaseNumber);
CREATE INDEX IX_Hearings_CaseId       ON [Legal].Hearings(CaseId);
CREATE INDEX IX_Hearings_DateTime     ON [Legal].Hearings(HearingDateTime);
CREATE INDEX IX_Documents_CaseId      ON [Docs].Documents(CaseId);
CREATE INDEX IX_Documents_IsArchived  ON [Docs].Documents(IsArchived);
CREATE INDEX IX_Notes_Related         ON [Legal].Notes(RelatedTable, RelatedId);
CREATE INDEX IX_Fees_CaseId           ON [Finance].Fees(CaseId);
CREATE INDEX IX_ActualPayments_FeeId  ON [Finance].ActualPayments(FeeId);
CREATE INDEX IX_Appeals_CaseId        ON [Legal].Appeals(CaseId);
CREATE INDEX IX_Users_Email           ON [Core].Users(Email);
CREATE INDEX IX_Clients_NationalId    ON [Legal].Clients(NationalId);


INSERT INTO [Core].[Roles] ([Id], [RoleName], [Permissions]) VALUES
(1, N'مدير',    N'ALL'),
(2, N'محامي',    N'CASES,HEARINGS,DOCUMENTS,FEES,NOTES,APPEALS'),
(3, N'سكرتاريه', N'VIEW_CASES,NOTES,DOCUMENTS_VIEW');
go
INSERT INTO [Lookup].[CaseTypes] ([Id], [TypeName]) VALUES
(1, N'مدني'),
(2, N'جنائي'),
(3, N'تجاري'),
(4, N'اسره'),
(5, N'إداري'),
(7, N'أخرى');
GO
GO
INSERT INTO [Lookup].[CaseStatuses] ([Id], [StatusName], [Color]) VALUES
(1, N'مفتوحة',    '#1D9E75'),
(2, N'معلقة',     '#BA7517'),
(3, N'مغلقة',     '#888780'),
(4, N'الخاسره',   '#E24B4A'),
(5, N'الرابحه',   '#639922'),
(6, N'مؤرشفة',    '#534AB7');
go

CREATE TRIGGER [Core].[trg_LockRolesTable]
ON [Core].[Roles]
INSTEAD OF INSERT, UPDATE, DELETE
AS
BEGIN
    RAISERROR (N'عفواً! جدول الأدوار (Roles) هو جدول بيانات ثابتة ولا يمكن التعديل عليه أو الحذف منه نهائياً.', 16, 1);
    ROLLBACK TRANSACTION;
END;
GO

CREATE TRIGGER [Lookup].[trg_LockCaseTypesTable]
ON [Lookup].[CaseTypes]
INSTEAD OF INSERT, UPDATE, DELETE
AS
BEGIN
    RAISERROR (N'تنبيه: جدول أنواع القضايا يحتوي على بيانات نظام ثابتة. لا يمكن التعديل أو الحذف.', 16, 1);
    ROLLBACK TRANSACTION;
END;
GO

CREATE TRIGGER [Lookup].[trg_LockCaseStatusesTable]
ON [Lookup].[CaseStatuses]
INSTEAD OF INSERT, UPDATE, DELETE
AS
BEGIN
    RAISERROR (N'تنبيه: جدول حالات القضايا يحتوي على بيانات نظام ثابتة. لا يمكن التعديل أو الحذف.', 16, 1);
    ROLLBACK TRANSACTION;
END;
GO



create or alter proc [admin].stp_fullBackup_LegalCaseManagementDB
as
begin
	declare @dbName varchar(100) ='LegalCaseManagementDB';
	declare @path varchar(300) ='C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\Backup\';
	declare @tag varchar(50) =format(getdate() , 'yyyyMMdd_HHmmss')
	declare @FullName nvarchar(450) =@path+@dbName +'_full_'+@tag+'.bak';
	begin try
    DECLARE @BackupName NVARCHAR(200);
SET @BackupName = 'Full Backup of ' + @dbName;
	backup database @dbName
	to disk =@FullName
        with format , init , name = @BackupName, STATS = 10;
    END TRY
    BEGIN CATCH
        PRINT 'Error Number: ' + CAST(ERROR_NUMBER() AS VARCHAR(10));
        PRINT 'Error Message: ' + ERROR_MESSAGE();
    END CATCH
end


go
CREATE OR ALTER PROCEDURE [admin].sp_DiffBackup_LegalCaseManagementDB
AS
BEGIN
    SET NOCOUNT ON;
	declare @dbName varchar(100) ='LegalCaseManagementDB';
	declare @path varchar(300) ='C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\Backup\';
	declare @tag varchar(50) =format(getdate() , 'yyyyMMdd_HHmmss')
	declare @DiffFileName nvarchar(450) =@path+@dbName +'_Diff_'+@tag+'.bak';

    BEGIN TRY
        DECLARE @BackupName NVARCHAR(200);
        SET @BackupName = 'Diff Backup of' + @dbName;
        BACKUP DATABASE @dbName 
        TO DISK = @DiffFileName 
        WITH DIFFERENTIAL, NAME = @BackupName, STATS = 10;

    END TRY
    BEGIN CATCH
        PRINT 'Error Number: ' + CAST(ERROR_NUMBER() AS VARCHAR(10));
        PRINT 'Error Message: ' + ERROR_MESSAGE();
    END CATCH
END
GO
 شايف جدوال النةتس دا انا هستفاد منو اي يعني انا عندي كولم ف كل تيبل بيكتب النوتس فيه 