using System.Buffers;

namespace VimRenaissance;
internal interface IWordMotion
{
  (int newLeft, int newTop) GetSmallWordBeginBackward(int left, int top, Buffer1D buffer);
  (int newLeft, int newTop) GetSmallWordBeginForward(int left, int top, Buffer1D buffer);
  (int newLeft, int newTop) GetSmallWordEndBackward(int left, int top, Buffer1D buffer);
  (int newLeft, int newTop) GetSmallWordEndForward(int left, int top, Buffer1D buffer);
}
internal interface IWordMotionV2
{
  Cursor2D GetSmallWordBeginBackward(Cursor2D cursor2D, Buffer1D buffer);
  Cursor2D GetSmallWordBeginForward(Cursor2D cursor2D, Buffer1D buffer);
  Cursor2D GetSmallWordEndBackward(Cursor2D cursor2D, Buffer1D buffer);
  Cursor2D GetSmallWordEndForward(Cursor2D cursor2D, Buffer1D buffer);
}
internal interface IWordMotionV3
{
  Cursor2D ChargeUntilSpaceExclusive(Cursor2D cursor2D, Buffer1D buffer, Direction direction);
  Cursor2D ChargeUntilBeingInclusive(Cursor2D cursor2D, Buffer1D buffer, Direction direction);
}
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
  private bool IsSameKind(Buffer1D buffer)
  {
    var prev = GetCharKind(buffer.Previous);
    var current = GetCharKind(buffer.Current);
    return prev == current;
  }
  public Cursor2D ChargeUntilSpaceExclusive(Cursor2D cursor, Buffer1D buffer, Direction direction)
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
  public Cursor2D ChargeUntilBeingInclusive(Cursor2D cursor2D, Buffer1D buffer, Direction direction)
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