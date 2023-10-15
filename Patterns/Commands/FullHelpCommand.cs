using Patterns.Abstractions;

namespace Patterns.Commands;

public class FullHelpCommand : ICommand
{
    public void Execute()
    {
        Command.PrintFullHelp();
    }
}
