namespace TyGoTech.Tool.LightweightScriptManager;

public static class CommandInvoker
{
    public static async Task<int> InvokeAsync(params string[] args) => await CreateRootCommand().InvokeAsync(args);

    private static RootCommand CreateRootCommand()
    {
        return new()
        {
            new BuildCommand(),
            new InitCommand(),
            new FetchCommand(),
        };
    }
}