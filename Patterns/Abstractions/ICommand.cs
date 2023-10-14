using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Patterns.Abstractions;

public interface ICommand
{
    void Execute();
}
