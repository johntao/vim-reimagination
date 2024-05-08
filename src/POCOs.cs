using System.Buffers;
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
public enum CharKind
{
  None,
  Primary,
  Secondary,
  Space,
}
public ref struct DummyBuffer
{
  private readonly ReadOnlySpan<char> _primary = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_";
  private readonly ReadOnlySpan<char> _secondary = ",:+=-*/\\(){}[]<>!@#$%^&*;\"'`~|?";
  private readonly ReadOnlySpan<char> _space = " \t\n\r\f\v";
  private readonly SearchValues<char> _searchPrimary, _searchSecondary, _searchSpace;
  private int _anchor1D;
  public DummyBuffer(ReadOnlySpan<char> Content, int Width)
  {
    this.Content = Content;
    this.Width = Width;
    _searchPrimary = SearchValues.Create(_primary);
    _searchSecondary = SearchValues.Create(_secondary);
    _searchSpace = SearchValues.Create(_space);
  }
  // internal readonly BufferIterator Iterator;
  internal readonly ReadOnlySpan<char> Content;
  internal readonly int Width;
  public readonly CharKind Current => Content[_anchor1D] switch
  {
    var c when _searchPrimary.Contains(c) => CharKind.Primary,
    var c when _searchSecondary.Contains(c) => CharKind.Secondary,
    var c when _searchSpace.Contains(c) => CharKind.Space,
    _ => CharKind.None,
  };
  public bool MoveNext() => ++_anchor1D < Content.Length;
  public void Reset(int anchor1D) => _anchor1D = anchor1D;
}
// internal ref struct BufferIterator //(int anchor1D, DummyBuffer buffer) : IEnumerator<CharKind>
// {
//   private readonly ReadOnlySpan<char> _primary = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_";
//   private readonly ReadOnlySpan<char> _secondary = ",:+=-*/\\(){}[]<>!@#$%^&*;\"'`~|?";
//   private readonly ReadOnlySpan<char> _space = " \t\n\r\f\v";
//   private readonly SearchValues<char> _searchPrimary, _searchSecondary, _searchSpace;
//   private int _anchor1D;
//   private DummyBuffer _buffer;
//   public BufferIterator()
//   {
//     _searchPrimary = SearchValues.Create(_primary);
//     _searchSecondary = SearchValues.Create(_secondary);
//     _searchSpace = SearchValues.Create(_space);
//   }
//   public readonly CharKind Current => _buffer.Content[_anchor1D] switch
//   {
//     var c when _searchPrimary.Contains(c) => CharKind.Primary,
//     var c when _searchSecondary.Contains(c) => CharKind.Secondary,
//     var c when _searchSpace.Contains(c) => CharKind.Space,
//     _ => CharKind.None,
//   };
//   public bool MoveNext() => ++_anchor1D < _buffer.Content.Length;
//   public void Reset(int anchor1D, DummyBuffer buffer)
//   {
//     _anchor1D = anchor1D;
//     _buffer = buffer;
//   }
//   // object IEnumerator.Current => throw new NotImplementedException();
//   // public void Dispose()
//   // {
//   //   throw new NotImplementedException();
//   // }
// }