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
  public void Clear() => Console.Clear();
  public bool CursorVisible
  {
    set => Console.CursorVisible = value;
  }
  public int CursorTop
  {
    get => Console.CursorTop;
    set => Console.CursorTop = value;
  }
  public ConsoleKeyInfo ReadKey() => Console.ReadKey(true);
  public void SetCursorPosition(int left, int top) => Console.SetCursorPosition(left, top);
  public void Write(string c) => Console.Write(c);
  public (int Left, int Top) GetCursorPosition() => Console.GetCursorPosition();
}

internal interface ITextRenderer
{
  void Write(char c);
  void WriteLine(string message);
  void WriteLine(Enum message);
  void Write(string c);
  void Clear();
  bool CursorVisible { set; }
  int CursorTop { get; set; }
  ConsoleKeyInfo ReadKey();
  (int Left, int Top) GetCursorPosition();
  void SetCursorPosition(int left, int top);
}