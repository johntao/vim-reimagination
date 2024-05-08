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
public readonly ref struct DummyBuffer(ReadOnlySpan<char> Content, int Width)
{
  static DummyBuffer()
  {
  }
  internal readonly ReadOnlySpan<char> Content = Content;
  internal readonly int Width = Width;
}