using VimRenaissance;
/// <summary>
/// Can't tell the advantage of using ref struct, but it's required to use ref struct for `Buffer1D _buffer`
/// I wonder the perf difference between ref struct and static class
/// should benchmark it someday
/// </summary>
internal readonly ref struct Editor
{
  static Editor()
  {
  }
  private readonly Buffer1D _buffer;
  private readonly SmallWordMotion _smallWordMotion;
  private readonly BigWordMotion _bigWordMotion;
  public Editor()
  {
    var tmpl = File.ReadAllLines("./template.txt");
    ReadOnlySpan<char> src = string.Join("", tmpl);
    _buffer = new Buffer1D(src, Cfg.WinWID);
    _smallWordMotion = new SmallWordMotion();
    _bigWordMotion = new BigWordMotion();
    Console.Write(src.ToString());
    Console.SetCursorPosition(0, 0);
  }
  internal void Run()
  {
    while (true)
    {
      var readkey = Console.ReadKey(true);
      var keychar = readkey.KeyChar;
      switch (keychar)
      {
        case 'n': MoveHorizontalWrap(-45); break;
        case '.': MoveHorizontalWrap(45); break;
        case 'q': MoveHorizontalByPattern(TextPattern.BigWordStart, Direction.Backward); break;
        case 'w': MoveHorizontalByPattern(TextPattern.BigWordEnd, Direction.Backward); break;
        case 'e': MoveHorizontalByPattern(TextPattern.BigWordStart, Direction.Forward); break;
        case 'r': MoveHorizontalByPattern(TextPattern.BigWordEnd, Direction.Forward); break;
        case 'a': MoveHorizontalByPattern(TextPattern.SmallWordStart, Direction.Backward); break;
        case 's': MoveHorizontalByPattern(TextPattern.SmallWordEnd, Direction.Backward); break;
        case 'd': MoveHorizontalByPattern(TextPattern.SmallWordStart, Direction.Forward); break;
        case 'f': MoveHorizontalByPattern(TextPattern.SmallWordEnd, Direction.Forward); break;
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
      (TextPattern.SmallWordStart, Direction.Backward) => _smallWordMotion.ChargeUntilSpaceExclusive(cursor, _buffer, Direction.Backward),
      (TextPattern.SmallWordEnd, Direction.Backward) => _smallWordMotion.ChargeUntilBeingInclusive(cursor, _buffer, Direction.Backward),
      (TextPattern.SmallWordStart, Direction.Forward) => _smallWordMotion.ChargeUntilBeingInclusive(cursor, _buffer, Direction.Forward),
      (TextPattern.SmallWordEnd, Direction.Forward) => _smallWordMotion.ChargeUntilSpaceExclusive(cursor, _buffer, Direction.Forward),
      (TextPattern.BigWordStart, Direction.Backward) => _bigWordMotion.ChargeUntilSpaceExclusive(cursor, _buffer, Direction.Backward),
      (TextPattern.BigWordEnd, Direction.Backward) => _bigWordMotion.ChargeUntilBeingInclusive(cursor, _buffer, Direction.Backward),
      (TextPattern.BigWordStart, Direction.Forward) => _bigWordMotion.ChargeUntilBeingInclusive(cursor, _buffer, Direction.Forward),
      (TextPattern.BigWordEnd, Direction.Forward) => _bigWordMotion.ChargeUntilSpaceExclusive(cursor, _buffer, Direction.Forward),
      _ => throw new NotImplementedException(),
    };
    Console.SetCursorPosition(newLeft, newTop);
  }
}
