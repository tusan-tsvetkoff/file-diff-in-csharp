using Patterns.Abstractions;
using Patterns.Enums;
using Patterns.Extensions;

namespace Patterns.Commands;

public class CommandInvoker
{
    private ICommand _command = new NullCommand();
    public void SetCommand(ICommand command)
    {
        _command = command;
    }

    public void ExecuteCommand()
    {
        _command.Execute();
    }
}
