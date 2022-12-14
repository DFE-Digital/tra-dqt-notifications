create table [dfeta_optionsetmapping] (
    [Id] uniqueidentifier not null constraint [EPK[dbo]].[dfeta_optionsetmapping]]] primary key,
    [SinkCreatedOn] datetime,
    [SinkModifiedOn] datetime,
    [statecode] int,
    [statuscode] int,
    [modifiedby] uniqueidentifier,
    [modifiedby_entitytype] nvarchar(128),
    [modifiedonbehalfby] uniqueidentifier,
    [modifiedonbehalfby_entitytype] nvarchar(128),
    [createdonbehalfby] uniqueidentifier,
    [createdonbehalfby_entitytype] nvarchar(128),
    [organizationid] uniqueidentifier,
    [organizationid_entitytype] nvarchar(128),
    [createdby] uniqueidentifier,
    [createdby_entitytype] nvarchar(128),
    [createdbyyominame] nvarchar(100),
    [organizationidname] nvarchar(100),
    [dfeta_optionsetmappingname] nvarchar(100),
    [createdonbehalfbyyominame] nvarchar(100),
    [modifiedonbehalfbyyominame] nvarchar(100),
    [modifiedbyyominame] nvarchar(100),
    [createdbyname] nvarchar(100),
    [dfeta_exportcode] nvarchar(100),
    [dfeta_crmcode] int,
    [timezoneruleversionnumber] int,
    [importsequencenumber] int,
    [modifiedonbehalfbyname] nvarchar(100),
    [dfeta_optionsetmappingid] uniqueidentifier,
    [modifiedbyname] nvarchar(100),
    [versionnumber] bigint,
    [modifiedon] datetime,
    [overriddencreatedon] datetime,
    [utcconversiontimezonecode] int,
    [createdonbehalfbyname] nvarchar(100),
    [createdon] datetime
)
