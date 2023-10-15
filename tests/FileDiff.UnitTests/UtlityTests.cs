using System.Text;
using FluentAssertions;
using Patterns;

namespace FileDiff.UnitTests;

public class UtilityTests
{
    [Theory]
    [InlineData("kitten", "sitting", 3)]
    [InlineData("flaw", "lawn", 2)]
    [InlineData("", "abc", 3)]
    [InlineData("abc", "abc", 0)]
    [InlineData("abcdef", "ghijkl", 6)]
    public void Levenshtein_CalculatesDistanceCorrectly(string a, string b, int expectedDistance)
    {
        // Arrange

        // Act
        int distance = Utility.Levenshtein(a, b);

        // Assert
        distance.Should().Be(expectedDistance);
    }

    [Fact]
    public void SuggestCommand_ReturnsCorrectSuggestion()
    {
        // Arrange
        const string command = "patc";
        const string expectedSuggestion = "patch";
        var sb = new StringBuilder();
        using var outputCapture = new ConsoleOutputCapture();

        // Act
        Utility.SuggestCommand(command, sb);

        // Assert
        var capturedOutput = outputCapture.GetCapturedOutput();
        capturedOutput.Should().Contain(expectedSuggestion);
    }

    [Fact]
    public void SuggestCommand_ReturnsHelpfulMessage()
    {
        // Arrange
        const string command = "unrecognizedcommand";
        const string expectedSuggestion = $"Command '{command}' not found";
        var sb = new StringBuilder();
        using var outputCapture = new ConsoleOutputCapture();

        // Act
        Utility.SuggestCommand(command, sb);

        // Assert
        var capturedOutput = outputCapture.GetCapturedOutput();
        capturedOutput.Should().NotBeNullOrWhiteSpace();
        capturedOutput.Should().Contain(expectedSuggestion);
    }
}