create table [dfeta_sanctioncode] (
    [Id] uniqueidentifier not null constraint [EPK[dbo]].[dfeta_sanctioncode]]] primary key,
    [SinkCreatedOn] datetime,
    [SinkModifiedOn] datetime,
    [statecode] int,
    [statuscode] int,
    [dfeta_allreports] bit,
    [dfeta_individualteacherreport] bit,
    [dfeta_section128barringdirectionreport] bit,
    [dfeta_restrictportalalertsharing] bit,
    [dfeta_inductionfailreport] bit,
    [dfeta_othereeamemberstatereport] bit,
    [dfeta_blockselfserviceaccess] bit,
    [dfeta_prohibitedteacherreport] bit,
    [dfeta_gtcsanctionreport] bit,
    [createdonbehalfby] uniqueidentifier,
    [createdonbehalfby_entitytype] nvarchar(128),
    [modifiedonbehalfby] uniqueidentifier,
    [modifiedonbehalfby_entitytype] nvarchar(128),
    [createdby] uniqueidentifier,
    [createdby_entitytype] nvarchar(128),
    [organizationid] uniqueidentifier,
    [organizationid_entitytype] nvarchar(128),
    [modifiedby] uniqueidentifier,
    [modifiedby_entitytype] nvarchar(128),
    [modifiedbyyominame] nvarchar(100),
    [createdbyname] nvarchar(100),
    [modifiedon] datetime,
    [dfeta_sanctioncodeid] uniqueidentifier,
    [modifiedbyname] nvarchar(100),
    [createdonbehalfbyyominame] nvarchar(100),
    [modifiedonbehalfbyname] nvarchar(100),
    [importsequencenumber] int,
    [createdon] datetime,
    [modifiedonbehalfbyyominame] nvarchar(100),
    [versionnumber] bigint,
    [overriddencreatedon] datetime,
    [createdonbehalfbyname] nvarchar(100),
    [timezoneruleversionnumber] int,
    [createdbyyominame] nvarchar(100),
    [organizationidname] nvarchar(100),
    [dfeta_name] nvarchar(450),
    [dfeta_value] nvarchar(20),
    [utcconversiontimezonecode] int
)
