namespace VimRenaissance;
/// <summary>
/// Can't tell the advantage of using ref struct, but it's required to use ref struct for `ReadOnlySpan<char> Content`
/// </summary>
internal ref struct Buffer1D
{
  internal Cursor2D Cursor2D { get; private set; }
  private int _cursor1D;
  private Direction _direction;
  internal Buffer1D(ReadOnlySpan<char> Content, int Width)
  {
    this.Content = Content;
    this.Width = Width;
  }
  internal readonly ReadOnlySpan<char> Content;
  internal readonly int Width;
  internal char Previous { get; private set; }
  internal readonly char Current => Content[_cursor1D];
  internal void Reset(Cursor2D cursor2D, Direction direction)
  {
    Cursor2D = cursor2D;
    _cursor1D = cursor2D.Top * Width + cursor2D.Left;
    Previous = Current;
    _direction = direction;
  }
  internal readonly bool HasNext() => _direction switch
  {
    Direction.Forward => _cursor1D + 1 < Content.Length,
    Direction.Backward => _cursor1D - 1 >= 0,
    _ => throw new NotImplementedException(),
  };
  internal bool HasNext_Move()
  {
    var hasNext = HasNext();
    Previous = Current;
    if (hasNext)
      MoveCursor();
    return hasNext;
  }
  private void MoveCursor()
  {
    switch (_direction)
    {
      case Direction.Forward:
        ++_cursor1D;
        ++Cursor2D;
        break;
      case Direction.Backward:
        --_cursor1D;
        --Cursor2D;
        break;
      default:
        throw new NotImplementedException();
    }
  }
}