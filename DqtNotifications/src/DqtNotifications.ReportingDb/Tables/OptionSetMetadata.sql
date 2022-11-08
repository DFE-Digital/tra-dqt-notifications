create table [OptionSetMetadata] (
    [EntityName] nvarchar(64) not null,
    [OptionSetName] nvarchar(64) not null,
    [Option] int not null,
    [IsUserLocalizedLabel] bit not null,
    [LocalizedLabelLanguageCode] int not null,
    [LocalizedLabel] nvarchar(350),
    primary key([EntityName], [OptionSetName], [Option], [IsUserLocalizedLabel], [LocalizedLabelLanguageCode])
)
