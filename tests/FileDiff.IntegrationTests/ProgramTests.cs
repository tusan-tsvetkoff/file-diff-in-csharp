using System.IO;
using System.Text;
using FileDiff.UnitTests;
using FluentAssertions;
using Patterns;

namespace FileDiff.IntegrationTests;
public class ProgramTests
{
    [Fact]
    public void Program_Main_DiffCommand_ShowsDiff()
    {
        // Arrange
        string baseDir = AppContext.BaseDirectory;
        string fileAPath = Path.Combine(baseDir, "..", "..", "..", "..", "Data", "fileA.txt");
        string fileBPath = Path.Combine(baseDir, "..", "..", "..", "..", "Data", "fileB.txt");

        var commandLine = new string[] { "diff", fileAPath, fileBPath };
        var sb = new StringBuilder();
        using var outputCapture = new ConsoleOutputCapture();

        // Act
        Program.Main(commandLine);

        // Assert
        string capturedOutput = outputCapture.GetCapturedOutput();
        capturedOutput.Should().NotBeNullOrWhiteSpace();
        capturedOutput.Should().Contain("2 changes detected");
        capturedOutput.Should().Contain("---");
        capturedOutput.Should().Contain("+++");
        capturedOutput.Should().Contain("[A] '1' - Test file B");
        capturedOutput.Should().Contain("[R] '1' - Test file A");
    }

    [Fact]
    public void Program_Main_PatchCommand_PatchesFileCorrectly()
    {
        // Arrange
        string baseDir = AppContext.BaseDirectory;
        string originalPath = Path.Combine(baseDir, "..", "..", "..", "..", "Data", "original.txt");
        string patchPath = Path.Combine(baseDir, "..", "..", "..", "..", "Data", "file.patch");
        string expectedPath = Path.Combine(baseDir, "..", "..", "..", "..", "Data", "expected.txt");
        string tempPath = Path.GetTempFileName();
        File.Copy(originalPath, tempPath, overwrite: true);
        var commandLine = new string[] { "patch", patchPath, tempPath };

        // Act
        Program.Main(commandLine);

        // Assert
        string actual = File.ReadAllText(tempPath).TrimEnd();
        string expected = File.ReadAllText(expectedPath);
        expected.Should().Be(actual).And.NotBeNullOrWhiteSpace();

        // Clean up
        File.Delete(tempPath);
    }

    [Theory]
    [InlineData("diff")]
    [InlineData("patch")]
    public void Program_Main_CmdHelpCommand_ShowsCorrectHelp(string cmd)
    {
        // Arrange
        const string help = "help";
        var commandLine = new string[] { cmd, help };
        var sb = new StringBuilder();
        using var outputCapture = new ConsoleOutputCapture();

        // Act
        Program.Main(commandLine);

        // Assert
        string capturedOutput = outputCapture.GetCapturedOutput();
        capturedOutput.Should().NotBeNullOrWhiteSpace();
        capturedOutput.Should().Contain($"Usage: dotnet run {cmd}");
    }

    [Fact]
    public void Program_Main_HelpCommand_DisplaysHelp()
    {
        // Arrange
        string commandLine = "help";
        var sb = new StringBuilder();
        using var outputCapture = new ConsoleOutputCapture();

        // Act
        Program.Main(commandLine.Split(' '));

        // Assert
        string capturedOutput = outputCapture.GetCapturedOutput();
        capturedOutput.Should().NotBeNullOrWhiteSpace();
        capturedOutput.Should().Contain("Usage: dotnet run [command] [arguments]");
        capturedOutput.Should().Contain("Commands:");
    }

    [Fact]
    public void Program_Main_ListCommand_ListsCommands()
    {
        // Arrange
        string commandLine = "list";
        var sb = new StringBuilder();
        using var outputCapture = new ConsoleOutputCapture();

        // Act
        Program.Main(commandLine.Split(' '));

        // Assert
        string capturedOutput = outputCapture.GetCapturedOutput();
        capturedOutput.Should().NotBeNullOrWhiteSpace();
        capturedOutput.Should().Contain("Commands:");
        capturedOutput.Should().Contain("diff");
        capturedOutput.Should().Contain("patch");
        capturedOutput.Should().Contain("help");
    }

    [Fact]
    public void Program_Main_UnrecognizedCommand_ShowsUsageMessage()
    {
        // Arrange
        string commandLine = "unknowncommand";
        var sb = new StringBuilder();
        using var outputCapture = new ConsoleOutputCapture();

        // Act
        Program.Main(commandLine.Split(' '));

        // Assert
        string capturedOutput = outputCapture.GetCapturedOutput();
        capturedOutput.Should().NotBeNullOrWhiteSpace();
        capturedOutput.Should().Contain($"Command '{commandLine}' not found.");
        capturedOutput.Should().Contain(commandLine);
    }

    [Fact]
    public void Program_Main_NullCommand_ShowsFullHelp()
    {
        // Arrange
        var args = Array.Empty<string>();
        var sb = new StringBuilder();
        using var outputCapture = new ConsoleOutputCapture();

        // Act
        Program.Main(args);

        // Assert
        string capturedOutput = outputCapture.GetCapturedOutput();
        capturedOutput.Should().NotBeNullOrWhiteSpace();
        capturedOutput.Should().Contain("Usage: dotnet run [command] [arguments]");
        capturedOutput.Should().Contain("Commands:");
    }
}
