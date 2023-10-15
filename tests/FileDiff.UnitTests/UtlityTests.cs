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
        var capturedOutput = outputCapture.GetCapturedOutput();
        // Assert
        capturedOutput.Should().Contain(expectedSuggestion);
    }
}