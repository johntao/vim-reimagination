using System.Buffers;
Console.Clear();
Console.CancelKeyPress += delegate { Console.Clear(); };
new Editor().Run();
internal static class Cfg
{
  //210,419,628,837
  public const int WinWID = 209; //Console.WindowWidth;
  public const int WinHEI = 51; //Console.WindowHeight;
}
/// <summary>
/// 204,46 in vscode integrated terminal
/// 209,51 in windows terminal
/// </summary>
internal readonly ref struct Editor
{
  static Editor()
  {
  }
  private readonly DummyBuffer buffer;
  public Editor()
  {
    // ReadOnlySpan<char> tmpl = File.ReadAllText("./template.txt");
    var tmpl = File.ReadAllLines("./template.txt");
    ReadOnlySpan<char> src = string.Join("", tmpl);
    buffer = new DummyBuffer(src, Cfg.WinWID);
    Console.Write(src.ToString());
    Console.SetCursorPosition(0, 0);
  }
  internal void Run()
  {
    while (true)
    {
      var q = Console.ReadKey(true);
      var w = q.KeyChar;
      switch (w)
      {
        // case 'w': MoveHorizontalWrap(45); break;
        // case 'b': MoveHorizontalWrap(-45); break;
        case 'a': MoveHorizontalByPattern(TextPattern.WordBeginBackward); break;
        case 's': MoveHorizontalByPattern(TextPattern.WordEndBackward); break;
        case 'd': MoveHorizontalByPattern(TextPattern.WordEndForward); break;
        case 'f': MoveHorizontalByPattern(TextPattern.WordBeginFoward); break;
        case 'h': MoveHorizontal(-1); break;
        case 'j': MoveVertical(1); break;
        case 'k': MoveVertical(-1); break;
        case 'l': MoveHorizontal(1); break;
        case 'H': MoveHorizontal(-Cfg.WinWID); break;
        case 'J': MoveVertical(Cfg.WinHEI); break;
        case 'K': MoveVertical(-Cfg.WinHEI); break;
        case 'L': MoveHorizontal(Cfg.WinWID); break;
      }
    }
  }
  /// <summary>
  /// stops at the edge
  /// </summary>
  /// <param name="unit"></param>
  void MoveVertical(int unit)
  {
    var (left, top) = Console.GetCursorPosition();
    var newTop = top + unit;
    if (newTop < 0) newTop = 0;
    else if (newTop >= Cfg.WinHEI) newTop = Cfg.WinHEI - 1;
    Console.SetCursorPosition(left, newTop);
  }
  /// <summary>
  /// stops at the edge
  /// </summary>
  /// <param name="unit"></param>
  void MoveHorizontal(int unit)
  {
    var (left, top) = Console.GetCursorPosition();
    var newLeft = left + unit;
    if (newLeft < 0) newLeft = 0;
    else if (newLeft >= Cfg.WinWID) newLeft = Cfg.WinWID - 1;
    Console.SetCursorPosition(newLeft, top);
  }
  // void MoveHorizontalWrap(int unit)
  // {
  //   var (left, top) = Console.GetCursorPosition();
  //   var newLeft = left + unit;
  //   if (newLeft < 0)
  //   {
  //     newLeft += winWID;
  //     if (--top < 0)
  //     {
  //       top = 0;
  //       newLeft = 0;
  //     }
  //   }
  //   else if (newLeft >= winWID)
  //   {
  //     newLeft -= winWID;
  //     if (++top >= winHEI)
  //     {
  //       top = winHEI - 1;
  //       newLeft = winWID - 1;
  //     }
  //   }
  //   Console.SetCursorPosition(newLeft, top);
  // }
  void MoveHorizontalByPattern(TextPattern textPattern)
  {
    var (left, top) = Console.GetCursorPosition();
    var (newLeft, newTop) = textPattern switch
    {
      TextPattern.WordBeginBackward => buffer.GetSmallWordBeginBackward(left, top),
      TextPattern.WordEndBackward => buffer.GetSmallWordEndBackward(left, top),
      TextPattern.WordEndForward => buffer.GetSmallWordEndForward(left, top),
      TextPattern.WordBeginFoward => buffer.GetSmallWordBeginForward(left, top),
      _ => throw new NotImplementedException(),
    };
    // var txt = buffer.GetTextUnderCursor(left, top);
    Console.SetCursorPosition(newLeft, newTop);
    // var idx = Buffer.GetChunks().ToString().IndexOfAny(pattern, pos);
    // if (idx == -1) return;
    // var newTop = idx / winWID;
    // var newLeft = idx % winWID;
    // Console.SetCursorPosition(newLeft, newTop);
  }
}
enum TextPattern
{
  WordEndBackward = -2,
  WordBeginBackward,
  None,
  WordBeginFoward,
  WordEndForward,
}
internal readonly ref struct DummyBuffer
{
  static DummyBuffer()
  {
  }
  // private readonly ReadOnlySpan<char> _nonWordClass = " \t\n\r\f\v,:+=-*/\\(){}[]<>!@#$%^&*;\"'`~|?";
  // private readonly ReadOnlySpan<char> _nonWordClass = " ,.:;!?";
  private readonly ReadOnlySpan<char> _smallWord = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_";
  private readonly SearchValues<char> _searchSmallWord;
  // private readonly SearchValues<char> _searchNonWordClass;
  public DummyBuffer(ReadOnlySpan<char> Template, int Width)
  {
    _width = Width;
    _template = Template;
    _searchSmallWord = SearchValues.Create(_smallWord);
    // _searchNonWordClass = SearchValues.Create(_nonWordClass);
  }
  private readonly ReadOnlySpan<char> _template;
  private readonly int _width;
  #region forward
  //TODO: add unit test
  private (int, int) FindNegtiveForward(int left2D, int top2D)
  {
    var anchor1D = top2D * _width + left2D;
    ReadOnlySpan<char> spanAnchorToEnd = _template[anchor1D..];
    var foundIndexInSpan1D = spanAnchorToEnd.IndexOfAnyExcept(_searchSmallWord);
    if (foundIndexInSpan1D == -1) return (left2D, top2D);
    var txt = spanAnchorToEnd[..foundIndexInSpan1D].ToString();
    Console.SetCursorPosition(0, Cfg.WinHEI - 1);
    Console.Write(txt.PadRight(Cfg.WinWID, ' '));
    Console.SetCursorPosition(left2D, top2D);
    var newLeft2D = left2D + foundIndexInSpan1D;
    return (newLeft2D, top2D);
  }
  //TODO: add unit test
  private (int, int) FindPositiveForward(int left2D, int top2D)
  {
    var anchor1D = top2D * _width + left2D;
    ReadOnlySpan<char> spanAnchorToEnd = _template[anchor1D..];
    var foundIndexInSpan1D = spanAnchorToEnd.IndexOfAny(_searchSmallWord);
    if (foundIndexInSpan1D == -1) return (left2D, top2D);
    var newLeft2D = left2D + foundIndexInSpan1D;
    return (newLeft2D, top2D);
  }
  internal (int, int) GetSmallWordEndForward(int left2D, int top2D)
  {
    var isWordEndOrSpace = IsWordEndOrSpace(left2D, top2D);
    if (isWordEndOrSpace)
      (left2D, top2D) = FindPositiveForward(left2D + 1, top2D);
    var (newLeft, newTop) = FindNegtiveForward(left2D, top2D);
    return (newLeft - 1, newTop);
  }
  internal (int, int) GetSmallWordBeginForward(int left2D, int top2D)
  {
    var isWordEndOrSpace = IsWordEndOrSpace(left2D, top2D);
    if (!isWordEndOrSpace)
      (left2D, top2D) = FindNegtiveForward(left2D, top2D);
    var (newLeft, newTop) = FindPositiveForward(left2D + 1, top2D);
    return (newLeft, newTop);
  }
  private bool IsWordEndOrSpace(int left2D, int top2D)
  {
    var anchor1D = top2D * _width + left2D;
    var span = _template[anchor1D..(anchor1D + 2)];
    var isWordEnd = span.Length == 1 || (
      _smallWord.Contains(span[0]) &&
      !_smallWord.Contains(span[1])
    );
    if (isWordEnd) return true;
    var isSpace = !_smallWord.Contains(span[0]);
    return isSpace;
  }
  #endregion
  #region backward
  //TODO: add unit test
  private (int, int) FindNegativeBackward(int left2D, int top2D)
  {
    var anchor1D = top2D * _width + left2D;
    ReadOnlySpan<char> spanBeginToAnchor = _template[..(anchor1D + 1)];
    var foundIndexInSpan1D = spanBeginToAnchor.LastIndexOfAnyExcept(_searchSmallWord);
    if (foundIndexInSpan1D == -1) return (left2D, top2D);
    var txt = spanBeginToAnchor[(foundIndexInSpan1D + 1)..].ToString();
    Console.SetCursorPosition(0, Cfg.WinHEI - 1);
    Console.Write(txt.PadRight(Cfg.WinWID, ' '));
    Console.SetCursorPosition(left2D, top2D);
    var newLeft2D = left2D - (anchor1D - foundIndexInSpan1D);
    return (newLeft2D, top2D);
  }
  //TODO: add unit test
  private (int, int) FindPositiveBackward(int left2D, int top2D)
  {
    var anchor1D = top2D * _width + left2D;
    ReadOnlySpan<char> spanBeginToAnchor = _template[..(anchor1D + 1)];
    var foundIndexInSpan1D = spanBeginToAnchor.LastIndexOfAny(_searchSmallWord);
    if (foundIndexInSpan1D == -1) return (left2D, top2D);
    var newLeft2D = left2D - (anchor1D - foundIndexInSpan1D);
    return (newLeft2D, top2D);
  }
  internal (int, int) GetSmallWordEndBackward(int left2D, int top2D)
  {
    var isWordBeginOrSpace = IsWordBeginOrSpace(left2D, top2D);
    if (!isWordBeginOrSpace)
      (left2D, top2D) = FindNegativeBackward(left2D, top2D);
    var (newLeft, newTop) = FindPositiveBackward(left2D - 1, top2D);
    return (newLeft, newTop);
  }
  internal (int, int) GetSmallWordBeginBackward(int left2D, int top2D)
  {
    var isWordBeginOrSpace = IsWordBeginOrSpace(left2D, top2D);
    if (isWordBeginOrSpace)
      (left2D, top2D) = FindPositiveBackward(left2D - 1, top2D);
    var (newLeft, newTop) = FindNegativeBackward(left2D, top2D);
    return (newLeft + 1, newTop);
  }
  private bool IsWordBeginOrSpace(int left2D, int top2D)
  {
    var anchor1D = top2D * _width + left2D;
    var span = _template[(anchor1D - 1)..(anchor1D + 1)];
    var isWordBegin = span.Length == 1 || (
      !_smallWord.Contains(span[0]) &&
      _smallWord.Contains(span[1])
    );
    if (isWordBegin) return true;
    var isSpace = !_smallWord.Contains(span[0]);
    return isSpace;
  }
  #endregion
}