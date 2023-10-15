using Patterns.Abstractions;

namespace Patterns.Commands;

public class ListCommand : ICommand
{
    public void Execute()
    {
        Command.PrintCommandList();
    }
}
