create table [LegacyAudit] (
    [auditid] uniqueidentifier,
    [createdon] datetime,
    [actionname] nvarchar(30),
    [operationname] nvarchar(30),
    [userid] uniqueidentifier,
    [useridname] nvarchar(200),
    [objecttypecode] nvarchar(64),
    [objecttypecodename] nvarchar(100),
    [objectid] uniqueidentifier,
    [objectidname] nvarchar(160)
)
