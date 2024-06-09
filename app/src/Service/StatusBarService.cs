namespace VimReimagination.Service;
internal class StatusBarService(IWindow win, ICursor cur, IReadWrite rw) : StatusBarService.IWrite
{
  internal interface IWrite
  {
    void Write(string message);
  }
  private readonly IWindow _win = win;
  private readonly ICursor _cur = cur;
  private readonly IReadWrite _rw = rw;
  public void Write(string message)
  {
    var prev = _cur.GetCursorPosition2D();
    _cur.SetCursorPosition(0, _win.OuterHeight - 1);
    _rw.Write(message.PadRight(_win.Width));
    _cur.SetCursorPosition(prev);
  }
}