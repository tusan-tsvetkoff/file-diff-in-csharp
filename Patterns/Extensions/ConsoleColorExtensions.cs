using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Patterns.Extensions;

public static class ConsoleColorExtensions
{
    public static void Write(this string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ResetColor();
    }
    public static void WriteLine(this string text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }
    public static void Write(this IEnumerable<string> text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        foreach (var line in text)
        {
            Console.Write(line);
        }
        Console.ResetColor();
    }
    public static void WriteLine(this IEnumerable<string> text, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        foreach (var line in text)
        {
            Console.WriteLine(line);
        }
        Console.ResetColor();
    }
}
