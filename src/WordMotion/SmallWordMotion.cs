using System.Buffers;
namespace VimReimagination.WordMotion;
/// <summary>
/// this implementation fully comply with vim word motion
/// </summary>
internal class SmallWordMotionPattern : WordMotion
{
  private static readonly SearchValues<char> _searchPrimary;
  private static readonly SearchValues<char> _searchSecondary;
  static SmallWordMotionPattern()
  {
    const string Primary = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_";
    const string Secondary = ",.:+=-*/\\(){}[]<>!@#$%^&*;\"'`~?";
    _searchPrimary = SearchValues.Create(Primary);
    _searchSecondary = SearchValues.Create(Secondary);
  }
  protected override CharKind GetCharKind(char ch) => ch switch
  {
    var c when _searchPrimary.Contains(c) => CharKind.Primary,
    var c when _searchSecondary.Contains(c) => CharKind.Secondary,
    var c when _searchSpace.Contains(c) => CharKind.Space,
    _ => CharKind.None,
  };
}