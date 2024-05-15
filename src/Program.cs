using VimRenaissance;
Console.Clear();
Console.CancelKeyPress += delegate { Console.Clear(); };
// Console.WriteLine($"{Console.WindowWidth},{Console.WindowHeight}");
new Editor().Run();

/// <summary>
/// </summary>
internal readonly ref struct Editor
{
  static Editor()
  {
  }
  private readonly Buffer1D _buffer;
  private readonly WordMotionV3 _wordMotion;
  public Editor()
  {
    // ReadOnlySpan<char> tmpl = File.ReadAllText("./template.txt");
    var tmpl = File.ReadAllLines("./template.txt");
    ReadOnlySpan<char> src = string.Join("", tmpl);
    _buffer = new Buffer1D(src, Cfg.WinWID);
    _wordMotion = new WordMotionV3();
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
        case 'n': MoveHorizontalWrap(-45); break;
        case '.': MoveHorizontalWrap(45); break;
        case 'a': MoveHorizontalByPattern(TextPattern.SmallWordBegin, Direction.Backward); break;
        case 's': MoveHorizontalByPattern(TextPattern.SmallWordEnd, Direction.Backward); break;
        case 'd': MoveHorizontalByPattern(TextPattern.SmallWordEnd, Direction.Forward); break;
        case 'f': MoveHorizontalByPattern(TextPattern.SmallWordBegin, Direction.Forward); break;
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
  static void MoveVertical(int unit)
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
  static void MoveHorizontal(int unit)
  {
    var (left, top) = Console.GetCursorPosition();
    var newLeft = left + unit;
    if (newLeft < 0) newLeft = 0;
    else if (newLeft >= Cfg.WinWID) newLeft = Cfg.WinWID - 1;
    Console.SetCursorPosition(newLeft, top);
  }
  /// <summary>
  /// move vertically if exceess the edge
  /// </summary>
  /// <param name="unit"></param>
  static void MoveHorizontalWrap(int unit)
  {
    var (left, top) = Console.GetCursorPosition();
    var newLeft = left + unit;
    if (newLeft < 0)
    {
      newLeft += Cfg.WinWID;
      if (--top < 0)
      {
        top = 0;
        newLeft = 0;
      }
    }
    else if (newLeft >= Cfg.WinWID)
    {
      newLeft -= Cfg.WinWID;
      if (++top >= Cfg.WinHEI)
      {
        top = Cfg.WinHEI - 1;
        newLeft = Cfg.WinWID - 1;
      }
    }
    Console.SetCursorPosition(newLeft, top);
  }
  void MoveHorizontalByPattern(TextPattern textPattern, Direction direction)
  {
    var cursor = new Cursor2D(Console.GetCursorPosition());
    var (newLeft, newTop) = (textPattern, direction) switch
    {
      (TextPattern.SmallWordBegin, Direction.Backward) => _wordMotion.ChargeUntilBeingInclusive(cursor, _buffer, Direction.Backward),
      (TextPattern.SmallWordEnd, Direction.Backward) => _wordMotion.ChargeUntilSpaceExclusive(cursor, _buffer, Direction.Backward),
      (TextPattern.SmallWordEnd, Direction.Forward) => _wordMotion.ChargeUntilSpaceExclusive(cursor, _buffer, Direction.Forward),
      (TextPattern.SmallWordBegin, Direction.Forward) => _wordMotion.ChargeUntilBeingInclusive(cursor, _buffer, Direction.Forward),
      _ => throw new NotImplementedException(),
    };
    Console.SetCursorPosition(newLeft, newTop);
  }
}
