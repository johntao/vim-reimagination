namespace VimRenaissance;
internal static class Cfg
{
  /// <summary>
  /// 204,46 in vscode integrated terminal
  /// 209,51 in windows terminal (wide)
  /// 156,41 in windows terminal (narrow)
  /// </summary>
  //WideWidth: 210,419,628,837
  //NarrowWidth: 157,625,781,937
  internal const int WinWID = 156; //Console.WindowWidth;
  internal const int WinHEI = 41; //Console.WindowHeight;
}
internal enum Direction
{
  Forward,
  Backward,
}
internal enum TextPattern
{
  WordEndBackward = -2,
  WordBeginBackward,
  None,
  WordBeginFoward,
  WordEndForward,
}
internal enum CharKind
{
  None,
  Primary,
  Secondary,
  Space,
}
internal struct Cursor2D(int Left, int Top)
{
  internal int Left = Left;
  internal int Top = Top;
  internal bool HasHitBoundary = false;
  internal readonly Cursor2D Offset(Direction direction) => direction switch
  {
    Direction.Backward => HasHitBoundary ? this : new Cursor2D(Left - 1, Top),
    Direction.Forward => HasHitBoundary ? this : new Cursor2D(Left + 1, Top),
    _ => throw new NotImplementedException(),
  };
  public static Cursor2D operator ++(Cursor2D a)
  {
    ++a.Left;
    if (a.Left == Cfg.WinWID)
    {
      a.Left = 0;
      ++a.Top;
    }
    a.HasHitBoundary = a.Left == Cfg.WinWID - 1 && a.Top == Cfg.WinHEI - 1;
    return a;
  }
  public static Cursor2D operator --(Cursor2D a)
  {
    --a.Left;
    if (a.Left == -1)
    {
      a.Left = Cfg.WinWID - 1;
      --a.Top;
    }
    a.HasHitBoundary = a.Left == 0 && a.Top == 0;
    return a;
  }
  public static Cursor2D operator -(Cursor2D a, int b)
  {
    a.Left -= b;
    return a;
  }
  public static Cursor2D operator +(Cursor2D a, int b)
  {
    a.Left += b;
    return a;
  }
  internal readonly void Deconstruct(out int Left, out int Top)
  {
    Left = this.Left;
    Top = this.Top;
  }
}
