internal static class MarkdownTableRenderer
{
  internal static void ForEach<T>(this IEnumerable<T> seq, Action<T> action)
  {
    foreach (var item in seq)
      action(item);
  }
  internal static void ForEach<T>(this IEnumerable<T> seq, Action<T, int> action)
  {
    int idx = -1;
    foreach (var item in seq)
      action(item, ++idx);
  }
  internal static T[] ForEach<T>(this T[] seq, Action<T, int> action)
  {
    int idx = -1;
    foreach (var item in seq)
      action(item, ++idx);
    return seq;
  }

  internal static string JoinAsString(this IEnumerable<string> input, string seperator)
  => string.Join(seperator, input);
  static IEnumerable<T> For<T>(int cnt, Func<int, T> func)
    => Enumerable.Repeat(false, cnt).Select((_, idx) => func(idx));
  internal static void Run3(object[,] raw)
  {
    var colWidths = new int[3];
    var ww =
      For(raw.GetLength(0), rr =>
        For(raw.GetLength(1), cc =>
        {
          var cell = raw[rr, cc];
          var str = cell is null ? "" : (cell as string) ?? cell.ToString()!;
          if (str.Length > colWidths[cc])
            colWidths[cc] = str.Length;
          return str;
        }).ToArray())
      .ToList();
    Console.WriteLine(ww.Select(q => q.JoinAsString(",")).JoinAsString(Environment.NewLine));
  }
  /*
possible input interface:
- object[,] raw
- object[][] raw
- List<object[]> raw
- List<List<object>> raw
- string[,] raw
- string[][] raw
- List<string[]> raw
- List<List<string>> raw
- string[] columns, object[,] raw
- string[] columns, object[][] raw
- string[] columns, List<object[]> raw
- string[] columns, List<List<object>> raw
- string[] columns, string[,] raw
- string[] columns, string[][] raw
- string[] columns, List<string[]> raw
- string[] columns, List<List<string>> raw
  */
  internal static void Run2(string[] columns, object[,] raw)
  {
    const string Delimiter = " | ";
    var rowLength = raw.GetLength(0);
    var colLength = raw.GetLength(1);
    var colWidths = columns.Select(q => q.Length).ToArray();
    var rows = new List<string[]>(rowLength + 1) { columns };
    for (int rowIdx = 0; rowIdx < rowLength; rowIdx++)
    {
      var row = new string[colLength];
      for (int colIdx = 0; colIdx < colLength; colIdx++)
      {
        row[colIdx] = raw[rowIdx, colIdx]?.ToString() ?? "";
        if (row[colIdx].Length > colWidths[colIdx])
          colWidths[colIdx] = row[colIdx].Length;
      }
      rows.Add(row);
    }
    rows.Insert(1, colWidths.Select(width => new string('-', width)).ToArray());
    for (int rowIdx = 0; rowIdx < rows.Count; rowIdx++)
    {
      var cells = rows[rowIdx];
      var rtn = string.Join(Delimiter, cells.Select((q, idx) => q.PadRight(colWidths[idx])));
      Console.WriteLine(rtn);
    }
  }
  internal static void Run(string[] columns, List<string[]> rows)
  {
    const string Delimiter = " | ";
    rows.Insert(0, columns);
    var colWidths = new int[columns.Length];
    for (int i = 0; i < columns.Length; i++)
      colWidths[i] = rows.Max(row => row[i].Length);
    rows.Insert(1, colWidths.Select(width => new string('-', width)).ToArray());
    for (int rowIdx = 0; rowIdx < rows.Count; rowIdx++)
    {
      var cells = rows[rowIdx];
      var rtn = string.Join(Delimiter, cells.Select((q, idx) => q.PadRight(colWidths[idx])));
      Console.WriteLine(rtn);
    }
  }
}


// var str = "a";
// Console.WriteLine($"|{str.PadMiddleV2(4, true)}|");
// Console.WriteLine($"|{str.PadMiddleV2(4)}|");
// str = "ab";
// Console.WriteLine($"|{str.PadMiddleV2(4, true)}|");
// Console.WriteLine($"|{str.PadMiddleV2(4)}|");

internal static class StringExtension
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