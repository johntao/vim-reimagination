namespace VimRenaissance.Service;

internal class ConsoleRenderer : ITextRenderer
{
  // internal static int CursorTop { get; private set; } = 0;
  public void Write(char c)
  {
    Console.Write(c);
    --Console.CursorLeft;
  }
  public void WriteLine(string message)
  {
    // ++CursorTop;
    Console.WriteLine(message);
  }
  public void WriteLine(Enum message)
  {
    // ++CursorTop;
    Console.WriteLine(message);
  }
}

internal interface ITextRenderer
{
  void Write(char c);
  void WriteLine(string message);
  void WriteLine(Enum message);
}