namespace TyGoTech.Tool.LightweightScriptManager;

public class BuildCommand : CommandExt
{
    public const string CommandName = "build";

    public const string CommandDescription = "Build the latest code quality resources.";

    public BuildCommand()
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

        RuntimeConfig oldConfig;
        try
        {
            oldConfig = await repo.DeserializeConfigAsync();
        }
        catch (FileNotFoundException)
        {
            oldConfig = new();
        }

        var oldMaps = oldConfig.FileMaps.ToDictionary(m => m.RemotePath);
        var config = new RuntimeConfig();
        foreach (var file in repo.EnumerateFiles("*", new EnumerationOptions { RecurseSubdirectories = true }))
        {
            var remotePath = Path.GetRelativePath(repo.FullName, file.FullName).Replace('\\', '/');
            if (remotePath == Constants.RuntimeConfigFileName)
            {
                continue;
            }

            config.FileMaps.Add(oldMaps.TryGetValue(remotePath, out var oldMap) ? oldMap : new(remotePath));
        }

        await config.SerializeConfigAsync(repo);
    }
}