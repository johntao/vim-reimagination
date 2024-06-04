namespace VimReimagination.Helper;
internal static class BufferHelper
{
  internal static (char[] buffer, List<Range> ranges) Get(int width, int height)
  {
    int size = width * height;
    char[] rtn = new char[size];
    Span<char> buffer = rtn;
    // buffer.Fill(' ');
    Index start = 0;
    List<Range> ranges = [];
    List<int> widths = [width];
    foreach (ReadOnlySpan<char> line in File.ReadLines("./assets/template.txt"))
    {
      int lineLen = ExpandLineLengthToWindowWidth(width, widths, line);
      Range cursor = start..(start.Value + lineLen);
      bool hasHitBoundary = cursor.End.Value >= size;
      if (hasHitBoundary)
      {
        cursor = start..size;
        ranges.Add(cursor);
        int rngEnd = Math.Min(line.Length, size - start.Value);
        line[..rngEnd].CopyTo(buffer[cursor]);
        break;
      }
      ranges.Add(cursor);
      line.CopyTo(buffer[cursor]);
      if (line.Length < lineLen)
        buffer[cursor.End.Value - 1] = '\n';
      start = cursor.End;
    }
    return (rtn, ranges);
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