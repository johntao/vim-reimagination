using VimReimagination.Service;
namespace VimReimagination.Model;

internal struct Cursor2D(int Left, int Top, (int Width, int Height) Window)
{
  private readonly (int Width, int Height) Window = Window;
  internal int Left = Left;
  internal int Top = Top;
  internal bool HasHitBoundary = false;
  public Cursor2D(IWindow win, ICursor cur) : this(cur.GetCursorPosition2D(), win) { }
  private Cursor2D((int Left, int Top) cursor, IWindow win) : this(cursor.Left, cursor.Top, win.Window) { }
  internal readonly Cursor2D Offset(Direction direction) => direction switch
  {
    Direction.Forward => HasHitBoundary ? this : new Cursor2D(Left - 1, Top, Window),
    Direction.Backward => HasHitBoundary ? this : new Cursor2D(Left + 1, Top, Window),
    _ => throw new NotImplementedException(),
  };
  public static Cursor2D operator ++(Cursor2D a)
  {
    ++a.Left;
    if (a.Left == a.Window.Width)
    {
      a.Left = 0;
      ++a.Top;
    }
    a.HasHitBoundary = a.Left == a.Window.Width - 1 && a.Top == a.Window.Height - 1;
    return a;
  }
  public static Cursor2D operator --(Cursor2D a)
  {
    --a.Left;
    if (a.Left == -1)
    {
      a.Left = a.Window.Width - 1;
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
