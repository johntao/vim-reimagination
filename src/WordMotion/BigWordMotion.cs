using VimReimagination.Model;
using System.Buffers;
namespace VimReimagination.WordMotion;
/// <summary>
/// this implementation fully comply with vim word motion
/// </summary>
internal class BigWordMotionPattern : WordMotion
{
  private static readonly SearchValues<char> _searchPrimary;
  static BigWordMotionPattern()
  {
    const string Primary = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_,.:+=-*/\\(){}[]<>!@#$%^&*;\"'`~?";
    _searchPrimary = SearchValues.Create(Primary);
  }
  protected override CharKind GetCharKind(char ch) => ch switch
  {
    var c when _searchPrimary.Contains(c) => CharKind.Primary,
    var c when _searchSpace.Contains(c) => CharKind.Space,
    _ => CharKind.None,
  };
}