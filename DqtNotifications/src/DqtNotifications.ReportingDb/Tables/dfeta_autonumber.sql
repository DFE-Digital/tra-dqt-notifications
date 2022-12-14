create table [dfeta_autonumber] (
    [Id] uniqueidentifier not null constraint [EPK[dbo]].[dfeta_autonumber]]] primary key,
    [SinkCreatedOn] datetime,
    [SinkModifiedOn] datetime,
    [statecode] int,
    [statuscode] int,
    [dfeta_operation] int,
    [dfeta_overrideexistingnumber] bit,
    [organizationid] uniqueidentifier,
    [organizationid_entitytype] nvarchar(128),
    [modifiedby] uniqueidentifier,
    [modifiedby_entitytype] nvarchar(128),
    [createdonbehalfby] uniqueidentifier,
    [createdonbehalfby_entitytype] nvarchar(128),
    [createdby] uniqueidentifier,
    [createdby_entitytype] nvarchar(128),
    [modifiedonbehalfby] uniqueidentifier,
    [modifiedonbehalfby_entitytype] nvarchar(128),
    [dfeta_next] int,
    [dfeta_to] int,
    [dfeta_autonumberid] uniqueidentifier,
    [versionnumber] bigint,
    [modifiedonbehalfbyname] nvarchar(100),
    [dfeta_name] nvarchar(100),
    [organizationidname] nvarchar(100),
    [overriddencreatedon] datetime,
    [modifiedonbehalfbyyominame] nvarchar(100),
    [createdonbehalfbyname] nvarchar(100),
    [modifiedbyname] nvarchar(100),
    [modifiedon] datetime,
    [dfeta_attribute] nvarchar(100),
    [createdbyname] nvarchar(100),
    [utcconversiontimezonecode] int,
    [modifiedbyyominame] nvarchar(100),
    [createdonbehalfbyyominame] nvarchar(100),
    [dfeta_from] int,
    [createdon] datetime,
    [timezoneruleversionnumber] int,
    [dfeta_updateofattribute] nvarchar(100),
    [importsequencenumber] int,
    [createdbyyominame] nvarchar(100)
)
