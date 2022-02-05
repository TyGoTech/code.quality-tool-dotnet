namespace TyGoTech.Tool.CodeQuality;

public class CodeQualitySettings
{
    public static readonly CodeQualitySettings Default = new();

    public string SourceFolder { get; set; } = "src/";

    public string TestFolder { get; set; } = "src/test/";

    public Uri ResourcesUri { get; set; } = new("https://raw.githubusercontent.com/TyGoTech/code.quality-tool-dotnet/main/resources/");

    public bool NoTest { get; set; }
}