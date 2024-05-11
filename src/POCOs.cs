using System.Buffers;
namespace VimRenaissance;
internal static class Cfg
{
  //210,419,628,837
  public const int WinWID = 209; //Console.WindowWidth;
  public const int WinHEI = 51; //Console.WindowHeight;
}
enum TextPattern
{
  WordEndBackward = -2,
  WordBeginBackward,
  None,
  WordBeginFoward,
  WordEndForward,
}
public enum CharKind
{
  None,
  Primary,
  Secondary,
  Space,
}
public ref struct Buffer1D
{
  private readonly ReadOnlySpan<char> _primary = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_";
  private readonly ReadOnlySpan<char> _secondary = ",.:+=-*/\\(){}[]<>!@#$%^&*;\"'`~|?";
  private readonly ReadOnlySpan<char> _space = " \t\n\r\f\v";
  private readonly SearchValues<char> _searchPrimary, _searchSecondary, _searchSpace;
  private int _cursor1D, _left2D, _top2D;
  private CharKind _prev;
  internal readonly int Cursor1D => _cursor1D;
  internal readonly int Left2D => _left2D;
  internal readonly int Top2D => _top2D;
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
  public bool MoveNext()
  {
    var hasNext = _cursor1D + 1 < Content.Length;
    if (hasNext)
    {
      ++_cursor1D;
      ++_left2D;
      _prev = Current;
    }
    return hasNext;
  }

  public bool FetchNext()
  {
    var hasNext = _cursor1D + 1 < Content.Length;
    if (hasNext)
    {
      ++_cursor1D;
      ++_left2D;
      var hasNextAndIsSameKind = Current == _prev;
      _prev = Current;
      return hasNextAndIsSameKind;
    }
    return false;
  }
  public bool FetchPrev()
  {
    var hasNext = _cursor1D - 1 >= 0;
    if (hasNext)
    {
      --_cursor1D;
      --_left2D;
      var hasNextAndIsSameKind = Current == _prev;
      _prev = Current;
      return hasNextAndIsSameKind;
    }
    return hasNext;
  }

  public bool MovePrev()
  {
    var hasNext = _cursor1D - 1 >= 0;
    if (hasNext)
    {
      --_cursor1D;
      --_left2D;
      _prev = Current;
    }
    return hasNext;
  }

  public void Reset(int cursor1D)
  {
    _cursor1D = cursor1D;
  }
  public void Reset(int left2D, int top2D)
  {
    _left2D = left2D;
    _top2D = top2D;
    _cursor1D = top2D * Width + left2D;
    _prev = Current;
  }
}