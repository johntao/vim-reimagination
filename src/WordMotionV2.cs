namespace VimRenaissance;
/// <summary>
/// this implementation fully comply with vim word motion
/// </summary>
internal class WordMotionV2 : IWordMotionV2
{
  public Cursor2D GetSmallWordEndForward(int left2D, int top2D, Buffer1D buffer)
  {
    buffer.Reset(left2D, top2D, Direction.Forward);
    if (!buffer.Move(out var prev)) return new (left2D, top2D);
    while (buffer.MoveCheck()) ;
    if (prev == CharKind.Space)
    {
      while (buffer.MoveCheck()) ;
      return buffer.Cursor2D - 1;
    }
    else
    {
      return buffer.Cursor2D - 1;
    }
  }
  public Cursor2D GetSmallWordBeginForward(int left2D, int top2D, Buffer1D buffer)
  {
    buffer.Reset(left2D, top2D, Direction.Forward);
    if (!buffer.HasNext()) return new (left2D, top2D);
    while (buffer.MoveCheck()) ;
    if (buffer.Current == CharKind.Space)
    {
      while (buffer.MoveCheck()) ;
      return buffer.Cursor2D;
    }
    else
    {
      return buffer.Cursor2D;
    }
  }
  public Cursor2D GetSmallWordEndBackward(int left2D, int top2D, Buffer1D buffer)
  {
    buffer.Reset(left2D, top2D, Direction.Backward);
    if (!buffer.HasNext()) return new (left2D, top2D);
    while (buffer.MoveCheck()) ;
    if (buffer.Current == CharKind.Space)
    {
      while (buffer.MoveCheck()) ;
      return buffer.Cursor2D;
    }
    else
    {
      return buffer.Cursor2D;
    }
  }
  public Cursor2D GetSmallWordBeginBackward(int left2D, int top2D, Buffer1D buffer)
  {
    buffer.Reset(left2D, top2D, Direction.Backward);
    if (!buffer.Move(out var prev)) return new (left2D, top2D);
    while (buffer.MoveCheck()) ;
    if (prev == CharKind.Space)
    {
      while (buffer.MoveCheck()) ;
      return buffer.Cursor2D + 1;
    }
    else
    {
      return buffer.Cursor2D + 1;
    }
  }
}