namespace VimRenaissance;
public enum Direction
{
  Forward,
  Backward,
}
internal class WordMotionV2 : IWordMotion
{
  public (int, int) GetSmallWordEndForward(int left2D, int top2D, Buffer1D buffer)
  {
    buffer.Reset(left2D, top2D, Direction.Forward);
    if (!buffer.Move()) return (left2D, top2D);
    if (buffer.Current == CharKind.Space)
    {
      while (buffer.MoveCheck()) ;
      while (buffer.MoveCheck()) ;
      return (buffer.Left2D - 1, buffer.Top2D);
    }
    else
    {
      while (buffer.MoveCheck()) ;
      return (buffer.Left2D - 1, buffer.Top2D);
    }
  }
  public (int, int) GetSmallWordBeginForward(int left2D, int top2D, Buffer1D buffer)
  {
    buffer.Reset(left2D, top2D, Direction.Forward);
    if (!buffer.HasNext()) return (left2D, top2D);
    while (buffer.MoveCheck()) ;
    if (buffer.Current == CharKind.Space)
    {
      while (buffer.MoveCheck()) ;
      return (buffer.Left2D, buffer.Top2D);
    }
    else
    {
      return (buffer.Left2D, buffer.Top2D);
    }
  }
  public (int, int) GetSmallWordEndBackward(int left2D, int top2D, Buffer1D buffer)
  {
    buffer.Reset(left2D, top2D, Direction.Backward);
    if (!buffer.HasNext()) return (left2D, top2D);
    while (buffer.MoveCheck()) ;
    if (buffer.Current == CharKind.Space)
    {
      while (buffer.MoveCheck()) ;
      return (buffer.Left2D, buffer.Top2D);
    }
    else
    {
      return (buffer.Left2D, buffer.Top2D);
    }
  }
  public (int, int) GetSmallWordBeginBackward(int left2D, int top2D, Buffer1D buffer)
  {
    buffer.Reset(left2D, top2D, Direction.Backward);
    if (!buffer.Move()) return (left2D, top2D);
    if (buffer.Current == CharKind.Space)
    {
      while (buffer.MoveCheck()) ;
      while (buffer.MoveCheck()) ;
      return (buffer.Left2D + 1, buffer.Top2D);
    }
    else
    {
      while (buffer.MoveCheck()) ;
      return (buffer.Left2D + 1, buffer.Top2D);
    }
  }
}