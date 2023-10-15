using System.Text;
using Patterns.Abstractions;

namespace Patterns.Commands;

public class UnrecognizedCommand : ICommand
{
    private readonly string _command;
    private readonly StringBuilder _sb = new();
    public UnrecognizedCommand(string command)
    {
        _command = command;
    }
    public void Execute()
    {
        Utility.SuggestCommand(_command, _sb);
    }
}
