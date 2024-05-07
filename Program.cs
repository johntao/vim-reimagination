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
        case 'q': MoveHorizontalByPattern(TextPattern.WordBeginPrev); break;
        case 'w': MoveHorizontalByPattern(TextPattern.WordEndPrev); break;
        case 'e': MoveHorizontalByPattern(TextPattern.WordEndNext); break;
        case 'r': MoveHorizontalByPattern(TextPattern.WordBeginNext); break;
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
      TextPattern.WordBeginPrev => buffer.GetNextWordBeginPosition(left, top),
      TextPattern.WordEndPrev => buffer.GetNextWordEndPosition(left, top),
      TextPattern.WordEndNext => buffer.GetNextWordEndPosition(left, top),
      TextPattern.WordBeginNext => buffer.GetNextWordBeginPosition(left, top),
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
  WordEndPrev = -2,
  WordBeginPrev,
  None,
  WordBeginNext,
  WordEndNext,
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
  private (int, int) FromWordToNonWord(int left2D, int top2D)
  {
    var begin1D = top2D * _width + left2D;
    //qqqqq wwwww,  eeeee
    //      ^
    ReadOnlySpan<char> beginAnchor = _template[begin1D..];
    //wwwww,  eeeee
    //     ^
    var beginAnchorEnd1D = beginAnchor.IndexOfAny(_searchNonWordClass);
    if (beginAnchorEnd1D == -1) return (left2D, top2D);
    //wwwww,
    //^    ^
    var rtn = beginAnchor[..beginAnchorEnd1D].ToString();
    Console.SetCursorPosition(0, Cfg.WinHEI - 1);
    Console.Write(rtn.PadRight(Cfg.WinWID, ' '));
    Console.SetCursorPosition(left2D, top2D);
    var newLeft2D = left2D + beginAnchorEnd1D;
    return (newLeft2D, top2D);
  }
  internal (int, int) GetNextWordEndPosition(int left, int top)
  {
    // var (useAltAlgorythm, offset) = ProbeAlgorithm(left, top);
    // if (useAltAlgorythm)
    //   (left, top) = FromNonWordToWord(left + offset, top);
    (left, top) = FromNonWordToWord(left + 1, top);
    var (newLeft, newTop) = FromWordToNonWord(left, top);
    return (newLeft - 1, newTop);
  }
  // private (bool, int) ProbeAlgorithm(int left, int top)
  // {
  //   var begin1D = top * _width + left;
  //   ReadOnlySpan<char> beginAnchor = _template[begin1D..(begin1D + 2)];
  //   if (beginAnchor.Length == 1) return (false, 0);
  //   else if (beginAnchor.IndexOfAny(_searchNonWordClass) > 0)
  //   {
  //     var offset = _searchNonWordClass.Contains(beginAnchor[0]) ? 0 : 1;
  //     return (true, offset);
  //   }
  //   return (false, 0);
  // }
  //TODO: add unit test
  private (int, int) FromNonWordToWord(int left2D, int top2D)
  {
    var begin1D = top2D * _width + left2D;
    //qqqqq wwwww,  eeeee rrrrr
    //           ^
    ReadOnlySpan<char> beginAnchor = _template[begin1D..];
    //,  eeeee rrrrr
    //   ^
    var beginAnchorEnd1D = beginAnchor.IndexOfAny(_searchWordClass);
    if (beginAnchorEnd1D == -1) return (left2D, top2D);
    var newLeft2D = left2D + beginAnchorEnd1D;
    return (newLeft2D, top2D);
  }
  internal (int, int) GetNextWordBeginPosition(int left, int top)
  {
    var (newLeft, newTop) = FromWordToNonWord(left, top);
    return FromNonWordToWord(newLeft, newTop);
  }
}