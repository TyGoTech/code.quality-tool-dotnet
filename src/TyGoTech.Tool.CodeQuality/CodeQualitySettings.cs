namespace TyGoTech.Tool.CodeQuality;

public class CodeQualitySettings
{
    public static readonly CodeQualitySettings Default = new();

    public string SourceFolder { get; init; } = "src/";

    public string TestFolder { get; init; } = "src/test/";

    public Uri ResourcesUri { get; init; } = new("https://raw.githubusercontent.com/TyGoTech/code.quality-tool-dotnet/main/resources/");

    public bool NoTest { get; init; }
}