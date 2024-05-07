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
      TextPattern.WordBeginBackward => buffer.GetWordBeginPositionBackward(left, top),
      TextPattern.WordEndBackward => buffer.GetWordEndPositionBackward(left, top),
      TextPattern.WordEndForward => buffer.GetWordEndPositionForward(left, top),
      TextPattern.WordBeginFoward => buffer.GetWordBeginPositionForward(left, top),
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
  private readonly ReadOnlySpan<char> _nonWordClass = " ,.:;!?";
  private readonly ReadOnlySpan<char> _wordClass = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_";
  private readonly SearchValues<char> _searchWordClass, _searchNonWordClass;
  public DummyBuffer(ReadOnlySpan<char> Template, int Width)
  {
    _width = Width;
    _template = Template;
    _searchWordClass = SearchValues.Create(_wordClass);
    _searchNonWordClass = SearchValues.Create(_nonWordClass);
  }
  private readonly ReadOnlySpan<char> _template;
  private readonly int _width;
  //TODO: add unit test
  private (int, int) FindNonWordForward(int left2D, int top2D, int anchor1D)
  {
    ReadOnlySpan<char> spanAnchorToEnd = _template[anchor1D..];
    var foundIndexInSpan1D = spanAnchorToEnd.IndexOfAny(_searchNonWordClass);
    if (foundIndexInSpan1D == -1) return (left2D, top2D);
    var txt = spanAnchorToEnd[..foundIndexInSpan1D].ToString();
    Console.SetCursorPosition(0, Cfg.WinHEI - 1);
    Console.Write(txt.PadRight(Cfg.WinWID, ' '));
    Console.SetCursorPosition(left2D, top2D);
    var newLeft2D = left2D + foundIndexInSpan1D;
    return (newLeft2D, top2D);
  }
  //TODO: add unit test
  private (int, int) FindWordForward(int left2D, int top2D, int anchor1D)
  {
    ReadOnlySpan<char> spanAnchorToEnd = _template[anchor1D..];
    var foundIndexInSpan1D = spanAnchorToEnd.IndexOfAny(_searchWordClass);
    if (foundIndexInSpan1D == -1) return (left2D, top2D);
    var newLeft2D = left2D + foundIndexInSpan1D;
    return (newLeft2D, top2D);
  }
  internal (int, int) GetWordEndPositionForward(int left2D, int top2D)
  {
    var anchor1D = top2D * _width + left2D;
    // var 
    (left2D, top2D) = FindWordForward(left2D + 1, top2D, anchor1D);
    var (newLeft, newTop) = FindNonWordForward(left2D, top2D, anchor1D);
    return (newLeft - 1, newTop);
  }
  internal (int, int) GetWordBeginPositionForward(int left2D, int top2D)
  {
    var anchor1D = top2D * _width + left2D;
    var (newLeft, newTop) = FindNonWordForward(left2D, top2D, anchor1D);
    return FindWordForward(newLeft, newTop, anchor1D);
  }
  //TODO: add unit test
  private (int, int) FindNonWordBackward(int left2D, int top2D, int anchor1D)
  {
    ReadOnlySpan<char> spanBeginToAnchor = _template[..anchor1D];
    var foundIndexInSpan1D = spanBeginToAnchor.LastIndexOfAny(_searchNonWordClass);
    if (foundIndexInSpan1D == -1) return (left2D, top2D);
    var txt = spanBeginToAnchor[(foundIndexInSpan1D + 1)..].ToString();
    Console.SetCursorPosition(0, Cfg.WinHEI - 1);
    Console.Write(txt.PadRight(Cfg.WinWID, ' '));
    Console.SetCursorPosition(left2D, top2D);
    var newLeft2D = left2D - (anchor1D - foundIndexInSpan1D);
    return (newLeft2D, top2D);
  }
  //TODO: add unit test
  private (int, int) FindWordBackward(int left2D, int top2D, int anchor1D)
  {
    ReadOnlySpan<char> spanBeginToAnchor = _template[..anchor1D];
    var foundIndexInSpan1D = spanBeginToAnchor.LastIndexOfAny(_searchWordClass);
    if (foundIndexInSpan1D == -1) return (left2D, top2D);
    var newLeft2D = left2D - (anchor1D - foundIndexInSpan1D);
    return (newLeft2D, top2D);
  }
  internal (int, int) GetWordEndPositionBackward(int left2D, int top2D)
  {
    var anchor1D = top2D * _width + left2D;
    var isWord = _wordClass.Contains(_template[anchor1D]);
    if (isWord)
      (left2D, top2D) = FindNonWordBackward(left2D, top2D, anchor1D);
    var (newLeft, newTop) = FindWordBackward(left2D, top2D, anchor1D);
    return (newLeft, newTop);
  }
  internal (int, int) GetWordBeginPositionBackward(int left2D, int top2D)
  {
    var anchor1D = top2D * _width + left2D;
    (left2D, top2D) = FindWordBackward(left2D - 1, top2D, anchor1D);
    var (newLeft, newTop) = FindNonWordBackward(left2D, top2D, anchor1D);
    return (newLeft + 1, newTop);
  }
}