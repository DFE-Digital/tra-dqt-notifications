create table [campaign] (
    [Id] uniqueidentifier not null constraint [EPK[dbo]].[campaign]]] primary key,
    [SinkCreatedOn] datetime,
    [SinkModifiedOn] datetime,
    [statecode] int,
    [statuscode] int,
    [typecode] int,
    [msdyn_gdproptout] bit,
    [istemplate] bit,
    [createdby] uniqueidentifier,
    [createdby_entitytype] nvarchar(128),
    [modifiedonbehalfby] uniqueidentifier,
    [modifiedonbehalfby_entitytype] nvarchar(128),
    [transactioncurrencyid] uniqueidentifier,
    [transactioncurrencyid_entitytype] nvarchar(128),
    [createdonbehalfby] uniqueidentifier,
    [createdonbehalfby_entitytype] nvarchar(128),
    [pricelistid] uniqueidentifier,
    [pricelistid_entitytype] nvarchar(128),
    [owningteam] uniqueidentifier,
    [owningteam_entitytype] nvarchar(128),
    [owningbusinessunit] uniqueidentifier,
    [owningbusinessunit_entitytype] nvarchar(128),
    [modifiedby] uniqueidentifier,
    [modifiedby_entitytype] nvarchar(128),
    [owninguser] uniqueidentifier,
    [owninguser_entitytype] nvarchar(128),
    [ownerid] uniqueidentifier,
    [ownerid_entitytype] nvarchar(128),
    [expectedrevenue_base] decimal(38, 4),
    [othercost] decimal(38, 2),
    [totalcampaignactivityactualcost] decimal(38, 2),
    [expectedrevenue] decimal(38, 2),
    [totalactualcost] decimal(38, 2),
    [budgetedcost] decimal(38, 2),
    [totalactualcost_base] decimal(38, 4),
    [totalcampaignactivityactualcost_base] decimal(38, 4),
    [budgetedcost_base] decimal(38, 4),
    [othercost_base] decimal(38, 4),
    [entityimage_timestamp] bigint,
    [createdonbehalfbyname] nvarchar(100),
    [timezoneruleversionnumber] int,
    [tmpregardingobjectid] nvarchar(100),
    [owneridtype] nvarchar(4000),
    [versionnumber] bigint,
    [promotioncodename] nvarchar(128),
    [stageid] uniqueidentifier,
    [actualstart] datetime,
    [expectedresponse] int,
    [transactioncurrencyidname] nvarchar(100),
    [modifiedbyyominame] nvarchar(100),
    [message] nvarchar(256),
    [description] nvarchar(max),
    [entityimageid] uniqueidentifier,
    [emailaddress] nvarchar(100),
    [importsequencenumber] int,
    [createdon] datetime,
    [createdbyyominame] nvarchar(100),
    [codename] nvarchar(32),
    [campaignid] uniqueidentifier,
    [overriddencreatedon] datetime,
    [modifiedonbehalfbyyominame] nvarchar(100),
    [actualend] datetime,
    [traversedpath] nvarchar(1250),
    [entityimage_url] nvarchar(200),
    [proposedstart] datetime,
    [modifiedonbehalfbyname] nvarchar(100),
    [owneridyominame] nvarchar(100),
    [processid] uniqueidentifier,
    [createdonbehalfbyyominame] nvarchar(100),
    [createdbyname] nvarchar(100),
    [utcconversiontimezonecode] int,
    [modifiedbyname] nvarchar(100),
    [proposedend] datetime,
    [modifiedon] datetime,
    [pricelistname] nvarchar(100),
    [owningbusinessunitname] nvarchar(160),
    [exchangerate] decimal(38, 10),
    [owneridname] nvarchar(100),
    [name] nvarchar(128),
    [objective] nvarchar(max)
)
