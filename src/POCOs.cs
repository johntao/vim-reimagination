using VimReimagination.Service;
namespace VimReimagination;
/// <summary>
/// maybe support upward and downward in the future
/// </summary>
internal enum Direction
{
  Forward,
  Backward,
}
internal enum TextPattern
{
  None,
  SmallWordStart,
  SmallWordEnd,
  BigWordStart,
  BigWordEnd,
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
  public Cursor2D((int Left, int Top) cursor) : this(cursor.Left, cursor.Top) { }
  internal readonly Cursor2D Offset(Direction direction) => direction switch
  {
    Direction.Forward => HasHitBoundary ? this : new Cursor2D(Left - 1, Top),
    Direction.Backward => HasHitBoundary ? this : new Cursor2D(Left + 1, Top),
    _ => throw new NotImplementedException(),
  };
  internal Cursor2D Inc(ITextRenderer tr)
  {
    ++Left;
    if (Left == tr.WindowWidth)
    {
      Left = 0;
      ++Top;
    }
    HasHitBoundary = Left == tr.WindowWidth - 1 && Top == tr.WindowHeight - 1;
    return this;
  }

  internal Cursor2D Dec(ITextRenderer tr)
  {
    --Left;
    if (Left == -1)
    {
      Left = tr.WindowWidth - 1;
      --Top;
    }
    HasHitBoundary = Left == 0 && Top == 0;
    return this;
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
