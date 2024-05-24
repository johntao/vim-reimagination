using VimRenaissance;

Console.Clear();
Console.CancelKeyPress += delegate { Console.Clear(); };

MappingCommands.MapByUser();

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