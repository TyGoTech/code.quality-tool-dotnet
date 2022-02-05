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
    };

    public InitCommand()
        : base(
            CommandName,
            CommandDescription,
            CommandOptions,
            CommandHandler.Create<bool>(ExecuteAsync))
    {
    }

    public static async Task<int> ExecuteAsync(bool force)
    {
        var repoFolder = new DirectoryInfo("./");
        var settings = new CodeQualitySettings();

        var rc = Path.Combine(repoFolder.FullName, Constants.SettingsFileName);
        if (force)
        {
            File.Delete(rc);
        }

        if (File.Exists(rc))
        {
            await Console.Error.WriteLineAsync($"The settings file {rc} already exists (use the --force Luke).");
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