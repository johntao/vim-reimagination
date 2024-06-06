using VimReimagination.Model;
using VimReimagination.Helper;
using System.Text;

namespace VimReimagination.Service;
/// <summary>
/// Can't tell the advantage of using ref struct, but it's required to use ref struct for `ReadOnlySpan<char> Text`
/// </summary>
internal class BufferService(IReadWrite tr, IWindow win, ICursor cur) : IBufferService
{
  private readonly IReadWrite _tr = tr;
  private readonly IWindow _win = win;
  private readonly ICursor _cur = cur;
  private int _cursor1D;
  private Direction _direction;
  private char[] _buffer1d = null!;
  private List<Range> _ranges = null!;
  private int _winWidth;
  public Cursor2D Cursor2D { get; private set; }
  public char Previous { get; private set; }
  public char Current => _buffer1d[_cursor1D];
  public void Reset(Cursor2D cursor2D, Direction direction)
  {
    Cursor2D = cursor2D;
    _cursor1D = cursor2D.Top * _winWidth + cursor2D.Left;
    Previous = Current;
    _direction = direction;
  }
  public bool HasNext() => _direction switch
  {
    Direction.RowForward => _cursor1D + 1 < _buffer1d.Length,
    Direction.RowBackward => _cursor1D - 1 >= 0,
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
      case Direction.RowForward:
        ++_cursor1D;
        ++Cursor2D;
        break;
      case Direction.RowBackward:
        --_cursor1D;
        --Cursor2D;
        break;
      default:
        throw new NotImplementedException();
    }
  }
  public void IfWindowResizedThenReloadBuffer()
  {
    if (_winWidth == _win.Width) return;
    _winWidth = _win.Width;
    (_buffer1d, _ranges) = BufferHelper.Get(_winWidth, _win.Height);
    _tr.Clear();
    _tr.Write(_buffer1d);
    _cur.SetCursorPosition(0, 0);
  }

  public void SetChar(char v)
  {
    //erroneous code... _cursor1D is not updated. must refetch the cursor
    //it seems that we couple word motion with buffer service. and we thought that the cursor is always at the right position (which is used by word motion)
    //in fact, the cursor is only true when the cursor is moved by the word motion, otherwise, the cursor is not updated.
    var pos = _cur.GetCursorPosition1D(_win);
    _buffer1d[pos] = v;
    _tr.Write(v);
  }

  public void SaveFile()
  {
    using StreamWriter sw = File.CreateText("./assets/output.txt");
    foreach (var rng in _ranges)
    {
      var lastIdx = rng.End.Value - 1;
      while (lastIdx >= rng.Start.Value && (_buffer1d[lastIdx] == '\0' || _buffer1d[lastIdx] == '\n'))
        --lastIdx;
      char[] buffer = _buffer1d[rng.Start..(lastIdx + 1)];
      sw.WriteLine(buffer);
    }
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
  void SetChar(char v);
  void SaveFile();
}