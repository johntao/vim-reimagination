public ref struct Buffer1D
{
  private readonly ReadOnlySpan<char> _primary = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_";
  private readonly ReadOnlySpan<char> _secondary = ",.:+=-*/\\(){}[]<>!@#$%^&*;\"'`~|?";
  private readonly ReadOnlySpan<char> _space = " \t\n\r\f\v";
  private readonly SearchValues<char> _searchPrimary;
  private readonly SearchValues<char> _searchSecondary;
  private readonly SearchValues<char> _searchSpace;
  private int _cursor1D;
  internal Cursor2D Cursor2D { get; private set; }
  private Direction _direction;
  private CharKind _prev;
  public Buffer1D(ReadOnlySpan<char> Content, int Width)
  {
    this.Content = Content;
    this.Width = Width;
    _searchPrimary = SearchValues.Create(_primary);
    _searchSecondary = SearchValues.Create(_secondary);
    _searchSpace = SearchValues.Create(_space);
  }
  internal readonly ReadOnlySpan<char> Content;
  internal readonly int Width;
  public readonly CharKind Current => Content[_cursor1D] switch
  {
    var c when _searchPrimary.Contains(c) => CharKind.Primary,
    var c when _searchSecondary.Contains(c) => CharKind.Secondary,
    var c when _searchSpace.Contains(c) => CharKind.Space,
    _ => CharKind.None,
  };
  public void Reset(int left2D, int top2D, Direction direction)
  {
    Cursor2D = new Cursor2D { Left = left2D, Top = top2D };
    _cursor1D = top2D * Width + left2D;
    _prev = Current;
    _direction = direction;
  }
  public readonly bool HasNext() => _direction switch
  {
    Direction.Forward => _cursor1D + 1 < Content.Length,
    Direction.Backward => _cursor1D - 1 >= 0,
    _ => throw new NotImplementedException()
  };
  public bool Move(out CharKind charKind)
  {
    switch (_direction)
    {
      case Direction.Forward:
        {
          var hasNext = _cursor1D + 1 < Content.Length;
          if (hasNext)
          {
            ++_cursor1D;
            ++Cursor2D;
            _prev = Current;
          }
          charKind = Current;
          return hasNext;
        }
      case Direction.Backward:
        {
          var hasNext = _cursor1D - 1 >= 0;
          if (hasNext)
          {
            --_cursor1D;
            --Cursor2D;
            _prev = Current;
          }
          charKind = Current;
          return hasNext;
        }
      default:
        throw new NotImplementedException();
    }
  }
  public bool MoveCheck()
  {
    switch (_direction)
    {
      case Direction.Forward:
        {
          var hasNext = _cursor1D + 1 < Content.Length;
          if (hasNext)
          {
            ++_cursor1D;
            ++Cursor2D;
            var hasNextAndIsSameKind = Current == _prev;
            _prev = Current;
            return hasNextAndIsSameKind;
          }
          return hasNext;
        }
      case Direction.Backward:
        {
          var hasNext = _cursor1D - 1 >= 0;
          if (hasNext)
          {
            --_cursor1D;
            --Cursor2D;
            var hasNextAndIsSameKind = Current == _prev;
            _prev = Current;
            return hasNextAndIsSameKind;
          }
          return hasNext;
        }
      default:
        throw new NotImplementedException();
    }
  }
}