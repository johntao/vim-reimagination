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
  public (int Left, int Top) GetCursorPosition2D() => Console.GetCursorPosition();
  public int GetCursorPosition1D(IWindow win)
  {
    var (left, top) = GetCursorPosition2D();
    return top * win.Width + left;
  }
}
internal interface ICursor
{
  bool CursorVisible { set; }
  int CursorTop { get; set; }
  int CursorLeft { get; set; }
  (int Left, int Top) GetCursorPosition2D();
  int GetCursorPosition1D(IWindow win);
  void SetCursorPosition(int left, int top);
}