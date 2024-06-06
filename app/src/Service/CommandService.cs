using VimReimagination.Model;
using VimReimagination.Service;
using Cmd = VimReimagination.Model.Commands.All;

internal class CommandService(
  IWindow win,
  IBufferService buffer,
  PatternMotionService.IGo pm,
  BasicMotionService.IGo bm
) : CommandService.IProcess
{
  #region types and static
  internal interface IProcess
  {
    void Process(Cmd value);
  }
  #endregion
  private readonly BasicMotionService.IGo _bm = bm;
  private readonly PatternMotionService.IGo _pm = pm;
  private readonly IWindow _win = win;
  private readonly IBufferService _buffer = buffer;
  public void Process(Cmd cmd)
  {
    switch (cmd)
    {
      case Cmd.Row_Pattern_BigWordStart_Back:    _pm.Row(TextPattern.BigWordStart, Direction.RowBackward);   break;
      case Cmd.Row_Pattern_BigWordEnd_Back:      _pm.Row(TextPattern.BigWordEnd, Direction.RowBackward);     break;
      case Cmd.Row_Pattern_BigWordStart_Forth:   _pm.Row(TextPattern.BigWordStart, Direction.RowForward);    break;
      case Cmd.Row_Pattern_BigWordEnd_Forth:     _pm.Row(TextPattern.BigWordEnd, Direction.RowForward);      break;
      case Cmd.Row_Pattern_SmallWordStart_Back:  _pm.Row(TextPattern.SmallWordStart, Direction.RowBackward); break;
      case Cmd.Row_Pattern_SmallWordEnd_Back:    _pm.Row(TextPattern.SmallWordEnd, Direction.RowBackward);   break;
      case Cmd.Row_Pattern_SmallWordStart_Forth: _pm.Row(TextPattern.SmallWordStart, Direction.RowForward);  break;
      case Cmd.Row_Pattern_SmallWordEnd_Forth:   _pm.Row(TextPattern.SmallWordEnd, Direction.RowForward);    break;
      case Cmd.Row_1unit_Back:  _bm.Row(-1); break;
      case Cmd.Col_1unit_Forth: _bm.Col(1);  break;
      case Cmd.Col_1unit_Back:  _bm.Col(-1); break;
      case Cmd.Row_1unit_Forth: _bm.Row(1);  break;
      case Cmd.Row_FullScreen_Back_StopOnEdge:  _bm.RowStop(-_win.Width); break;
      case Cmd.Col_FullScreen_Forth_StopOnEdge: _bm.Col(_win.Height);     break;
      case Cmd.Col_FullScreen_Back_StopOnEdge:  _bm.Col(-_win.Height);    break;
      case Cmd.Row_FullScreen_Forth_StopOnEdge: _bm.RowStop(_win.Width);  break;
      case Cmd.SmallDelete: _buffer.SetChar(' '); break;
      case Cmd.SaveFile: _buffer.SaveFile(); break;
    }
  }
}