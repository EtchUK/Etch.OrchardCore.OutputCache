using OrchardCore.Modules.Manifest;

[assembly: Module(
    Author = "Etch UK Ltd.",
    Category = "Performance",
    Description = "Provides caching using Output Cache.",
    Name = "Output Caching",
    Version = "0.0.1",
    Website = "https://etchuk.com"
)]

[assembly: Feature(
    Id = "Etch.OrchardCore.OutputCache",
    Name = "Output Caching",
    Description = "Provides caching using Output Cache..",
    Category = "Performance"
)]

[assembly: Feature(
    Id = "Etch.OrchardCore.OutputCache.Redis",
    Name = "Redis Output Caching",
    Description = "Output cache storage using Redis.",
    Dependencies = new[]
    {
        "OrchardCore.Redis"
    },
    Category = "Performance"
)]