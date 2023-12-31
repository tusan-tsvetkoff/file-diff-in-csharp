﻿using System.Text;
using System.Text.RegularExpressions;
using Patterns.Common;
using Patterns.Enums;
using Patterns.Extensions;

namespace Patterns;

public static class Utility
{
    private const string _patchRegex = @"^\[(?<action>[AR])\]\s'(?<line>\d+)' - (?<content>.*)$";

    /// <summary>
    /// Calculates the Levenshtein distance between two strings.
    /// </summary>
    /// <param name="a">The first string to compare.</param>
    /// <param name="b">The second string to compare.</param>
    /// <returns>The Levenshtein distance between the two strings.</returns>
    public static int Levenshtein(string a, string b)
    {
        int result = 0;
        if (a.Equals(b)) return result;

        int lengthA = a.ToCharArray().Length;
        int lengthB = b.ToCharArray().Length;

        if (lengthA == 0) return lengthB;
        if (lengthB == 0) return lengthA;

        var cache = Enumerable.Range(1, lengthA).ToList();
        int distanceA;
        int distanceB;

        for (int i = 0; i < lengthB; i++)
        {
            char codeB = b[i];
            result = i;
            distanceA = i;

            for (int j = 0; j < lengthA; j++)
            {
                char codeA = a[j];
                distanceB = codeB == codeA ? distanceA : distanceA + 1;

                distanceA = cache.ElementAt(j);

                if (distanceA > result)
                {
                    result = distanceB > result ? result + 1 : distanceB;
                }
                else
                {
                    result = distanceB > distanceA ? distanceA + 1 : distanceB;
                }

                cache[j] = result;
            }
        }
        return result;
    }

    /// <summary>
    /// Suggests a command based on the user input by calculating the Levenshtein distance between the user input and available commands.
    /// </summary>
    /// <param name="command">The user input command.</param>
    /// <param name="sb">The StringBuilder instance to append the suggestion message.</param>
    public static void SuggestCommand(string command, StringBuilder sb)
    {
        var suggestions = Command.GetCommands().Select(c => c.Run).ToList();
        foreach (var suggestion in suggestions)
        {
            var distance = Levenshtein(command, suggestion);
            if (distance <= 3)
            {
                sb.AppendLine($"Command '{command}' not found.");
                sb.AppendLine("Suggestion:");
                sb.Append($"    Did you mean '{suggestion}'?");
            }
        }
        if (sb.Length == 0)
        {
            sb.AppendLine($"Command '{command}' not found.");
            sb.Append("Run 'dotnet run help' for usage.");
        }
        Console.WriteLine(sb.ToString());
    }

    /// <summary>
    /// Calculates the Levenshtein distance between two strings and generates a list of patches to transform string a into string b.
    /// </summary>
    /// <param name="a">The first string to compare.</param>
    /// <param name="b">The second string to compare.</param>
    /// <param name="filePatch">The list of patches to transform string a into string b.</param>
    public static void Levenshtein(string a, string b, out List<Patch> filePatch)
    {
        var sourceLines = File.ReadAllLines(a).ToList();
        var targetLines = File.ReadAllLines(b).ToList();

        int sourceLength = sourceLines.Count;
        int targetLength = targetLines.Count;

        var distances = new int[sourceLength + 1][];
        var actions = new int[sourceLength + 1][];

        for (int i = 0; i < sourceLength + 1; i++)
        {
            distances[i] = new int[targetLength + 1];
            actions[i] = new int[targetLength + 1];
        }

        distances[0][0] = 0;
        actions[0][0] = (int)Actions.IGNORE;

        for (int j = 1; j < targetLength + 1; j++)
        {
            int i = 0;
            distances[i][j] = j;
            actions[i][j] = (int)Actions.ADD;
        }

        for (int i = 1; i < sourceLength + 1; i++)
        {
            int j = 0;
            distances[i][j] = i;
            actions[i][j] = (int)Actions.REMOVE;
        }


        for (int i = 1; i < sourceLength + 1; i++)
        {
            for (int j = 1; j < targetLength + 1; j++)
            {
                if (sourceLines[i - 1] == targetLines[j - 1])
                {
                    distances[i][j] = distances[i - 1][j - 1];
                    actions[i][j] = (int)Actions.IGNORE;
                    continue;
                }

                int remove = distances[i - 1][j];
                int add = distances[i][j - 1];

                distances[i][j] = remove;
                actions[i][j] = (int)Actions.REMOVE;

                if (distances[i][j] > add)
                {
                    distances[i][j] = add;
                    actions[i][j] = (int)Actions.ADD;
                }

                distances[i][j] += 1;
            }
        }
        GetPatch(a, b, actions, sourceLength, targetLength, targetLines, sourceLines, out filePatch);
        // uncomment to trace cache in console
        // TraceCache(distances, actions);
    }

