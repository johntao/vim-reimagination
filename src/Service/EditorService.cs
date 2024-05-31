using VimReimagination.WordMotion;
namespace VimReimagination.Service;
internal enum MotionCommand
{
  None,
  Row_Pattern_BigWordStart_Back,
  Row_Pattern_BigWordEnd_Back,
  Row_Pattern_BigWordStart_Forth,
  Row_Pattern_BigWordEnd_Forth,
  Row_Pattern_SmallWordStart_Back,
  Row_Pattern_SmallWordEnd_Back,
  Row_Pattern_SmallWordStart_Forth,
  Row_Pattern_SmallWordEnd_Forth,
  Row_1unit_Back,
  Col_1unit_Forth,
  Col_1unit_Back,
  Row_1unit_Forth,
  Row_FullScreen_Back_StopOnEdge,
  Col_FullScreen_Forth_StopOnEdge,
  Col_FullScreen_Back_StopOnEdge,
  Row_FullScreen_Forth_StopOnEdge,
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
    void Run(Dictionary<char, MotionCommand> keymap);
  }
  private static readonly SmallWordMotionPattern _smallWordMotion = new();
  private static readonly BigWordMotionPattern _bigWordMotion = new();
  static EditorService() { }
  private readonly IBufferService _buffer = buffer;
  private readonly IReadWrite _tr = tr;
  private readonly IWindow _win = win;
  private readonly ICursor _cur = cur;
  public void Run(Dictionary<char, MotionCommand> keymap)
  {
    while (true)
    {
      _buffer.IfWindowResizedThenReloadBuffer();
      var readkey = _tr.ReadKey();
      var keychar = readkey.KeyChar;
      if (keymap.TryGetValue(keychar, out MotionCommand value))
        ProcessCommand(value);
    }
  }
  private void ProcessCommand(MotionCommand cmd)
  {
    switch (cmd)
    {
      case MotionCommand.Row_Pattern_BigWordStart_Back: MoveHorizontalByPattern(TextPattern.BigWordStart, Direction.Backward); break;
      case MotionCommand.Row_Pattern_BigWordEnd_Back: MoveHorizontalByPattern(TextPattern.BigWordEnd, Direction.Backward); break;
      case MotionCommand.Row_Pattern_BigWordStart_Forth: MoveHorizontalByPattern(TextPattern.BigWordStart, Direction.Forward); break;
      case MotionCommand.Row_Pattern_BigWordEnd_Forth: MoveHorizontalByPattern(TextPattern.BigWordEnd, Direction.Forward); break;
      case MotionCommand.Row_Pattern_SmallWordStart_Back: MoveHorizontalByPattern(TextPattern.SmallWordStart, Direction.Backward); break;
      case MotionCommand.Row_Pattern_SmallWordEnd_Back: MoveHorizontalByPattern(TextPattern.SmallWordEnd, Direction.Backward); break;
      case MotionCommand.Row_Pattern_SmallWordStart_Forth: MoveHorizontalByPattern(TextPattern.SmallWordStart, Direction.Forward); break;
      case MotionCommand.Row_Pattern_SmallWordEnd_Forth: MoveHorizontalByPattern(TextPattern.SmallWordEnd, Direction.Forward); break;
      case MotionCommand.Row_1unit_Back: MoveHorizontal(-1); break;
      case MotionCommand.Col_1unit_Forth: MoveVerticalStop(1); break;
      case MotionCommand.Col_1unit_Back: MoveVerticalStop(-1); break;
      case MotionCommand.Row_1unit_Forth: MoveHorizontal(1); break;
      case MotionCommand.Row_FullScreen_Back_StopOnEdge: MoveHorizontalStop(-_win.Width); break;
      case MotionCommand.Col_FullScreen_Forth_StopOnEdge: MoveVerticalStop(_win.Height); break;
      case MotionCommand.Col_FullScreen_Back_StopOnEdge: MoveVerticalStop(-_win.Height); break;
      case MotionCommand.Row_FullScreen_Forth_StopOnEdge: MoveHorizontalStop(_win.Width); break;
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
    else if (newTop >= _win.Height) newTop = _win.Height - 1;
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
    else if (newLeft >= _win.Width) newLeft = _win.Width - 1;
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
      newLeft += _win.Width;
      if (--top < 0)
      {
        top = 0;
        newLeft = 0;
      }
    }
    else if (newLeft >= _win.Width)
    {
      newLeft -= _win.Width;
      if (++top >= _win.Height)
      {
        top = _win.Height - 1;
        newLeft = _win.Width - 1;
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