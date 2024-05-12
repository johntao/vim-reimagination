using System.Buffers;
namespace VimRenaissance;
/// <summary>
/// this implementation failed to handle secondary word class which is neither space nor small word
/// </summary>
internal class WordMotionV1 : IWordMotion
{
  private static readonly string _smallWord = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_";
  private static readonly SearchValues<char> _searchSmallWord;
  static WordMotionV1()
  {
    _searchSmallWord = SearchValues.Create(_smallWord);
  }
  #region forward
  private static (int, int) FindNegtiveForward(int left2D, int top2D, Buffer1D buffer)
  {
    var anchor1D = top2D * buffer.Width + left2D;
    ReadOnlySpan<char> spanAnchorToEnd = buffer.Content[anchor1D..];
    var foundIndexInSpan1D = spanAnchorToEnd.IndexOfAnyExcept(_searchSmallWord);
    if (foundIndexInSpan1D == -1) return (left2D, top2D);
    var txt = spanAnchorToEnd[..foundIndexInSpan1D].ToString();
    Console.SetCursorPosition(0, Cfg.WinHEI - 1);
    Console.Write(txt.PadRight(Cfg.WinWID, ' '));
    Console.SetCursorPosition(left2D, top2D);
    var newLeft2D = left2D + foundIndexInSpan1D;
    return (newLeft2D, top2D);
  }
  private static (int, int) FindPositiveForward(int left2D, int top2D, Buffer1D buffer)
  {
    var anchor1D = top2D * buffer.Width + left2D;
    ReadOnlySpan<char> spanAnchorToEnd = buffer.Content[anchor1D..];
    var foundIndexInSpan1D = spanAnchorToEnd.IndexOfAny(_searchSmallWord);
    if (foundIndexInSpan1D == -1) return (left2D, top2D);
    var newLeft2D = left2D + foundIndexInSpan1D;
    return (newLeft2D, top2D);
  }
  public (int, int) GetSmallWordEndForward(int left2D, int top2D, Buffer1D buffer)
  {
    var isWordEndOrSpace = IsWordEndOrSpace(left2D, top2D, buffer);
    if (isWordEndOrSpace)
      (left2D, top2D) = FindPositiveForward(left2D + 1, top2D, buffer);
    var (newLeft, newTop) = FindNegtiveForward(left2D, top2D, buffer);
    return (newLeft - 1, newTop);
  }
  public (int, int) GetSmallWordBeginForward(int left2D, int top2D, Buffer1D buffer)
  {
    var isWordEndOrSpace = IsWordEndOrSpace(left2D, top2D, buffer);
    if (!isWordEndOrSpace)
      (left2D, top2D) = FindNegtiveForward(left2D, top2D, buffer);
    var (newLeft, newTop) = FindPositiveForward(left2D + 1, top2D, buffer);
    return (newLeft, newTop);
  }
  private static bool IsWordEndOrSpace(int left2D, int top2D, Buffer1D buffer)
  {
    var anchor1D = top2D * buffer.Width + left2D;
    var span = buffer.Content[anchor1D..(anchor1D + 2)];
    var isWordEnd = span.Length == 1 || (
      _smallWord.Contains(span[0]) &&
      !_smallWord.Contains(span[1])
    );
    if (isWordEnd) return true;
    var isSpace = !_smallWord.Contains(span[0]);
    return isSpace;
  }
  #endregion
  #region backward
  private static (int, int) FindNegativeBackward(int left2D, int top2D, Buffer1D buffer)
  {
    var anchor1D = top2D * buffer.Width + left2D;
    ReadOnlySpan<char> spanBeginToAnchor = buffer.Content[..(anchor1D + 1)];
    var foundIndexInSpan1D = spanBeginToAnchor.LastIndexOfAnyExcept(_searchSmallWord);
    if (foundIndexInSpan1D == -1) return (left2D, top2D);
    var txt = spanBeginToAnchor[(foundIndexInSpan1D + 1)..].ToString();
    Console.SetCursorPosition(0, Cfg.WinHEI - 1);
    Console.Write(txt.PadRight(Cfg.WinWID, ' '));
    Console.SetCursorPosition(left2D, top2D);
    var newLeft2D = left2D - (anchor1D - foundIndexInSpan1D);
    return (newLeft2D, top2D);
  }
  private static (int, int) FindPositiveBackward(int left2D, int top2D, Buffer1D buffer)
  {
    var anchor1D = top2D * buffer.Width + left2D;
    ReadOnlySpan<char> spanBeginToAnchor = buffer.Content[..(anchor1D + 1)];
    var foundIndexInSpan1D = spanBeginToAnchor.LastIndexOfAny(_searchSmallWord);
    if (foundIndexInSpan1D == -1) return (left2D, top2D);
    var newLeft2D = left2D - (anchor1D - foundIndexInSpan1D);
    return (newLeft2D, top2D);
  }
  public (int, int) GetSmallWordEndBackward(int left2D, int top2D, Buffer1D buffer)
  {
    var isWordBeginOrSpace = IsWordBeginOrSpace(left2D, top2D, buffer);
    if (!isWordBeginOrSpace)
      (left2D, top2D) = FindNegativeBackward(left2D, top2D, buffer);
    var (newLeft, newTop) = FindPositiveBackward(left2D - 1, top2D, buffer);
    return (newLeft, newTop);
  }
  public (int, int) GetSmallWordBeginBackward(int left2D, int top2D, Buffer1D buffer)
  {
    var isWordBeginOrSpace = IsWordBeginOrSpace(left2D, top2D, buffer);
    if (isWordBeginOrSpace)
      (left2D, top2D) = FindPositiveBackward(left2D - 1, top2D, buffer);
    var (newLeft, newTop) = FindNegativeBackward(left2D, top2D, buffer);
    return (newLeft + 1, newTop);
  }
  private static bool IsWordBeginOrSpace(int left2D, int top2D, Buffer1D buffer)
  {
    var anchor1D = top2D * buffer.Width + left2D;
    var span = buffer.Content[(anchor1D - 1)..(anchor1D + 1)];
    var isWordBegin = span.Length == 1 || (
      !_smallWord.Contains(span[0]) &&
      _smallWord.Contains(span[1])
    );
    if (isWordBegin) return true;
    var isSpace = !_smallWord.Contains(span[0]);
    return isSpace;
  }
  #endregion
}
