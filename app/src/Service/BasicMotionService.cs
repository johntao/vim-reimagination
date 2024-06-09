namespace VimReimagination.Service;

internal class BasicMotionService(IWindow win, ICursor cur) : BasicMotionService.IGo
{
  internal interface IGo
  {
    /// <summary>
    /// move vertically if exceess the edge
    /// </summary>
    /// <param name="unit"></param>
    void Row(int unit);
    /// <summary>
    /// stops at the edge
    /// </summary>
    /// <param name="unit"></param>
    void RowStop(int unit);

    /// <summary>
    /// stops at the edge
    /// </summary>
    /// <param name="unit"></param>
    void Col(int unit);
  }
  private readonly IWindow _win = win;
  private readonly ICursor _cur = cur;
  public void Row(int unit)
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
  public void RowStop(int unit)
  {
    var (left, top) = _cur.GetCursorPosition2D();
    var newLeft = left + unit;
    if (newLeft < 0) newLeft = 0;
    else if (newLeft >= _win.Width) newLeft = _win.Width - 1;
    _cur.SetCursorPosition(newLeft, top);
  }
  public void Col(int unit)
  {
    var (left, top) = _cur.GetCursorPosition2D();
    var newTop = top + unit;
    if (newTop < 0) newTop = 0;
    else if (newTop >= _win.Height) newTop = _win.Height - 1;
    _cur.SetCursorPosition(left, newTop);
  }
}