    public static void GetPatch(
         string a,
         string b,
         int[][] actions,
         int sourceLength,
         int targetLength,
         List<string> targetLines,
         List<string> sourceLines,
         out List<Patch> filePatch)
    {
        var patch = new List<Patch>();
        while (sourceLength > 0 || targetLength > 0)
        {
            var action = actions[sourceLength][targetLength];
            switch (action)
            {
                case (int)Actions.IGNORE:
                    // patch.Add($" {sourceLines[sourceLength - 1]}");
                    sourceLength--;
                    targetLength--;
                    break;
                case (int)Actions.MODIFY:
                    patch.Add(Patch.Create(Actions.MODIFY, (byte)targetLength, targetLines[targetLength - 1]));
                    patch.Add(Patch.Create(Actions.MODIFY, (byte)sourceLength, sourceLines[sourceLength - 1]));
                    sourceLength--;
                    targetLength--;
                    break;
                case (int)Actions.ADD:
                    patch.Add(Patch.Create(Actions.ADD, (byte)targetLength, targetLines[targetLength - 1]));
                    targetLength--;
                    break;
                case (int)Actions.REMOVE:
                    patch.Add(Patch.Create(Actions.REMOVE, (byte)sourceLength, sourceLines[sourceLength - 1]));
                    sourceLength--;
                    break;
            }
        }
        filePatch = patch;
        TraceDiff(patch, a, b);
    }

    public static void CreatePatchFile(List<Patch> filePatch)
    {
        using var sw = new StreamWriter("file.patch");
        foreach (var line in filePatch)
        {
            sw.WriteLine(line.ToString());
        }
    }

    /// <summary>
    /// Applies a patch file to a target file, modifying the target file according to the instructions in the patch file.
    /// </summary>
    /// <param name="patchPath">The path to the patch file.</param>
    /// <param name="targetPath">The path to the target file.</param>
    public static void PatchCommand(string patchPath, string targetPath)
    {
        var lines = File.ReadAllLines(targetPath).ToList();
        var patchLines = File.ReadAllLines(patchPath).ToList();
        var patch = new List<Patch>();
        bool ok = true;
        foreach (var patchLine in patchLines)
        {
            if (String.IsNullOrWhiteSpace(patchLine) || !patchLine.StartsWith('['))
            {
                continue;
            }

            var match = Regex.Match(patchLine, _patchRegex);

            if (!match.Success || !match.Groups[ActionGroups.Action].Success)
            {
                Console.WriteLine($"{General.InvalidPatchAction} '{patchLine}'");
                ok = false;
                continue;
            }
            if (ok is false)
            {
                break;
            }
            var action = match.Groups[ActionGroups.Action].Value;
            var line = match.Groups[ActionGroups.Line].Value;
            var content = match.Groups[ActionGroups.Content].Value;
            patch.Add(Patch.Create(action.ToAction(), line.ToByte(), content));
        }

        if (ok)
        {
            foreach (var item in patch.Reverse<Patch>())
            {
                if (item.Action == Actions.ADD)
                {
                    if (item.Line == lines.Count + 1)
                    {
                        lines.AddRange(new[] { item.Content });
                    }
                    else
                    {
                        lines.Insert(item.Line - 1, item.Content);
                    }
                }
                else if (item.Action == Actions.REMOVE)
                {
                    lines.RemoveAt(item.Line - 1);
                }
            }
            File.WriteAllLines(targetPath, lines);
        }
    }

    private static void TraceDiff(List<Patch> patch, string sourceFile, string targetFile)
    {
        patch.Reverse();

        Console.WriteLine($"{patch.Count} changes detected");
        Console.WriteLine($"--- {sourceFile}");
        Console.WriteLine($"+++ {targetFile}");
        Console.WriteLine();
        foreach (var item in patch)
        {
            item.ToString().WriteLine(item.Color);
        }
    }

    private static void TraceCache(int[][] distances, int[][] actions)
    {
        for (int row = 0; row < distances.Length; row++)
        {
            for (int col = 0; col < distances[row].Length; col++)
            {
                var item = distances[row][col];
                var action = (Actions)actions[row][col];
                Console.Write($"{item}({action.ToChar()}) ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }
}

