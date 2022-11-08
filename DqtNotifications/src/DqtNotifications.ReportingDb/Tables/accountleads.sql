create table [accountleads] (
    [Id] uniqueidentifier not null constraint [EPK[dbo]].[accountleads]]] primary key,
    [SinkCreatedOn] datetime,
    [SinkModifiedOn] datetime,
    [leadid] uniqueidentifier,
    [accountleadid] uniqueidentifier,
    [overriddencreatedon] datetime,
    [accountid] uniqueidentifier,
    [name] nvarchar(100),
    [timezoneruleversionnumber] int,
    [versionnumber] bigint,
    [importsequencenumber] int,
    [utcconversiontimezonecode] int
)
