create table [contactleads] (
    [Id] uniqueidentifier not null constraint [EPK[dbo]].[contactleads]]] primary key,
    [SinkCreatedOn] datetime,
    [SinkModifiedOn] datetime,
    [leadid] uniqueidentifier,
    [contactleadid] uniqueidentifier,
    [overriddencreatedon] datetime,
    [name] nvarchar(100),
    [timezoneruleversionnumber] int,
    [contactid] uniqueidentifier,
    [versionnumber] bigint,
    [importsequencenumber] int,
    [utcconversiontimezonecode] int
)
