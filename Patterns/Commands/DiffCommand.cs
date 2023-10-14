using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Patterns.Abstractions;

namespace Patterns.Commands;

public class DiffCommand : ICommand
{
    private readonly string _filePathSource;
    private readonly string _filePathTarget;
    private readonly string? _outputFile;

    public DiffCommand(string filePathSource, string filePathTarget, string? outputFile = null)
    {
        _filePathSource = filePathSource;
        _filePathTarget = filePathTarget;
        _outputFile = outputFile;
    }
    public void Execute()
    {
        Utility.Levenshtein(_filePathSource, _filePathTarget, out var filePatch);
        if (_outputFile is not null)
            Utility.CreatePatchFile(filePatch);
    }
}
