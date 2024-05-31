using VimReimagination.Model;
using System.Buffers;
using VimReimagination.Service;

namespace VimReimagination.WordMotion;

internal abstract class WordMotion : IWordMotionV3
{
  #region static
  protected static readonly SearchValues<char> _searchSpace;
  static WordMotion()
  {
    const string Space = " \t\n\r\f\v|";
    _searchSpace = SearchValues.Create(Space);
  }
  private static bool IsSpace(char prev) => _searchSpace.Contains(prev);
  #endregion
  protected abstract CharKind GetCharKind(char ch);
  private bool IsSameKind(IBufferService buffer)
  {
    var prev = GetCharKind(buffer.Previous);
    var current = GetCharKind(buffer.Current);
    return prev == current;
  }
  public Cursor2D ChargeUntilBlankExclusive(Cursor2D cursor, IBufferService buffer, Direction direction)
  {
    buffer.Reset(cursor, direction);
    if (!buffer.HasNext_Move()) return cursor;
    while (buffer.HasNext_Move() && IsSameKind(buffer)) ;
    if (IsSpace(buffer.Previous))
    {
      while (buffer.HasNext_Move() && IsSameKind(buffer)) ;
      return buffer.Cursor2D.Offset(direction);
    }
    else
    {
      return buffer.Cursor2D.Offset(direction);
    }
  }
  public Cursor2D ChargeUntilMatterInclusive(Cursor2D cursor2D, IBufferService buffer, Direction direction)
  {
    buffer.Reset(cursor2D, direction);
    if (!buffer.HasNext()) return cursor2D;
    while (buffer.HasNext_Move() && IsSameKind(buffer)) ;
    if (IsSpace(buffer.Current))
    {
      while (buffer.HasNext_Move() && IsSameKind(buffer)) ;
      return buffer.Cursor2D;
    }
    else
    {
      return buffer.Cursor2D;
    }
  }
}