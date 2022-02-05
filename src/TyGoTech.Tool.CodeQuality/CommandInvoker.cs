namespace TyGoTech.Tool.CodeQuality;

public static class CommandInvoker
{
    public static async Task<int> InvokeAsync(params string[] args) => await CreateRootCommand().InvokeAsync(args);

    private static RootCommand CreateRootCommand()
    {
        return new()
        {
            new InitCommand(),
            new FetchCommand(),
        };
    }
}