namespace Patterns;

public enum Actions
{
    IGNORE = 1,
    MODIFY,
    ADD,
    REMOVE
}

public static class ActionsExtensions
{
    public static string ToSymbol(this Actions action) =>
        action switch
        {
            Actions.IGNORE => " ",
            Actions.MODIFY => "~",
            Actions.ADD => "+",
            Actions.REMOVE => "-",
            _ => throw new NotImplementedException()
        };

    public static char ToChar(this Actions action) =>
        action switch
        {
            Actions.IGNORE => 'I',
            Actions.MODIFY => 'M',
            Actions.ADD => 'A',
            Actions.REMOVE => 'R',
            _ => throw new NotImplementedException()
        };

    public static Actions ToAction(this string action) =>
        action switch
        {
            "I" => Actions.IGNORE,
            "M" => Actions.MODIFY,
            "A" => Actions.ADD,
            "R" => Actions.REMOVE,
            _ => throw new NotImplementedException()
        };

    public static byte ToByte(this string line) =>
        byte.Parse(line);
}

