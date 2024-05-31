using VimReimagination.WordMotion;
namespace VimReimagination.Service;
internal enum NormalCommand
{
  None,
  MoveHorizontalByPatternBigWordStartBackward,
  MoveHorizontalByPatternBigWordEndBackward,
  MoveHorizontalByPatternBigWordStartForward,
  MoveHorizontalByPatternBigWordEndForward,
  MoveHorizontalByPatternSmallWordStartBackward,
  MoveHorizontalByPatternSmallWordEndBackward,
  MoveHorizontalByPatternSmallWordStartForward,
  MoveHorizontalByPatternSmallWordEndForward,
  MoveHorizontal1uBackward,
  MoveVertical1uForward,
  MoveVertical1uBackward,
  MoveHorizontal1uForward,
  MoveHorizontalFullScreenBackwardStop,
  MoveVerticalFullScreenForwardStop,
  MoveVerticalFullScreenBackwardStop,
  MoveHorizontalFullScreenForwardStop,
}
/// <summary>
/// Can't tell the advantage of using ref struct, but it's required to use ref struct for `Buffer1D _buffer`
/// I wonder the perf difference between ref struct and static class
/// should benchmark it someday
/// </summary>
internal class EditorService(IReadWrite tr, IBufferService buffer, IWindow win, ICursor cur) : EditorService.IRun
{
  internal interface IRun
  {
    void Run(Dictionary<char, NormalCommand> keymap);
  }
  private static readonly SmallWordMotionPattern _smallWordMotion = new();
  private static readonly BigWordMotionPattern _bigWordMotion = new();
  static EditorService() { }
  private readonly IBufferService _buffer = buffer;
  private readonly IReadWrite _tr = tr;
  private readonly IWindow _win = win;
  private readonly ICursor _cur = cur;
  public void Run(Dictionary<char, NormalCommand> keymap)
  {
    while (true)
    {
      _buffer.IfWindowResizedThenReloadBuffer();
      var readkey = _tr.ReadKey();
      var keychar = readkey.KeyChar;
      if (keymap.TryGetValue(keychar, out NormalCommand value))
        ProcessCommand(value);
    }
  }
  private void ProcessCommand(NormalCommand cmd)
  {
    switch (cmd)
    {
      case NormalCommand.MoveHorizontalByPatternBigWordStartBackward: MoveHorizontalByPattern(TextPattern.BigWordStart, Direction.Backward); break;
      case NormalCommand.MoveHorizontalByPatternBigWordEndBackward: MoveHorizontalByPattern(TextPattern.BigWordEnd, Direction.Backward); break;
      case NormalCommand.MoveHorizontalByPatternBigWordStartForward: MoveHorizontalByPattern(TextPattern.BigWordStart, Direction.Forward); break;
      case NormalCommand.MoveHorizontalByPatternBigWordEndForward: MoveHorizontalByPattern(TextPattern.BigWordEnd, Direction.Forward); break;
      case NormalCommand.MoveHorizontalByPatternSmallWordStartBackward: MoveHorizontalByPattern(TextPattern.SmallWordStart, Direction.Backward); break;
      case NormalCommand.MoveHorizontalByPatternSmallWordEndBackward: MoveHorizontalByPattern(TextPattern.SmallWordEnd, Direction.Backward); break;
      case NormalCommand.MoveHorizontalByPatternSmallWordStartForward: MoveHorizontalByPattern(TextPattern.SmallWordStart, Direction.Forward); break;
      case NormalCommand.MoveHorizontalByPatternSmallWordEndForward: MoveHorizontalByPattern(TextPattern.SmallWordEnd, Direction.Forward); break;
      case NormalCommand.MoveHorizontal1uBackward: MoveHorizontal(-1); break;
      case NormalCommand.MoveVertical1uForward: MoveVerticalStop(1); break;
      case NormalCommand.MoveVertical1uBackward: MoveVerticalStop(-1); break;
      case NormalCommand.MoveHorizontal1uForward: MoveHorizontal(1); break;
      case NormalCommand.MoveHorizontalFullScreenBackwardStop: MoveHorizontalStop(-_win.WindowWidth); break;
      case NormalCommand.MoveVerticalFullScreenForwardStop: MoveVerticalStop(_win.WindowHeight); break;
      case NormalCommand.MoveVerticalFullScreenBackwardStop: MoveVerticalStop(-_win.WindowHeight); break;
      case NormalCommand.MoveHorizontalFullScreenForwardStop: MoveHorizontalStop(_win.WindowWidth); break;
    }
  }
  /// <summary>
  /// stops at the edge
  /// </summary>
  /// <param name="unit"></param>
  void MoveVerticalStop(int unit)
  {
    var (left, top) = _cur.GetCursorPosition();
    var newTop = top + unit;
    if (newTop < 0) newTop = 0;
    else if (newTop >= _win.WindowHeight) newTop = _win.WindowHeight - 1;
    _cur.SetCursorPosition(left, newTop);
  }
  /// <summary>
  /// stops at the edge
  /// </summary>
  /// <param name="unit"></param>
  void MoveHorizontalStop(int unit)
  {
    var (left, top) = _cur.GetCursorPosition();
    var newLeft = left + unit;
    if (newLeft < 0) newLeft = 0;
    else if (newLeft >= _win.WindowWidth) newLeft = _win.WindowWidth - 1;
    _cur.SetCursorPosition(newLeft, top);
  }
  /// <summary>
  /// move vertically if exceess the edge
  /// </summary>
  /// <param name="unit"></param>
  void MoveHorizontal(int unit)
  {
    var (left, top) = _cur.GetCursorPosition();
    var newLeft = left + unit;
    if (newLeft < 0)
    {
      newLeft += _win.WindowWidth;
      if (--top < 0)
      {
        top = 0;
        newLeft = 0;
      }
    }
    else if (newLeft >= _win.WindowWidth)
    {
      newLeft -= _win.WindowWidth;
      if (++top >= _win.WindowHeight)
      {
        top = _win.WindowHeight - 1;
        newLeft = _win.WindowWidth - 1;
      }
    }
    _cur.SetCursorPosition(newLeft, top);
  }
  void MoveHorizontalByPattern(TextPattern textPattern, Direction direction)
  {
    var cursor = new Cursor2D(_win, _cur);
    var (newLeft, newTop) = (textPattern, direction) switch
    {
      (TextPattern.SmallWordStart, Direction.Backward) => _smallWordMotion.ChargeUntilBlankExclusive(cursor, _buffer, Direction.Backward),
      (TextPattern.SmallWordEnd, Direction.Backward) => _smallWordMotion.ChargeUntilMatterInclusive(cursor, _buffer, Direction.Backward),
      (TextPattern.SmallWordStart, Direction.Forward) => _smallWordMotion.ChargeUntilMatterInclusive(cursor, _buffer, Direction.Forward),
      (TextPattern.SmallWordEnd, Direction.Forward) => _smallWordMotion.ChargeUntilBlankExclusive(cursor, _buffer, Direction.Forward),
      (TextPattern.BigWordStart, Direction.Backward) => _bigWordMotion.ChargeUntilBlankExclusive(cursor, _buffer, Direction.Backward),
      (TextPattern.BigWordEnd, Direction.Backward) => _bigWordMotion.ChargeUntilMatterInclusive(cursor, _buffer, Direction.Backward),
      (TextPattern.BigWordStart, Direction.Forward) => _bigWordMotion.ChargeUntilMatterInclusive(cursor, _buffer, Direction.Forward),
      (TextPattern.BigWordEnd, Direction.Forward) => _bigWordMotion.ChargeUntilBlankExclusive(cursor, _buffer, Direction.Forward),
      _ => throw new NotImplementedException(),
    };
    _cur.SetCursorPosition(newLeft, newTop);
  }
}