using System.Buffers;
namespace VimRenaissance;
/// <summary>
/// this implementation fully comply with vim word motion
/// </summary>
internal class WordMotionV3 : IWordMotionV3
{
  private static readonly SearchValues<char> _searchPrimary;
  private static readonly SearchValues<char> _searchSecondary;
  private static readonly SearchValues<char> _searchSpace;
  static WordMotionV3()
  {
    const string Primary = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_";
    const string Secondary = ",.:+=-*/\\(){}[]<>!@#$%^&*;\"'`~?";
    const string Space = " \t\n\r\f\v|";
    _searchPrimary = SearchValues.Create(Primary);
    _searchSecondary = SearchValues.Create(Secondary);
    _searchSpace = SearchValues.Create(Space);
  }
  private static bool IsSameKind(Buffer1D buffer)
  {
    var prev = GetCharKind(buffer.Previous);
    var current = GetCharKind(buffer.Current);
    return prev == current;
  }
  private static CharKind GetCharKind(char ch) => ch switch
  {
    var c when _searchPrimary.Contains(c) => CharKind.Primary,
    var c when _searchSecondary.Contains(c) => CharKind.Secondary,
    var c when _searchSpace.Contains(c) => CharKind.Space,
    _ => CharKind.None,
  };
  private static bool IsSpace(char prev) => _searchSpace.Contains(prev);
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