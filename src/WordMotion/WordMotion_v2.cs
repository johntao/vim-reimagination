using System.Buffers;
namespace VimRenaissance;
/// <summary>
/// this implementation fully comply with vim word motion
/// </summary>
[Obsolete("This implementation is not used anymore")]
internal class WordMotion_v2 : IWordMotionV2
{
  private static readonly SearchValues<char> _searchPrimary;
  private static readonly SearchValues<char> _searchSecondary;
  private static readonly SearchValues<char> _searchSpace;
  static WordMotion_v2()
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
  public Cursor2D GetSmallWordEndForward(Cursor2D cursor, Buffer1D buffer)
  {
    buffer.Reset(cursor, Direction.Forward);
    if (!buffer.HasNext_Move()) return cursor;
    while (buffer.HasNext_Move() && IsSameKind(buffer)) ;
    if (IsSpace(buffer.Previous))
    {
      while (buffer.HasNext_Move() && IsSameKind(buffer)) ;
      return buffer.Cursor2D.Offset(Direction.Forward);
    }
    else
    {
      return buffer.Cursor2D.Offset(Direction.Forward);
    }
  }
  public Cursor2D GetSmallWordBeginForward(Cursor2D cursor2D, Buffer1D buffer)
  {
    buffer.Reset(cursor2D, Direction.Forward);
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
  public Cursor2D GetSmallWordEndBackward(Cursor2D cursor2D, Buffer1D buffer)
  {
    buffer.Reset(cursor2D, Direction.Backward);
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
  public Cursor2D GetSmallWordBeginBackward(Cursor2D cursor2D, Buffer1D buffer)
  {
    buffer.Reset(cursor2D, Direction.Backward);
    if (!buffer.HasNext_Move()) return cursor2D;
    while (buffer.HasNext_Move() && IsSameKind(buffer)) ;
    if (IsSpace(buffer.Previous))
    {
      while (buffer.HasNext_Move() && IsSameKind(buffer)) ;
      return buffer.Cursor2D.Offset(Direction.Backward);
    }
    else
    {
      return buffer.Cursor2D.Offset(Direction.Backward);
    }
  }
}