namespace TyGoTech.Tool.CodeQuality;

public static class Constants
{
    public const string RuntimeConfigFileName = "codequalityrc.json";

    public static readonly Uri DefaultResourcesUri = new(
        "https://raw.githubusercontent.com/TyGoTech/code.quality-tool-dotnet/main/resources/dotnet/");

    public static readonly Uri RuntimeConfigSchemaUri = new(
        "http://raw.githubusercontent.com/TyGoTech/code.quality-tool-dotnet/main/resources/schema.json");
}