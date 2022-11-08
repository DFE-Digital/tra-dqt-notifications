create table [GlobalOptionSetMetadata] (
    [OptionSetName] nvarchar(64) not null,
    [Option] int not null,
    [IsUserLocalizedLabel] bit not null,
    [LocalizedLabelLanguageCode] int not null,
    [LocalizedLabel] nvarchar(350),
    primary key([OptionSetName], [Option], [IsUserLocalizedLabel], [LocalizedLabelLanguageCode])
)
