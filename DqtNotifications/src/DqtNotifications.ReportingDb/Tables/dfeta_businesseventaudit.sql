create table [dfeta_businesseventaudit] (
    [Id] uniqueidentifier not null constraint [EPK[dbo]].[dfeta_businesseventaudit]]] primary key,
    [SinkCreatedOn] datetime,
    [SinkModifiedOn] datetime,
    [statecode] int,
    [statuscode] int,
    [dfeta_event] int,
    [owningbusinessunit] uniqueidentifier,
    [owningbusinessunit_entitytype] nvarchar(128),
    [owninguser] uniqueidentifier,
    [owninguser_entitytype] nvarchar(128),
    [modifiedby] uniqueidentifier,
    [modifiedby_entitytype] nvarchar(128),
    [createdby] uniqueidentifier,
    [createdby_entitytype] nvarchar(128),
    [owningteam] uniqueidentifier,
    [owningteam_entitytype] nvarchar(128),
    [dfeta_person] uniqueidentifier,
    [dfeta_person_entitytype] nvarchar(128),
    [modifiedonbehalfby] uniqueidentifier,
    [modifiedonbehalfby_entitytype] nvarchar(128),
    [createdonbehalfby] uniqueidentifier,
    [createdonbehalfby_entitytype] nvarchar(128),
    [ownerid] uniqueidentifier,
    [ownerid_entitytype] nvarchar(128),
    [dfeta_datamigration] nvarchar(100),
    [dfeta_businesseventauditid] uniqueidentifier,
    [modifiedonbehalfbyyominame] nvarchar(100),
    [versionnumber] bigint,
    [owningbusinessunitname] nvarchar(160),
    [modifiedonbehalfbyname] nvarchar(100),
    [owneridname] nvarchar(100),
    [overriddencreatedon] datetime,
    [createdonbehalfbyname] nvarchar(100),
    [modifiedbyname] nvarchar(100),
    [dfeta_newvalue] nvarchar(250),
    [dfeta_changedfield] nvarchar(100),
    [modifiedon] datetime,
    [createdbyname] nvarchar(100),
    [utcconversiontimezonecode] int,
    [modifiedbyyominame] nvarchar(100),
    [dfeta_personname] nvarchar(160),
    [createdonbehalfbyyominame] nvarchar(100),
    [createdon] datetime,
    [timezoneruleversionnumber] int,
    [owneridtype] nvarchar(4000),
    [importsequencenumber] int,
    [dfeta_oldvalue] nvarchar(250),
    [dfeta_personyominame] nvarchar(450),
    [createdbyyominame] nvarchar(100),
    [owneridyominame] nvarchar(100)
)
