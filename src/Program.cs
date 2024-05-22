Console.Clear();
Console.CancelKeyPress += delegate { Console.Clear(); };

Stuff[] source = MappingCommands._stuff;
var table = new TableHelper(source.To5ColTable());
table.WriteToConsole();

class TableHelper
{
  enum RowStyle
  {
    None,
    Header,
    Separator,
    Normal,
  }
  private int[] _widths = null!;
  private readonly string[][] _rows = null!;
  /*
  future enhancement:
  - column style
  - row style
  */
  public TableHelper(IEnumerable<string[]> rows)
  {
    _rows = rows.Select(row =>
    {
      var cells = row;
      _widths ??= new int[cells.Length];
      return cells.ForEach((q, colIdx) =>
      {
        if (q.Length > _widths[colIdx])
          _widths[colIdx] = q.Length;
      });
    }).ToArray();
  }
  const string Delimiter = " | ";
  internal void WriteToConsole()
  {
    WriteRow(_rows[0], RowStyle.Header);
    WriteRow(_rows[0], RowStyle.Separator);
    _rows.Skip(1).ForEach((cells, _) => WriteRow(cells));
  }
  string PadMiddle(string q, int idx) => q.PadMiddle(_widths[idx]);
  string PadRight(string q, int idx) => q.PadRight(_widths[idx]);
  string Separator(string _, int idx) => new('-', _widths[idx]);
  private void WriteRow(string[] cells, RowStyle style = RowStyle.Normal)
  {
    var formattedCells = style switch
    {
      RowStyle.Header => cells.Select(PadMiddle),
      RowStyle.Separator => cells.Select(Separator),
      RowStyle.Normal => cells.Select(PadRight),
      _ => throw new NotImplementedException(),
    };
    Console.WriteLine(string.Join(Delimiter, formattedCells));
  }
}

// ReadOnlySpan<char> qq = "abcde";
// qq[2..];

// Enumerable.Repeat(false, 5)
// object[,] raw = new object[5, 3]
// {
//   { "a", "b", "c" },
//   { "d", "e", "f" },
//   { "g", "h", "i" },
//   { "j", "k", "l" },
//   { "m", "n", "o" },
// };
// MarkdownTableRenderer.Run3(raw);
// string qq = null;
// Console.WriteLine(qq?.ToString());

// Enumerable.Range(0, 5).ForEach(rowIdx =>
// {
//   var row = new string[3];
//   Enumerable.Range(0, 3).ForEach(colIdx =>
//   {
//     row[colIdx] = raw[rowIdx, colIdx]?.ToString() ?? "";
//     if (row[colIdx].Length > colWidths[colIdx])
//       colWidths[colIdx] = row[colIdx].Length;
//   });
//   rows.Add(row);
// });
// char[] qqq = new char[2];
// System.Console.WriteLine($"'{qqq[1]}'");
// Console.WriteLine($"{Console.WindowWidth},{Console.WindowHeight}");
// MarkdownTableRenderer.Run("README.md");
// var result = ChooseLayout.Run();
// var layout = MappingCommands.Run(result);
// new Editor().Run(layout);

// internal static class EnumerableHelper
// {
//   internal static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
//   {
//     foreach (var item in source)
//       action(item);
//     return source;
//   }
//   internal static void Zip<T,U, O>(this IEnumerable<T> source, IEnumerable<U> partner, Action<T> action)
//   {
//     int i = -1;
//     foreach (var item in source)
//     {
//       ++i;
//       action(item);
//     }
//   }
// }