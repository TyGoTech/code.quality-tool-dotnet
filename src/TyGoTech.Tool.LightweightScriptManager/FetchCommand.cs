namespace TyGoTech.Tool.LightweightScriptManager;

public class FetchCommand : CommandExt
{
    public const string CommandName = "fetch";

    public const string CommandDescription = "Fetch the latest code quality resources.";

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

        using var downloader = new ResourceMapDownloader(config.ResourcesUri!, repo);
        foreach (var map in config.FileMaps)
        {
            await downloader.DownloadAsync(map);
        }
    }
}