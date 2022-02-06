namespace TyGoTech.Tool.LightweightScriptManager;

using System.Text.Json;
using System.Text.Json.Serialization;

public static class ExtensionMethods
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
        WriteIndented = true,
    };

    public static async Task<RuntimeConfig> DeserializeConfigAsync(this DirectoryInfo repoFolder)
    {
        var runtimeConfig = repoFolder.Combine(Constants.RuntimeConfigFileName);
        if (!runtimeConfig.Exists)
        {
            throw new FileNotFoundException(
                $"The runtime config file {runtimeConfig} does not exist. " +
                "Either use the 'init' command to initialize the repo or, " +
                "if the repo has already been initialized, try again in the repo root.",
                runtimeConfig.FullName);
        }

        using var stream = runtimeConfig.OpenRead();
        return (await JsonSerializer.DeserializeAsync<RuntimeConfig>(stream, JsonSerializerOptions))!;
    }

    public static async Task SerializeConfigAsync(this RuntimeConfig config, DirectoryInfo repoFolder)
    {
        var runtimeConfig = repoFolder.Combine(Constants.RuntimeConfigFileName);
        using var stream = runtimeConfig.Open(FileMode.Create, FileAccess.Write, FileShare.None);
        await JsonSerializer.SerializeAsync(stream, config, JsonSerializerOptions);
    }

    public static Uri Append(this Uri uri, params string[] paths)
    {
        return new Uri(
            paths.Aggregate(
                uri.AbsoluteUri,
                (current, path) => $"{current.TrimEnd('/')}/{path.TrimStart('/')}"));
    }

    public static FileInfo Combine(this DirectoryInfo directory, string path) =>
        new(Path.Combine(directory.FullName, path));
}