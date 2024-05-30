using VimReimagination.Helper;

namespace VimReimagination.Service;
/// <summary>
/// Can't tell the advantage of using ref struct, but it's required to use ref struct for `ReadOnlySpan<char> Text`
/// </summary>
internal class BufferService(ITextRenderer tr) : IBufferService
{
  private readonly ITextRenderer _tr = tr;
  private int _cursor1D;
  private Direction _direction;
  private char[] _buffer = null!;
  private int _winWidth;
  public Cursor2D Cursor2D { get; private set; }
  public char Previous { get; private set; }
  public char Current => _buffer[_cursor1D];
  public void Reset(Cursor2D cursor2D, Direction direction)
  {
    Cursor2D = cursor2D;
    _cursor1D = cursor2D.Top * _winWidth + cursor2D.Left;
    Previous = Current;
    _direction = direction;
  }
  public bool HasNext() => _direction switch
  {
    Direction.Forward => _cursor1D + 1 < _buffer.Length,
    Direction.Backward => _cursor1D - 1 >= 0,
    _ => throw new NotImplementedException(),
  };
  public bool HasNext_Move()
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
  public void IfWindowResizedThenReloadBuffer()
  {
    if (_winWidth == _tr.WindowWidth) return;
    _winWidth = _tr.WindowWidth;
    _buffer = BufferHelper.Get(_winWidth, _tr.WindowHeight);
    _tr.Clear();
    _tr.Write(_buffer);
    _tr.SetCursorPosition(0, 0);
  }
}
internal interface IBufferService
{
  char Previous { get; }
  char Current { get; }
  Cursor2D Cursor2D { get; }
  void Reset(Cursor2D cursor2D, Direction direction);
  bool HasNext();
  bool HasNext_Move();
  void IfWindowResizedThenReloadBuffer();
}