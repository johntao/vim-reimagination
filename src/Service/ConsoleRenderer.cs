namespace VimRenaissance.Service;

internal static class ConsoleRenderer // : ITextRenderer
{
  // internal static int CursorTop { get; private set; } = 0;
  internal static void Write(char c)
  {
    Console.Write(c);
    --Console.CursorLeft;
  }
  internal static void WriteLine(string message)
  {
    // ++CursorTop;
    Console.WriteLine(message);
  }
  internal static void WriteLine(Enum message)
  {
    // ++CursorTop;
    Console.WriteLine(message);
  }
}

// internal interface ITextRenderer
// {
//   void Write(char c);
//   void WriteLine(string message);
//   void WriteLine(Enum message);
// }