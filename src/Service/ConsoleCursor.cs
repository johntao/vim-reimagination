namespace VimReimagination.Service;

internal class ConsoleCursor : ICursor
{
  public bool CursorVisible
  {
    set => Console.CursorVisible = value;
  }
  public int CursorTop
  {
    get => Console.CursorTop;
    set => Console.CursorTop = value;
  }
  public int CursorLeft
  {
    get => Console.CursorLeft;
    set => Console.CursorLeft = value;
  }
  public void SetCursorPosition(int left, int top) => Console.SetCursorPosition(left, top);
  public (int Left, int Top) GetCursorPosition() => Console.GetCursorPosition();
}
internal interface ICursor
{
  bool CursorVisible { set; }
  int CursorTop { get; set; }
  int CursorLeft { get; set; }
  (int Left, int Top) GetCursorPosition();
  void SetCursorPosition(int left, int top);
}