namespace TyGoTech.Tool.CodeQuality;

using System.IO;
using System.Text.Json;

public class InitCommand : CommandExt
{
    public const string CommandName = "init";

    public const string CommandDescription = "Initialize code quality bindings for repository";

    public static readonly IReadOnlyList<Option> CommandOptions = new Option[]
    {
        new Option(
            new[] { "--force", "-f" },
            "Overwrites the current settings file if one exists."),
        new Option<DirectoryInfo>(
            new[] { "--repo-root-folder", "-r" },
            () => new DirectoryInfo(CodeQualitySettings.Default.RepoRootFolder),
            "The repository root folder.")
            .LegalFileNamesOnly(),
        new Option<DirectoryInfo>(
            new[] { "--source-folder", "-s" },
            () => new DirectoryInfo(CodeQualitySettings.Default.SourceFolder),
            "The source folder.")
            .LegalFileNamesOnly(),
        new Option<DirectoryInfo>(
            new[] { "--test-folder", "-t" },
            () => new DirectoryInfo(CodeQualitySettings.Default.TestFolder),
            "The test folder.")
            .LegalFileNamesOnly(),
    };

    public InitCommand()
        : base(
            CommandName,
            CommandDescription,
            CommandOptions,
            CommandHandler.Create<bool, DirectoryInfo, DirectoryInfo, DirectoryInfo>(Execute))
    {
    }

    public static int Execute(
        bool force,
        DirectoryInfo repoRootFolder,
        DirectoryInfo sourceFolder,
        DirectoryInfo testFolder)
    {
        if (!repoRootFolder.Exists)
        {
            Console.WriteLine($"The repo root folder {repoRootFolder} does not exist.");
            return 1;
        }

        var settings = new CodeQualitySettings
        {
            RepoRootFolder = repoRootFolder.ToString(),
            SourceFolder = sourceFolder.ToString(),
            TestFolder = testFolder.ToString(),
        };

        var rc = Path.Combine(repoRootFolder.FullName, Constants.SettingsFileName);
        if (force)
        {
            File.Delete(rc);
        }

        if (File.Exists(rc))
        {
            Console.WriteLine($"The settings file {rc} already exists (use the '--force' Luke).");
            return 1;
        }

        using var stream = File.OpenWrite(rc);
        JsonSerializer.Serialize(
            stream,
            settings,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
            });

        return 0;
    }
}