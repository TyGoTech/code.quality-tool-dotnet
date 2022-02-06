namespace TyGoTech.Tool.LightweightScriptManager;

public class FetchCommand : CommandExt
{
    public const string CommandName = "fetch";

    public const string CommandDescription = "Fetch the latest resources using the runtime config settings.";

    public FetchCommand()
    : base(
        CommandName,
        CommandDescription,
        Array.Empty<Option>(),
        CommandHandler.Create(ExecuteAsync))
    {
    }

    private static async Task ExecuteAsync()
    {
        var repo = new DirectoryInfo(".");

        var config = await repo.DeserializeConfigAsync();
        config.EnsureValid();

        using var downloader = new ResourceDownloader(config.PackageUri!, repo);
        foreach (var map in config.FileMaps)
        {
            await downloader.DownloadAsync(map);
        }
    }
}