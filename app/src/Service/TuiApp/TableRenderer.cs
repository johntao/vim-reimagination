namespace VimReimagination.Service;
using VimReimagination.Helper;
/*
future enhancement:
- column style
- row style
*/
internal class TableRenderer(IReadWrite rw, ICursor cur) : TableRenderer.IPublic
{
  internal interface IPublic
  {
    void Initialize(IEnumerable<string[]> rows, int hOffset = 1);
    void UpdateChoice(string yourChoice);
    int StartLineIdx { get; }
    int EndLineIdx { get; }
  }
  enum RowStyle
  {
    None,
    Header,
    Separator,
    Normal,
  }
  private const string Delimiter = " | ";
  private readonly IReadWrite _rw = rw;
  private readonly ICursor _cur = cur;
  #region state that required initialization
  private int[] _widths = null!;
  private string[][] _rows = null!;
  private int _choiceColIdx;
  public int StartLineIdx { get; private set; }
  public int EndLineIdx { get; private set; }
  private int _hOffset;
  public void Initialize(IEnumerable<string[]> rows, int hOffset = 1)
  {
    _hOffset = hOffset;
    StartLineIdx = _cur.CursorTop + 2;
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
    Render();
  }
  private void Render()
  {
    RenderRow(_rows[0], RowStyle.Header);
    RenderRow(_rows[0], RowStyle.Separator);
    _rows.Skip(1).ForEach((cells, _) => RenderRow(cells));
    EndLineIdx = --_cur.CursorTop;
    _cur.SetCursorPosition(0, StartLineIdx);
    _rw.Write('>');
  }
  #endregion
  string PadMiddle(string q, int idx) => q.PadMiddle(_widths[idx]);
  string Separator(string _, int idx) => new('-', _widths[idx]);
  string PadRight(string q, int idx) => q.PadRight(_widths[idx]);
  private void RenderRow(string[] cells, RowStyle style = RowStyle.Normal)
  {
    var formattedCells = style switch
    {
      RowStyle.Header => cells.Select(PadMiddle),
      RowStyle.Separator => cells.Select(Separator),
      RowStyle.Normal => cells.Select(PadRight),
      _ => throw new NotImplementedException(),
    };
    _rw.WriteLine($"{new string(' ', _hOffset)}{string.Join(Delimiter, formattedCells)}");
  }
  public void UpdateChoice(string yourChoice)
  {
    _cur.CursorLeft = _choiceColIdx;
    _rw.Write(yourChoice);
    _cur.CursorLeft = 0;
  }
}