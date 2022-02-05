namespace TyGoTech.Tool.CodeQuality;

using System.Text.Json;

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

    private static async Task<int> ExecuteAsync()
    {
        var repoFolder = new DirectoryInfo(".");
        var rc = repoFolder.Combine(Constants.SettingsFileName);
        if (!rc.Exists)
        {
            await Console.Error.WriteLineAsync(
                $"The settings file {rc} does not exist. " +
                "Either try the 'init' command to initialize the repo or, " +
                "if the repo has already been initialized, try again in the repo root.");

            return 1;
        }

        using var stream = rc.OpenRead();
        var settings = JsonSerializer.Deserialize<CodeQualitySettings>(
            stream,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            })!;

        if (Path.IsPathRooted(settings.SourceFolder) || Path.IsPathFullyQualified(settings.SourceFolder))
        {
            await Console.Error.WriteLineAsync($"The source folder '{settings.SourceFolder}' path must be relative.");
            return 1;
        }

        if (Path.IsPathRooted(settings.TestFolder) || Path.IsPathFullyQualified(settings.TestFolder))
        {
            await Console.Error.WriteLineAsync($"The test folder '{settings.TestFolder}' path must be relative.");
            return 1;
        }

        using var client = new HttpClient();
        if (!await client.TryDownloadFileAsync(
            settings!.ResourcesUri.Append(Constants.RootEditorConfigRemote),
            repoFolder.Combine(Constants.RootEditorConfigLocal).FullName))
        {
            return 1;
        }

        var sourceFolder = new DirectoryInfo(settings.SourceFolder);
        sourceFolder.Create();
        if (!await client.TryDownloadFileAsync(
            settings!.ResourcesUri.Append(Constants.BuildPropsRemote),
            sourceFolder.Combine(Constants.BuildPropsLocal).FullName))
        {
            return 1;
        }

        if (!await client.TryDownloadFileAsync(
            settings!.ResourcesUri.Append(Constants.SharedBuildPropsRemote),
            sourceFolder.Combine(Constants.SharedBuildPropsLocal).FullName))
        {
            return 1;
        }

        var overrideFile = sourceFolder.Combine(Constants.OverrideBuildPropsLocal);
        if (!overrideFile.Exists
            && !await client.TryDownloadFileAsync(
                settings!.ResourcesUri.Append(Constants.OverrideBuildPropsRemote),
                sourceFolder.Combine(Constants.OverrideBuildPropsLocal).FullName))
        {
            return 1;
        }

        if (settings.NoTest)
        {
            Console.WriteLine($"Test resources disabled, skipping.");
            return 0;
        }

        var testFolder = new DirectoryInfo(settings.TestFolder);
        testFolder.Create();
        if (!await client.TryDownloadFileAsync(
            settings!.ResourcesUri.Append(Constants.TestBuildPropsRemote),
            testFolder.Combine(Constants.TestBuildPropsLocal).FullName))
        {
            return 1;
        }

#pragma warning disable IDE0046
        if (!await client.TryDownloadFileAsync(
            settings!.ResourcesUri.Append(Constants.TestEditorConfigRemote),
            testFolder.Combine(Constants.TestEditorConfigLocal).FullName))
        {
            return 1;
        }

        return 0;
    }
}