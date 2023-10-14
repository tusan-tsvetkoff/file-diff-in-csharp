using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Patterns.Abstractions;

namespace Patterns.Commands;

public class ListCommand : ICommand
{
    public void Execute()
    {
        Command.PrintCommandList();
    }
}
