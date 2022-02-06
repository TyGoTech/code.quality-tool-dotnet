namespace TyGoTech.Tool.LightweightScriptManager;

public abstract class CommandExt : Command
{
    protected CommandExt(
        string name,
        string description,
        IEnumerable<Option> options,
        ICommandHandler handler)
        : base(name, description)
    {
        foreach (var option in options)
        {
            this.AddOption(option);
        }

        this.Handler = handler;
    }
}