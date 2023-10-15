using Patterns.Abstractions;

namespace Patterns.Commands;

public class NullCommand : ICommand
{
    public void Execute()
    {
        Command.PrintFullHelp();
    }
}
