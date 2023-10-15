using System.Text;
using FileDiff.UnitTests;
using FluentAssertions;
using Patterns;

namespace FileDiff.IntegrationTests;
public class ProgramTests
{
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
}
