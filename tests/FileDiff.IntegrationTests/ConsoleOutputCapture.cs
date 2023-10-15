namespace FileDiff.UnitTests;

public class ConsoleOutputCapture : IDisposable
{
    private StringWriter stringWriter;
    private TextWriter originalOutput;

    public ConsoleOutputCapture()
    {
        stringWriter = new StringWriter();
        originalOutput = Console.Out;
        Console.SetOut(stringWriter);
    }

    public string GetCapturedOutput()
    {
        return stringWriter.ToString();
    }

    public void Dispose()
    {
        Console.SetOut(originalOutput);
        stringWriter.Dispose();
    }
}