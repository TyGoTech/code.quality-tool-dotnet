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
            new[] { "--source-folder", "-s" },
            () => new DirectoryInfo(CodeQualitySettings.Default.SourceFolder),
            "The relative path to the source folder.")
            .LegalFileNamesOnly(),
        new Option<DirectoryInfo>(
            new[] { "--test-folder", "-t" },
            () => new DirectoryInfo(CodeQualitySettings.Default.TestFolder),
            "The relative path to the test folder.")
            .LegalFileNamesOnly(),
    };

    public InitCommand()
        : base(
            CommandName,
            CommandDescription,
            CommandOptions,
            CommandHandler.Create<bool, DirectoryInfo, DirectoryInfo>(Execute))
    {
    }

    public static int Execute(
        bool force,
        DirectoryInfo sourceFolder,
        DirectoryInfo testFolder)
    {
        var repoFolder = new DirectoryInfo("./");
        var settings = new CodeQualitySettings
        {
            SourceFolder = sourceFolder.ToString(),
            TestFolder = testFolder.ToString(),
        };

        if (Path.IsPathRooted(settings.SourceFolder) || Path.IsPathFullyQualified(settings.SourceFolder))
        {
            Console.WriteLine($"The source folder '{settings.SourceFolder}' path must be relative.");
            return 1;
        }

        if (Path.IsPathRooted(settings.TestFolder) || Path.IsPathFullyQualified(settings.TestFolder))
        {
            Console.WriteLine($"The test folder '{settings.TestFolder}' path must be relative.");
            return 1;
        }

        var rc = Path.Combine(repoFolder.FullName, Constants.SettingsFileName);
        if (force)
        {
            File.Delete(rc);
        }

        if (File.Exists(rc))
        {
            Console.WriteLine($"The settings file {rc} already exists (use the --force Luke).");
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