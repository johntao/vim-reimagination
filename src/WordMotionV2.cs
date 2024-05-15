using System.Buffers;

namespace VimRenaissance;
/// <summary>
/// this implementation fully comply with vim word motion
/// </summary>
internal class WordMotionV2 : IWordMotionV2
{
  private static readonly SearchValues<char> _searchPrimary;
  private static readonly SearchValues<char> _searchSecondary;
  private static readonly SearchValues<char> _searchSpace;
  static WordMotionV2()
  {
    const string Primary = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_";
    const string Secondary = ",.:+=-*/\\(){}[]<>!@#$%^&*;\"'`~?";
    const string Space = " \t\n\r\f\v|";
    _searchPrimary = SearchValues.Create(Primary);
    _searchSecondary = SearchValues.Create(Secondary);
    _searchSpace = SearchValues.Create(Space);
  }
  public Cursor2D GetSmallWordEndForward(int left2D, int top2D, Buffer1D buffer)
  {
    buffer.Reset(left2D, top2D, Direction.Forward);
    if (!buffer.HasNext_Move()) return new(left2D, top2D);
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

  public Cursor2D GetSmallWordBeginForward(int left2D, int top2D, Buffer1D buffer)
  {
    buffer.Reset(left2D, top2D, Direction.Forward);
    if (!buffer.HasNext()) return new(left2D, top2D);
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
  public Cursor2D GetSmallWordEndBackward(int left2D, int top2D, Buffer1D buffer)
  {
    buffer.Reset(left2D, top2D, Direction.Backward);
    if (!buffer.HasNext()) return new(left2D, top2D);
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
  public Cursor2D GetSmallWordBeginBackward(int left2D, int top2D, Buffer1D buffer)
  {
    buffer.Reset(left2D, top2D, Direction.Backward);
    if (!buffer.HasNext_Move()) return new(left2D, top2D);
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
}