using Patterns.Enums;
using Patterns.Extensions;

namespace Patterns;

public readonly record struct Patch
{
    public Actions Action { get; init; }
    public byte Line { get; init; }
    public string Content { get; init; }
    public ConsoleColor Color => Action switch
    {
        Actions.ADD => ConsoleColor.Green,
        Actions.REMOVE => ConsoleColor.Red,
        _ => ConsoleColor.White
    };

    private Patch(Actions action, byte line, string content) =>
        (Action, Line, Content) = (action, line, content);

    public static Patch Create(Actions action, byte line, string content) =>
        new(action, line, content);

    public override string ToString() =>
        $"[{Action.ToChar()}] '{Line}' - {Content.TrimStart()}";
}
