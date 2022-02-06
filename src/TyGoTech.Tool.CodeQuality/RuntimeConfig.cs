namespace TyGoTech.Tool.CodeQuality;

using System.Text.Json.Serialization;

public class RuntimeConfig
{
#pragma warning disable CA1822
    [JsonPropertyName("$schema")]
    public Uri Schema => new("https://raw.githubusercontent.com/TyGoTech/code.quality-tool-dotnet/main/resources/schema.json");

    public Uri? ResourcesUri { get; set; }

    public IList<ResourceMap> FileMaps { get; } = new List<ResourceMap>();

    public void EnsureValid()
    {
        foreach (var map in this.FileMaps)
        {
            map.EnsureValid();
        }
    }
}