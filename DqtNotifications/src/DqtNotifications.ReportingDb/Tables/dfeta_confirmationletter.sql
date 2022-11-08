create table [dfeta_confirmationletter] (
    [Id] uniqueidentifier not null constraint [EPK[dbo]].[dfeta_confirmationletter]]] primary key,
    [SinkCreatedOn] datetime,
    [SinkModifiedOn] datetime,
    [statecode] int,
    [statuscode] int,
    [owningteam] uniqueidentifier,
    [owningteam_entitytype] nvarchar(128),
    [owninguser] uniqueidentifier,
    [owninguser_entitytype] nvarchar(128),
    [modifiedby] uniqueidentifier,
    [modifiedby_entitytype] nvarchar(128),
    [createdby] uniqueidentifier,
    [createdby_entitytype] nvarchar(128),
    [dfeta_personid] uniqueidentifier,
    [dfeta_personid_entitytype] nvarchar(128),
    [owningbusinessunit] uniqueidentifier,
    [owningbusinessunit_entitytype] nvarchar(128),
    [modifiedonbehalfby] uniqueidentifier,
    [modifiedonbehalfby_entitytype] nvarchar(128),
    [createdonbehalfby] uniqueidentifier,
    [createdonbehalfby_entitytype] nvarchar(128),
    [ownerid] uniqueidentifier,
    [ownerid_entitytype] nvarchar(128),
    [modifiedonbehalfbyyominame] nvarchar(100),
    [dfeta_securitynumber] nvarchar(50),
    [versionnumber] bigint,
    [owningbusinessunitname] nvarchar(160),
    [modifiedonbehalfbyname] nvarchar(100),
    [owneridname] nvarchar(100),
    [overriddencreatedon] datetime,
    [dfeta_personidyominame] nvarchar(450),
    [createdonbehalfbyname] nvarchar(100),
    [modifiedbyname] nvarchar(100),
    [dfeta_personidname] nvarchar(160),
    [dfeta_confirmationletterid] uniqueidentifier,
    [modifiedon] datetime,
    [createdbyname] nvarchar(100),
    [utcconversiontimezonecode] int,
    [modifiedbyyominame] nvarchar(100),
    [createdonbehalfbyyominame] nvarchar(100),
    [createdon] datetime,
    [timezoneruleversionnumber] int,
    [owneridtype] nvarchar(4000),
    [importsequencenumber] int,
    [createdbyyominame] nvarchar(100),
    [owneridyominame] nvarchar(100)
)
