create table [SQLCountHistory] (
    [Id] bigint identity not null primary key,
    [EntityName] nvarchar(64) not null,
    [Count] bigint not null,
    [UpdateTime] datetime not null
)
