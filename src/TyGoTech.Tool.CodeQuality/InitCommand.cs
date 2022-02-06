namespace TyGoTech.Tool.CodeQuality;

using System.IO;

public class InitCommand : CommandExt
{
    private const string CommandName = "init";

    private const string CommandDescription = "Initialize code quality bindings for repository";

    private static readonly IReadOnlyList<Option> CommandOptions = new Option[]
    {
        new Option(
            new[] { "--force", "-f" },
            "Overwrites the current runtime config file if one exists."),
        new Option<Uri>(
            new[] { "--resources-uri", "-u" },
            () => Constants.DefaultResourcesUri,
            "The base URI that hosts the config files."),
    };

    private static readonly ResourceMap RuntimeConfigMap = new("codequalityrc.json", "codequalityrc.json", false);

    public InitCommand()
        : base(
            CommandName,
            CommandDescription,
            CommandOptions,
            CommandHandler.Create<bool, Uri>(ExecuteAsync))
    {
    }

    public static async Task ExecuteAsync(bool force, Uri resourcesUri)
    {
        var repo = new DirectoryInfo("./");
        var runtimeConfig = RuntimeConfigMap.GetLocal(repo);
        if (runtimeConfig.Exists && !force)
        {
            throw new InvalidOperationException(
                $"The runtime config file {runtimeConfig} already exists (use the --force Luke).");
        }

        using var downloader = new ResourceMapDownloader(resourcesUri, repo);
        await downloader.DownloadAsync(RuntimeConfigMap);

        var settings = await repo.DeserializeConfigAsync();
        settings.ResourcesUri = resourcesUri;
        await settings.SerializeConfigAsync(repo);
    }
}