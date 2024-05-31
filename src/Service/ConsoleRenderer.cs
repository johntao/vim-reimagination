namespace VimReimagination.Service;

internal class ConsoleRenderer : ITextRenderer
{
  public void Write(char[] buffer) => Console.Write(buffer);
  public void Write(string c) => Console.Write(c);
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
  public void Clear() => Console.Clear();
  public ConsoleKeyInfo ReadKey() => Console.ReadKey(true);
}
internal interface ITextRenderer
{
  void Write(char[] buffer);
  void Write(char c);
  void WriteLine(string message);
  void WriteLine(Enum message);
  void Write(string c);
  void Clear();
  ConsoleKeyInfo ReadKey();
}