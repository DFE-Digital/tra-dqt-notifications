create table [AttributeMetadata] (
    [Id] bigint identity not null primary key,
    [EntityName] nvarchar(64) not null,
    [AttributeName] nvarchar(64) not null,
    [AttributeType] nvarchar(64) not null,
    [AttributeTypeCode] int not null,
    [Version] bigint,
    [Timestamp] datetime,
    [MetadataId] nvarchar(64),
    [Precision] int
)
