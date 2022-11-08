create table [StateMetadata] (
    [EntityName] nvarchar(64) not null,
    [State] int not null,
    [IsUserLocalizedLabel] bit not null,
    [LocalizedLabelLanguageCode] int not null,
    [LocalizedLabel] nvarchar(350),
    primary key([EntityName], [State], [IsUserLocalizedLabel], [LocalizedLabelLanguageCode])
)
