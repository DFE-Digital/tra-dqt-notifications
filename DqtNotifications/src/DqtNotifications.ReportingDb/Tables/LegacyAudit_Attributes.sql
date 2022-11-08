create table [LegacyAudit_Attributes] (
    [auditattributeid] uniqueidentifier,
    [auditid] uniqueidentifier,
    [fieldname] nvarchar(255),
    [oldvalue] nvarchar(max),
    [oldvalue_label] nvarchar(2000),
    [oldvalue_type] nvarchar(64),
    [newvalue] nvarchar(max),
    [newvalue_label] nvarchar(2000),
    [newvalue_type] nvarchar(64)
)
