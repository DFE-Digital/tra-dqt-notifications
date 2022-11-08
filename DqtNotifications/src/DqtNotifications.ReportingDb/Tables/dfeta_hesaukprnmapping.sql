create table [dfeta_hesaukprnmapping] (
    [Id] uniqueidentifier not null constraint [EPK[dbo]].[dfeta_hesaukprnmapping]]] primary key,
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
    [createdonbehalfbyyominame] nvarchar(100),
    [modifiedonbehalfbyyominame] nvarchar(100),
    [modifiedbyyominame] nvarchar(100),
    [createdbyname] nvarchar(100),
    [dfeta_heestabukprn] nvarchar(8),
    [versionnumber] bigint,
    [timezoneruleversionnumber] int,
    [dfeta_hesaukprnmappingid] uniqueidentifier,
    [importsequencenumber] int,
    [modifiedonbehalfbyname] nvarchar(100),
    [dfeta_crmheestabukprn] int,
    [modifiedbyname] nvarchar(100),
    [modifiedon] datetime,
    [overriddencreatedon] datetime,
    [utcconversiontimezonecode] int,
    [createdonbehalfbyname] nvarchar(100),
    [createdon] datetime,
    [dfeta_value] nvarchar(4)
)
