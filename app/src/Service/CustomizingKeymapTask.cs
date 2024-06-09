namespace VimReimagination.Service;
internal class CustomizingKeymapTask(
  IReadWrite tr,
  TableRenderer.IPublic tbl,
  ICursor cur,
  CommandService.IGet cmd
) : CustomizingKeymapTask.IRun
{
  #region types
  internal interface IRun
  {
    Dictionary<char, CommandInfo> Run(ChoosingKeymapTask.Result result);
  }
  #endregion
  #region static
  internal static IEnumerable<string[]> To5ColTable(IList<CommandInfo> stuffs)
  {
    yield return new[] { "NormalCommand", "Description", "Qwerty", "Dvorak", "YourChoice" };
    foreach (var item in stuffs)
    {
      yield return new[]
      {
        item.Code.ToString(),
        item.Description,
        item.QwertyKey.ToString(),
        item.DvorakKey.ToString(),
        item.YourChoice.ToString(),
      };
    }
  }
  #endregion
  private readonly IReadWrite _tr = tr;
  private readonly TableRenderer.IPublic _tbl = tbl;
  private readonly CommandService.IGet _cmd = cmd;
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
    _tr.Clear();
    _cur.CursorVisible = false;
    const string Message = """
Press any key to map the following command to that key
Press Enter to cancel mapping and map the rest using QWERTY
Press Backspace to cancel mapping and map the rest using Dvorak
Press arrow keys to navigate
""";
    _tr.WriteLine(Message);
    _tbl.Initialize(To5ColTable(_cmd.List));
    var isLooping = true;
    var useQwerty = false;
    while (isLooping)
    {
      var readkey = _tr.ReadKey();
      var top = _cur.CursorTop;
      switch (readkey)
      {
        case var q when q.Key is ConsoleKey.UpArrow:
          if (top <= _tbl.StartLineIdx) break;
          _tr.Write(' ');
          --_cur.CursorTop;
          _tr.Write('>');
          break;
        case var q when q.Key is ConsoleKey.DownArrow:
          if (top >= _tbl.EndLineIdx) break;
          _tr.Write(' ');
          ++_cur.CursorTop;
          _tr.Write('>');
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
          _tr.Write(' ');
          ++_cur.CursorTop;
          _tr.Write('>');
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