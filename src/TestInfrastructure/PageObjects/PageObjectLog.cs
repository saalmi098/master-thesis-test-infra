namespace TestInfrastructure.PageObjects;

/// <summary>
/// Timestamped debug output for page-object creation. The sink is per async flow (per test),
/// so parallel tests do not mix output. Falls back to the console when no sink is set.
/// </summary>
public static class PageObjectLog
{
    private static readonly AsyncLocal<Action<string>?> Sink = new();

    /// <summary>
    /// Redirects log output of the current async flow (e.g. to the test output); null restores the console.
    /// </summary>
    /// <param name="sink">Receives each log line; null restores console output.</param>
    public static void SetSink(Action<string>? sink) => Sink.Value = sink;

    /// <summary>
    /// Writes a timestamped line, indented by two spaces per <paramref name="depth"/> level.
    /// </summary>
    /// <param name="depth">Nesting level; two spaces of indentation per level.</param>
    /// <param name="message">Text to log.</param>
    internal static void Write(int depth, string message)
    {
        var line = $"[{DateTime.Now:HH:mm:ss.fff}] {new string(' ', depth * 2)}{message}";
        (Sink.Value ?? Console.WriteLine)(line);
    }
}
