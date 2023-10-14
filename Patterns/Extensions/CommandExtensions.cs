using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Patterns.Enums;

namespace Patterns.Extensions;

public static class CommandExtensions
{
    public static CommandTypes ToCommand(this string command)
    {
        return command switch
        {
            "patch" => CommandTypes.Patch,
            "help" => CommandTypes.Help,
            "diff" => CommandTypes.Diff,
            "list" => CommandTypes.List,
            _ => CommandTypes.None // ? Maybe Have UnrecognizedCommand instead
        };
    }
}
