using System.Buffers;
namespace VimRenaissance;

internal ref struct Buffer1D
{
  private readonly SearchValues<char> _searchPrimary;
  private readonly SearchValues<char> _searchSecondary;
  private readonly SearchValues<char> _searchSpace;
  internal Cursor2D Cursor2D { get; private set; }
  private int _cursor1D;
  private Direction _direction;
  private CharKind _prev;
  internal Buffer1D(ReadOnlySpan<char> Content, int Width)
  {
    this.Content = Content;
    this.Width = Width;
    const string Primary = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_";
    const string Secondary = ",.:+=-*/\\(){}[]<>!@#$%^&*;\"'`~?";
    const string Space = " \t\n\r\f\v|";
    _searchPrimary = SearchValues.Create(Primary);
    _searchSecondary = SearchValues.Create(Secondary);
    _searchSpace = SearchValues.Create(Space);
  }
  internal readonly ReadOnlySpan<char> Content;
  internal readonly int Width;
  internal readonly CharKind Current => Content[_cursor1D] switch
  {
    var c when _searchPrimary.Contains(c) => CharKind.Primary,
    var c when _searchSecondary.Contains(c) => CharKind.Secondary,
    var c when _searchSpace.Contains(c) => CharKind.Space,
    _ => CharKind.None,
  };
  internal void Reset(int left2D, int top2D, Direction direction)
  {
    Cursor2D = new Cursor2D { Left = left2D, Top = top2D };
    _cursor1D = top2D * Width + left2D;
    _prev = Current;
    _direction = direction;
  }
  internal readonly bool HasNext() => _direction switch
  {
    Direction.Forward => _cursor1D + 1 < Content.Length,
    Direction.Backward => _cursor1D - 1 >= 0,
    _ => throw new NotImplementedException(),
  };
  internal bool HasNext_Move(out CharKind charKind)
  {
    var hasNext = HasNext();
    if (hasNext)
      MoveCursor();
    _prev = Current;
    charKind = Current;
    return hasNext;
  }
  internal bool HasNext_Move_Check()
  {
    var hasNext = HasNext();
    if (hasNext)
    {
      MoveCursor();
      hasNext = Current == _prev; //hasNextAndIsSameKind
    }
    _prev = Current;
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