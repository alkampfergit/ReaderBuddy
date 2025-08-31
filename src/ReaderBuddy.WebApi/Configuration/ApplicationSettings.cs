namespace ReaderBuddy.WebApi.Configuration;

public class ApplicationSettings
{
    public const string SectionName = "ApplicationSettings";

    public string Name { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public bool EnableDetailedErrors { get; set; }
}