using System.Text;

namespace Patterns;

public readonly record struct Command
{
    public string Run { get; init; }
    public string Args { get; init; }
    public string Description { get; init; }

    private Command(string run, string args, string description)
        => (Run, Args, Description) = (run, args, description);

    private static readonly Command _diff = Create("diff", "<source> <target> [> file.patch]", "Generate a patch file from two files");
    private static readonly Command _patch = Create("patch", "<file.patch> <patch-target>", "Apply a patch file to a file");
    private static readonly Command _help = Create("help", "", "Show this help message");
    private static readonly Command _list = Create("list", "", "List all commands");

    private static Command Create(string run, string args, string description) =>
        new(run, args, description);
    public static IEnumerable<Command> GetCommands() =>
        new List<Command>
        {
            _diff,
            _patch,
            _help,
            _list
        };

    public static Command GetCommand(string command) =>
        GetCommands().FirstOrDefault(c => c.Run.Equals(command, StringComparison.InvariantCultureIgnoreCase));

    public static void PrintFullHelp()
    {
        var sb = new StringBuilder();
        HelpConstruct(sb);
        Console.WriteLine(sb.ToString());
    }

    private static void HelpConstruct(StringBuilder sb)
    {
        sb.AppendLine();
        sb.AppendLine(General.Usage);
        sb.AppendLine();
        sb.AppendLine("Commands:");
        sb.AppendLine($"  {_diff.Run} {_diff.Args}  {_diff.Description}");
        sb.AppendLine($"  {_patch.Run} {_patch.Args}      {_patch.Description}");
        sb.AppendLine($"  {_list.Run}                                   {_list.Description}");
        sb.AppendLine($"  {_help.Run}                                   {_help.Description}");
        sb.AppendLine();
        sb.AppendLine("source:");
        sb.AppendLine("  The source file to compare");
        sb.AppendLine("target:");
        sb.AppendLine("  The target file to compare");
        sb.AppendLine("file.patch:");
        sb.AppendLine("  The patch file to apply");
        sb.AppendLine("patch-target:");
        sb.AppendLine("  The file to apply the patch to");
        sb.AppendLine();
        sb.Append("Run 'dotnet run [command] help' for more information about a command.");
    }

    public static void PrintCommandList()
    {
        var sb = new StringBuilder();
        sb.AppendLine("Commands:");
        foreach (var cmd in GetCommands())
        {
            sb.AppendLine($"  {cmd.Run} {cmd.Args}");
        }
        Console.WriteLine(sb.ToString());
    }

    public static void PrintCommandHelp(string command)
    {
        var sb = new StringBuilder();
        var cmd = GetCommand(command);
        if (cmd == default)
        {
            Utility.SuggestCommand(command, sb);
        }
        else
        {
            sb.AppendLine($"Usage: dotnet run {cmd.Run} {cmd.Args}");
            sb.AppendLine();
            sb.Append(cmd.Description);
        }
        Console.WriteLine(sb.ToString());
    }
}