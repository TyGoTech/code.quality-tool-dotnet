namespace TyGoTech.Tool.CodeQuality;

using System.Text.Json.Serialization;

public record ResourceMap(
    string RemotePath,
    string? LocalPath = default,
    bool Preserve = false)
{
    [JsonIgnore]
    public string DerivedLocalPath => this.LocalPath ?? this.RemotePath;

    public Uri GetRemote(Uri resourcesUri) => resourcesUri.Append(this.RemotePath);

    public FileInfo GetLocal(DirectoryInfo repoRoot) => repoRoot.Combine(this.DerivedLocalPath);

    public void EnsureValid()
    {
        if (Path.IsPathRooted(this.DerivedLocalPath) || Path.IsPathFullyQualified(this.DerivedLocalPath))
        {
            throw new InvalidOperationException($"The the local resource path must be relative. {this}.");
        }
    }
}