create table [TargetMetadata] (
    [EntityName] nvarchar(64) not null,
    [AttributeName] nvarchar(64) not null,
    [ReferencedEntity] nvarchar(64) not null,
    [ReferencedAttribute] nvarchar(64),
    primary key([EntityName], [AttributeName], [ReferencedEntity])
)
