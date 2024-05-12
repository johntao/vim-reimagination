namespace VimRenaissance;
internal static class Cfg
{
  //210,419,628,837
  internal const int WinWID = 209; //Console.WindowWidth;
  internal const int WinHEI = 51; //Console.WindowHeight;
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
  public static Cursor2D operator ++(Cursor2D a)
  {
    ++a.Left;
    return a;
  }
  public static Cursor2D operator --(Cursor2D a)
  {
    --a.Left;
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
