using VimRenaissance.Service;

namespace VimRenaissance.Helper;
/*
future enhancement:
- column style
- row style
*/
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
  internal readonly int StartLineIdx;
  private readonly int _choiceColIdx;
  internal int EndLineIdx { get; private set; }
  private readonly int _hOffset;
  public TableHelper(IEnumerable<string[]> rows, int hOffset = 1)
  {
    _hOffset = hOffset;
    StartLineIdx = Console.CursorTop + 2;
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
    const int ChoiceDistance = 4;
    _choiceColIdx = hOffset + Delimiter.Length * ChoiceDistance + _widths.Take(ChoiceDistance).Sum();
  }
  const string Delimiter = " | ";
  internal void WriteToConsole()
  {
    WriteRow(_rows[0], RowStyle.Header);
    WriteRow(_rows[0], RowStyle.Separator);
    _rows.Skip(1).ForEach((cells, _) => WriteRow(cells));
    EndLineIdx = --Console.CursorTop;
    Console.SetCursorPosition(0, StartLineIdx);
    ConsoleHelper.Write('>');
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
    ConsoleHelper.WriteLine($"{new string(' ', _hOffset)}{string.Join(Delimiter, formattedCells)}");
  }
  internal void UpdateChoice(string yourChoice)
  {
    Console.CursorLeft = _choiceColIdx;
    Console.Write(yourChoice);
    Console.CursorLeft = 0;
  }
}
