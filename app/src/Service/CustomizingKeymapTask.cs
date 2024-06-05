using Cmd = VimReimagination.Model.Commands.All;
namespace VimReimagination.Service;
internal class CustomizingKeymapTask(IReadWrite tr, TableRenderer.IPublic tbl, ICursor cur) : CustomizingKeymapTask.IRun
{
  #region types
  internal interface IRun
  {
    Dictionary<char, Cmd> Run(ChoosingKeymapTask.Result result);
  }
  internal class CommandInfo(Cmd command, string description, char qwertyKey, char dvorakKey)
  {
    public Cmd Command { get; } = command;
    public string Description { get; } = description;
    public char QwertyKey { get; } = qwertyKey;
    public char DvorakKey { get; } = dvorakKey;
    public char YourChoice { get; internal set; } = ' ';
  }
  #endregion
  #region static
  internal static IEnumerable<string[]> To5ColTable(CommandInfo[] stuffs)
  {
    yield return new[] { "NormalCommand", "Description", "Qwerty", "Dvorak", "YourChoice" };
    foreach (var item in stuffs)
    {
      yield return new[]
      {
        item.Command.ToString(),
        item.Description,
        item.QwertyKey.ToString(),
        item.DvorakKey.ToString(),
        item.YourChoice.ToString(),
      };
    }
  }
  private static readonly CommandInfo[] _stuff =
  [
    new(Cmd.Row_Pattern_BigWordStart_Back,    "Move by pattern 'Start of Big Word'  , Back , Row dir", 'q', '"'),
    new(Cmd.Row_Pattern_BigWordEnd_Back,      "Move by pattern 'End of Big Word'    , Back , Row dir", 'w', '<'),
    new(Cmd.Row_Pattern_BigWordStart_Forth,   "Move by pattern 'Start of Big Word'  , Forth, Row dir", 'e', '>'),
    new(Cmd.Row_Pattern_BigWordEnd_Forth,     "Move by pattern 'End of Big Word'    , Forth, Row dir", 'r', 'p'),
    new(Cmd.Row_Pattern_SmallWordStart_Back,  "Move by pattern 'Start of Small Word', Back , Row dir", 'a', 'a'),
    new(Cmd.Row_Pattern_SmallWordEnd_Back,    "Move by pattern 'End of Small Word'  , Back , Row dir", 's', 'o'),
    new(Cmd.Row_Pattern_SmallWordStart_Forth, "Move by pattern 'Start of Small Word', Forth, Row dir", 'd', 'e'),
    new(Cmd.Row_Pattern_SmallWordEnd_Forth,   "Move by pattern 'End of Small Word'  , Forth, Row dir", 'f', 'u'),
    new(Cmd.Row_1unit_Back,  "Move 1 unit, Back , Row dir", 'h', 'd'),
    new(Cmd.Col_1unit_Forth, "Move 1 unit, Forth, Col dir", 'j', 'h'),
    new(Cmd.Col_1unit_Back,  "Move 1 unit, Back , Col dir", 'k', 't'),
    new(Cmd.Row_1unit_Forth, "Move 1 unit, Forth, Row dir", 'l', 'n'),
    new(Cmd.Row_FullScreen_Back_StopOnEdge,  "Move 1 full screen width , Back , Row dir, Stop on edge", 'H', 'D'),
    new(Cmd.Col_FullScreen_Forth_StopOnEdge, "Move 1 full screen height, Forth, Col dir, Stop on edge", 'J', 'H'),
    new(Cmd.Col_FullScreen_Back_StopOnEdge,  "Move 1 full screen height, Back , Col dir, Stop on edge", 'K', 'T'),
    new(Cmd.Row_FullScreen_Forth_StopOnEdge, "Move 1 full screen width , Forth, Row dir, Stop on edge", 'L', 'N'),
    new(Cmd.SmallDelete,     "Delete the character under the cursor", 'x', 'q'),
    new(Cmd.SaveFile,        "Dump current buffer to file", 'z', ';'),
  ];
  private static readonly char[] _dvorakKeys = _stuff.Select(q => q.DvorakKey).ToArray();
  private static readonly char[] _qwertyKeys = _stuff.Select(q => q.QwertyKey).ToArray();
  #endregion
  private readonly IReadWrite _tr = tr;
  private readonly TableRenderer.IPublic _tbl = tbl;
  private readonly ICursor _cur = cur;
  public Dictionary<char, Cmd> Run(ChoosingKeymapTask.Result result)
  {
    var normalCommands = _stuff.Select(q => q.Command).ToArray();
    return result switch
    {
      ChoosingKeymapTask.Result.UseDefaultQwerty => _qwertyKeys.Zip(normalCommands).ToDictionary(),
      ChoosingKeymapTask.Result.MapQwertyToDvorak => _dvorakKeys.Zip(normalCommands).ToDictionary(),
      ChoosingKeymapTask.Result.MapByUser => MapByUser().Zip(normalCommands).ToDictionary(),
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
    _tbl.Initialize(To5ColTable(_stuff));
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
          var hasConflict = _stuff.Any((item, idx) => item.YourChoice == readkey.KeyChar && idx != currentIdx);
          if (hasConflict)
          {
            _tbl.UpdateChoice("!!");
            break;
          }
          var item = _stuff[currentIdx];
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
    return _stuff.Select(q =>
    {
      if (q.YourChoice == ' ')
        q.YourChoice = useQwerty ? q.QwertyKey : q.DvorakKey;
      return q.YourChoice;
    }).ToArray(); ;
  }
}