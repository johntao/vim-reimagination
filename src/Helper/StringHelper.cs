namespace VimRenaissance.Helper;
internal static class StringHelper
{
  internal static string PadMiddle(this string input, int totalWidth, bool isLeanToRight = false, char paddingChar = ' ')
  {
    if (input.Length >= totalWidth) return input;
    var padding = Math.DivRem(totalWidth - input.Length, 2, out int remainder);
    var offset = isLeanToRight ? padding + remainder : padding;
    return string.Create(totalWidth, (input, offset, paddingChar), (span, state) =>
    {
      var (_input, _offset, _paddingChar) = state;
      span.Fill(_paddingChar);
      _input.AsSpan().CopyTo(span[_offset..]);
    });
  }
}