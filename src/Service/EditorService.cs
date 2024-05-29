using System.Text;
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
internal class EditorService(ITextRenderer tr) : EditorService.IRun
{
  internal interface IRun
  {
    void Run(Dictionary<char, NormalCommand> keymap);
  }
  private static readonly SmallWordMotionPattern _smallWordMotion = new();
  private static readonly BigWordMotionPattern _bigWordMotion = new();
  static EditorService() { }
  private BufferService _buffer;
  private readonly ITextRenderer _tr = tr;
  // public EditorService() => throw new NotSupportedException();
  private int PreviousWindowWidth { get; set; }
  public void Run(Dictionary<char, NormalCommand> keymap)
  {
    var rrr = new StringBuilder();
    // rrr.
    while (true)
    {
      if (PreviousWindowWidth != Console.WindowWidth)
      {
        // var tmpl = File.ReadAllLines("./assets/template.txt");
        // ReadOnlySpan<char> src = string.Join("", tmpl);
        // _buffer = new Buffer1D(src, Cfg.WinWID);
        // _tr.Write(src.ToString());
        // _tr.SetCursorPosition(0, 0);
        PreviousWindowWidth = Console.WindowWidth;
        // _tr.Clear();
        // _tr.Write(_buffer.ToString());
        // _tr.SetCursorPosition(0, 0);
      }
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
      case NormalCommand.MoveHorizontalFullScreenBackwardStop: MoveHorizontalStop(-Cfg.WinWID); break;
      case NormalCommand.MoveVerticalFullScreenForwardStop: MoveVerticalStop(Cfg.WinHEI); break;
      case NormalCommand.MoveVerticalFullScreenBackwardStop: MoveVerticalStop(-Cfg.WinHEI); break;
      case NormalCommand.MoveHorizontalFullScreenForwardStop: MoveHorizontalStop(Cfg.WinWID); break;
    }
  }
  /// <summary>
  /// stops at the edge
  /// </summary>
  /// <param name="unit"></param>
  void MoveVerticalStop(int unit)
  {
    var (left, top) = _tr.GetCursorPosition();
    var newTop = top + unit;
    if (newTop < 0) newTop = 0;
    else if (newTop >= Cfg.WinHEI) newTop = Cfg.WinHEI - 1;
    _tr.SetCursorPosition(left, newTop);
  }
  /// <summary>
  /// stops at the edge
  /// </summary>
  /// <param name="unit"></param>
  void MoveHorizontalStop(int unit)
  {
    var (left, top) = _tr.GetCursorPosition();
    var newLeft = left + unit;
    if (newLeft < 0) newLeft = 0;
    else if (newLeft >= Cfg.WinWID) newLeft = Cfg.WinWID - 1;
    _tr.SetCursorPosition(newLeft, top);
  }
  /// <summary>
  /// move vertically if exceess the edge
  /// </summary>
  /// <param name="unit"></param>
  void MoveHorizontal(int unit)
  {
    var (left, top) = _tr.GetCursorPosition();
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
    _tr.SetCursorPosition(newLeft, top);
  }
  void MoveHorizontalByPattern(TextPattern textPattern, Direction direction)
  {
    var cursor = new Cursor2D(_tr.GetCursorPosition());
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
    _tr.SetCursorPosition(newLeft, newTop);
  }
}