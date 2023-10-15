using Patterns.Abstractions;

namespace Patterns.Commands;

public class CmdHelpCommand : ICommand
{
    private readonly string _cmdTarget;
    public CmdHelpCommand(string cmdTarget)
    {
        _cmdTarget = cmdTarget;
    }
    public void Execute()
    {
        Command.PrintCommandHelp(_cmdTarget);
    }
}
