namespace VimReimagination.Service;
using VimReimagination.Model;
internal class CustomizingKeymapTask(
  IReadWrite rw,
  ITableRenderer tbl,
  ICursor cur,
  Command.IGet cmd
) : CustomizingKeymapTask.IRun
{
  #region types and static
  internal interface IRun
  {
    Dictionary<char, CommandInfo> Run(ChoosingKeymapTask.Result result);
  }
  #endregion
  private readonly IReadWrite _rw = rw;
  private readonly ITableRenderer _tbl = tbl;
  private readonly Command.IGet _cmd = cmd;
  private readonly ICursor _cur = cur;
  public Dictionary<char, CommandInfo> Run(ChoosingKeymapTask.Result result)
  {
    return result switch
    {
      ChoosingKeymapTask.Result.UseDefaultQwerty => _cmd.List.ToDictionary(q => q.QwertyKey),
      ChoosingKeymapTask.Result.MapQwertyToDvorak => _cmd.List.ToDictionary(q => q.DvorakKey),
      ChoosingKeymapTask.Result.MapByUser => MapByUser().Zip(_cmd.List).ToDictionary(),
      _ => throw new InvalidOperationException(),
    };
  }
  private char[] MapByUser()
  {
    _rw.Clear();
    _cur.CursorVisible = false;
    const string Message = """
Press any key to map the following command to that key
Press Enter to cancel mapping and map the rest using QWERTY
Press Backspace to cancel mapping and map the rest using Dvorak
Press arrow keys to navigate
""";
    _rw.WriteLine(Message);
    _tbl.Initialize(_cmd.List.To5ColTable());
    var isLooping = true;
    var useQwerty = false;
    while (isLooping)
    {
      var readkey = _rw.ReadKey();
      var top = _cur.CursorTop;
      switch (readkey)
      {
        case var q when q.Key is ConsoleKey.UpArrow:
          if (top <= _tbl.StartLineIdx) break;
          _rw.Write(' ');
          --_cur.CursorTop;
          _rw.Write('>');
          break;
        case var q when q.Key is ConsoleKey.DownArrow:
          if (top >= _tbl.EndLineIdx) break;
          _rw.Write(' ');
          ++_cur.CursorTop;
          _rw.Write('>');
          break;
        case var q when !char.IsControl(q.KeyChar):
          var currentIdx = top - _tbl.StartLineIdx;
          bool hasConflict = _cmd.List.Any((item, idx) => item.YourChoice == readkey.KeyChar && idx != currentIdx);
          if (hasConflict)
          {
            _tbl.UpdateChoice("!!");
            break;
          }
          var item = _cmd.List[currentIdx];
          item.YourChoice = readkey.KeyChar;
          _tbl.UpdateChoice(item.YourChoice + " ");
          if (top >= _tbl.EndLineIdx) break;
          _rw.Write(' ');
          ++_cur.CursorTop;
          _rw.Write('>');
          break;
        case var q when q.Key is ConsoleKey.Backspace:
          isLooping = false;
          break;
        case var q when q.Key is ConsoleKey.Enter:
          useQwerty = true;
          isLooping = false;
          break;
      }
    }
    _cur.CursorVisible = true;
    return _cmd.List.Select(q =>
    {
      if (q.YourChoice == ' ')
        q.YourChoice = useQwerty ? q.QwertyKey : q.DvorakKey;
      return q.YourChoice;
    }).ToArray(); ;
  }
}