using VimReimagination.Model;
using VimReimagination.Service;
using VimReimagination.WordMotion;
using Cmd = VimReimagination.Model.Commands.All;

internal class CommandService(IWindow win, ICursor cur, IBufferService buffer) : CommandService.IProcess
{
  #region types and static
  enum TextPattern
  {
    None,
    SmallWordStart,
    SmallWordEnd,
    BigWordStart,
    BigWordEnd,
  }
  internal interface IProcess
  {
    void Process(Cmd value);
  }
  private static readonly SmallWordMotionPattern _smallWordMotion = new();
  private static readonly BigWordMotionPattern _bigWordMotion = new();
  #endregion
  private readonly IWindow _win = win;
  private readonly ICursor _cur = cur;
  private readonly IBufferService _buffer = buffer;
  public void Process(Cmd cmd)
  {
    switch (cmd)
    {
      case Cmd.Row_Pattern_BigWordStart_Back: MoveHorizontalByPattern(TextPattern.BigWordStart, Direction.Backward); break;
      case Cmd.Row_Pattern_BigWordEnd_Back: MoveHorizontalByPattern(TextPattern.BigWordEnd, Direction.Backward); break;
      case Cmd.Row_Pattern_BigWordStart_Forth: MoveHorizontalByPattern(TextPattern.BigWordStart, Direction.Forward); break;
      case Cmd.Row_Pattern_BigWordEnd_Forth: MoveHorizontalByPattern(TextPattern.BigWordEnd, Direction.Forward); break;
      case Cmd.Row_Pattern_SmallWordStart_Back: MoveHorizontalByPattern(TextPattern.SmallWordStart, Direction.Backward); break;
      case Cmd.Row_Pattern_SmallWordEnd_Back: MoveHorizontalByPattern(TextPattern.SmallWordEnd, Direction.Backward); break;
      case Cmd.Row_Pattern_SmallWordStart_Forth: MoveHorizontalByPattern(TextPattern.SmallWordStart, Direction.Forward); break;
      case Cmd.Row_Pattern_SmallWordEnd_Forth: MoveHorizontalByPattern(TextPattern.SmallWordEnd, Direction.Forward); break;
      case Cmd.Row_1unit_Back: MoveHorizontal(-1); break;
      case Cmd.Col_1unit_Forth: MoveVerticalStop(1); break;
      case Cmd.Col_1unit_Back: MoveVerticalStop(-1); break;
      case Cmd.Row_1unit_Forth: MoveHorizontal(1); break;
      case Cmd.Row_FullScreen_Back_StopOnEdge: MoveHorizontalStop(-_win.Width); break;
      case Cmd.Col_FullScreen_Forth_StopOnEdge: MoveVerticalStop(_win.Height); break;
      case Cmd.Col_FullScreen_Back_StopOnEdge: MoveVerticalStop(-_win.Height); break;
      case Cmd.Row_FullScreen_Forth_StopOnEdge: MoveHorizontalStop(_win.Width); break;
      case Cmd.SmallDelete: _buffer.SetChar(' '); break;
      case Cmd.SaveFile: _buffer.SaveFile(); break;
    }
  }
  private void MoveHorizontalByPattern(TextPattern textPattern, Direction direction)
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

  /// <summary>
  /// stops at the edge
  /// </summary>
  /// <param name="unit"></param>
  void MoveVerticalStop(int unit)
  {
    var (left, top) = _cur.GetCursorPosition2D();
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
    var (left, top) = _cur.GetCursorPosition2D();
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
    var (left, top) = _cur.GetCursorPosition2D();
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
}