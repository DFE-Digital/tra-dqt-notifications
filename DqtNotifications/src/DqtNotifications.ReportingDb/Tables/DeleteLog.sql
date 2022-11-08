create table [DeleteLog] (
    [Id] bigint identity not null primary key,
    [EntityName] nvarchar(64) not null,
    [RecordId] nvarchar(64) not null,
    [SinkDeleteTime] datetime not null,
    [VersionNumber] bigint not null
)
