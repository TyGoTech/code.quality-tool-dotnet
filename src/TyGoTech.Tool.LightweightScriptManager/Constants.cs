namespace TyGoTech.Tool.LightweightScriptManager;

public static class Constants
{
    public const string RuntimeConfigFileName = "lsmrc.json";

    public static readonly Uri DefaultResourcesUri = new(
        "https://raw.githubusercontent.com/TyGoTech/tool-lightweight.script.manager/main/resources/dotnet/");

    public static readonly Uri RuntimeConfigSchemaUri = new(
        "http://raw.githubusercontent.com/TyGoTech/tool-lightweight.script.manager/main/resources/schema.json");
}