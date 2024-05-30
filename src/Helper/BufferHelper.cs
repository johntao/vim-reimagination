namespace VimReimagination.Helper;
internal static class BufferHelper
{
  internal static char[] Get(int width, int height)
  {
    int size = width * height;
    char[] rtn = new char[size];
    Span<char> buffer = rtn;
    buffer.Fill(' ');
    Index start = 0;
    List<int> widths = [width];
    foreach (ReadOnlySpan<char> line in File.ReadLines("./assets/template.txt"))
    {
      int lineLen = ExpandLineLengthToWindowWidth(width, widths, line);
      Range cursor = start..(start.Value + lineLen);
      bool hasHitBoundary = cursor.End.Value > size;
      if (hasHitBoundary)
      {
        cursor = start..size;
        line[..(size - start.Value)].CopyTo(buffer[cursor]);
        break;
      }
      line.CopyTo(buffer[cursor]);
      start = cursor.End;
    }
    return rtn;
  }
  private static int ExpandLineLengthToWindowWidth(int winWidth, List<int> widths, ReadOnlySpan<char> line)
  {
    int idx = 0;
    while (line.Length > widths[idx++])
    {
      if (idx < widths.Count) continue;
      int newWidth = winWidth * (widths.Count + 1);
      widths.Add(newWidth);
    }
    return widths[idx - 1];
  }
}