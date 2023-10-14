using System.Text;
using Patterns.Commands;
using Patterns.Enums;
using Patterns.Extensions;
using ICommand = Patterns.Abstractions.ICommand;
namespace Patterns;

public class Program
{
    private static readonly StringBuilder sb = new();
    static void Main(string[] args)
    {
        var invoker = new CommandInvoker();
        ICommand command = new NullCommand();
        switch (args.Length)
        {
            case 1:
                command = args[0].ToCommand() switch
                {
                    CommandTypes.Help => new FullHelpCommand(),
                    CommandTypes.List => new ListCommand(),
                    _ => new UnrecognizedCommand(args[0])
                };
                break;
            case 2:
                if (args[1] == "help" && args[0].ToCommand() is not CommandTypes.None)
                {
                    command = new CmdHelpCommand(args[0]);
                }
                command = new UnrecognizedCommand(args[0]);
                break;
            case 3:
                command = args[0].ToCommand() switch
                {
                    CommandTypes.Diff => new DiffCommand(args[1], args[2]),
                    CommandTypes.Patch => new PatchCommand(args[1], args[2]),
                    _ => new UnrecognizedCommand(args[0])
                };
                break;
            case 5:
                command = args[0].ToCommand() switch
                {
                    CommandTypes.Diff => new DiffCommand(args[1], args[2], args[4]),
                    CommandTypes.Patch => new PatchCommand(args[1], args[2]),
                    _ => new UnrecognizedCommand(args[0])
                };
                break;
            default:
                break;
        }
        invoker.SetCommand(command);
        invoker.ExecuteCommand();
    }
}
