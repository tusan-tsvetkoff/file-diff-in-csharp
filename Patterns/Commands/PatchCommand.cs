using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Patterns.Abstractions;

namespace Patterns.Commands;

public class PatchCommand : ICommand
{
    private readonly string _patchPath;
    private readonly string _filePathTarget;
    public PatchCommand(string patchPath, string filePathTarget)
    {
        _patchPath = patchPath;
        _filePathTarget = filePathTarget;
    }

    public void Execute()
    {
        Utility.PatchCommand(_patchPath, _filePathTarget);
    }
}
