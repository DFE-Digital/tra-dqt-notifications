create table [dfeta_induction] (
    [Id] uniqueidentifier not null constraint [EPK[dbo]].[dfeta_induction]]] primary key,
    [SinkCreatedOn] datetime,
    [SinkModifiedOn] datetime,
    [statecode] int,
    [statuscode] int,
    [dfeta_inductionexemptionreason] int,
    [dfeta_outcome] int,
    [dfeta_inductionstatus] int,
    [dfeta_extensionlengthunit] int,
    [dfeta_appealreceived] bit,
    [dfeta_transitionarrangementseligible] bit,
    [owningteam] uniqueidentifier,
    [owningteam_entitytype] nvarchar(128),
    [dfeta_personid] uniqueidentifier,
    [dfeta_personid_entitytype] nvarchar(128),
    [owninguser] uniqueidentifier,
    [owninguser_entitytype] nvarchar(128),
    [createdonbehalfby] uniqueidentifier,
    [createdonbehalfby_entitytype] nvarchar(128),
    [modifiedby] uniqueidentifier,
    [modifiedby_entitytype] nvarchar(128),
    [createdby] uniqueidentifier,
    [createdby_entitytype] nvarchar(128),
    [owningbusinessunit] uniqueidentifier,
    [owningbusinessunit_entitytype] nvarchar(128),
    [modifiedonbehalfby] uniqueidentifier,
    [modifiedonbehalfby_entitytype] nvarchar(128),
    [dfeta_requestedbyid] uniqueidentifier,
    [dfeta_requestedbyid_entitytype] nvarchar(128),
    [ownerid] uniqueidentifier,
    [ownerid_entitytype] nvarchar(128),
    [dfeta_startdate] datetime,
    [dfeta_personidname] nvarchar(160),
    [owneridname] nvarchar(100),
    [importsequencenumber] int,
    [dfeta_outcomedate] datetime,
    [dfeta_numeracypassdate] datetime,
    [dfeta_appealreceivedon] datetime,
    [dfeta_name] nvarchar(100),
    [utcconversiontimezonecode] int,
    [createdbyyominame] nvarchar(100),
    [dfeta_extensionlength] nvarchar(20),
    [modifiedbyname] nvarchar(100),
    [versionnumber] bigint,
    [dfeta_certificatenumber] nvarchar(20),
    [modifiedbyyominame] nvarchar(100),
    [timezoneruleversionnumber] int,
    [owneridtype] nvarchar(4000),
    [createdonbehalfbyyominame] nvarchar(100),
    [dfeta_requestedbyidyominame] nvarchar(450),
    [dfeta_requestedbyidname] nvarchar(160),
    [dfeta_inductionid] uniqueidentifier,
    [owneridyominame] nvarchar(100),
    [modifiedon] datetime,
    [dfeta_personidyominame] nvarchar(450),
    [dfeta_completiondate] datetime,
    [modifiedonbehalfbyyominame] nvarchar(100),
    [createdbyname] nvarchar(100),
    [createdon] datetime,
    [owningbusinessunitname] nvarchar(160),
    [createdonbehalfbyname] nvarchar(100),
    [modifiedonbehalfbyname] nvarchar(100),
    [overriddencreatedon] datetime
)
