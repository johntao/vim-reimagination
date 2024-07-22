namespace VimReimagination.Service;

using System.Collections.Generic;
using Spectre.Console;

internal class TableRendererV2(IReadWrite rw, ICursor cur) : ITableRenderer
{
  private readonly IReadWrite _rw = rw;
  private readonly ICursor _cur = cur;
  public int StartLineIdx { get; private set; }
  public int EndLineIdx { get; private set; }
  private int _hOffset;
  public void Initialize(IEnumerable<string[]> rows, int hOffset = 1)
  {
    _hOffset = hOffset;
    StartLineIdx = _cur.CursorTop + 2;

    {
      Table table = new() { Border = TableBorder.Minimal, };
      IEnumerator<string[]> itr = rows.GetEnumerator();
      itr.MoveNext();
      string[] header = itr.Current;
      table.AddColumns(header);
      while (itr.MoveNext())
        table.AddRow(itr.Current);
      var pad = new Padder(table).PadLeft(_hOffset);
      AnsiConsole.Write(pad);
    }

    EndLineIdx = --_cur.CursorTop;
    _cur.SetCursorPosition(0, StartLineIdx);
    _rw.Write('>');
  }

  public void UpdateChoice(string yourChoice)
  {
    throw new NotImplementedException();
  }
}