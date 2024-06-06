namespace VimReimagination.Model;
/// <summary>
/// maybe support upward and downward in the future
/// </summary>
internal enum Direction
{
  RowForward,
  RowBackward,
}
internal enum CharKind
{
  None,
  Primary,
  Secondary,
  Space,
}
internal enum TextPattern
{
  None,
  SmallWordStart,
  SmallWordEnd,
  BigWordStart,
  BigWordEnd,
}