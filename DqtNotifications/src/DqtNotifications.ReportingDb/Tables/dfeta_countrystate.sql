create table [dfeta_countrystate] (
    [Id] uniqueidentifier not null constraint [EPK[dbo]].[dfeta_countrystate]]] primary key,
    [SinkCreatedOn] datetime,
    [SinkModifiedOn] datetime,
    [statecode] int,
    [statuscode] int,
    [modifiedby] uniqueidentifier,
    [modifiedby_entitytype] nvarchar(128),
    [dfeta_countryid] uniqueidentifier,
    [dfeta_countryid_entitytype] nvarchar(128),
    [organizationid] uniqueidentifier,
    [organizationid_entitytype] nvarchar(128),
    [modifiedonbehalfby] uniqueidentifier,
    [modifiedonbehalfby_entitytype] nvarchar(128),
    [createdonbehalfby] uniqueidentifier,
    [createdonbehalfby_entitytype] nvarchar(128),
    [createdby] uniqueidentifier,
    [createdby_entitytype] nvarchar(128),
    [createdbyyominame] nvarchar(100),
    [organizationidname] nvarchar(100),
    [dfeta_name] nvarchar(250),
    [createdonbehalfbyyominame] nvarchar(100),
    [dfeta_countrystateid] uniqueidentifier,
    [modifiedonbehalfbyyominame] nvarchar(100),
    [modifiedbyyominame] nvarchar(100),
    [createdbyname] nvarchar(100),
    [versionnumber] bigint,
    [timezoneruleversionnumber] int,
    [importsequencenumber] int,
    [modifiedonbehalfbyname] nvarchar(100),
    [modifiedbyname] nvarchar(100),
    [modifiedon] datetime,
    [overriddencreatedon] datetime,
    [dfeta_countryidname] nvarchar(250),
    [utcconversiontimezonecode] int,
    [createdonbehalfbyname] nvarchar(100),
    [createdon] datetime,
    [dfeta_value] nvarchar(20)
)
