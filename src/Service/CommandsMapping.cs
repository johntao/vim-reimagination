using Cmd = VimRenaissance.NormalCommand;
using Ch = VimRenaissance.Service.ConsoleRenderer;
using VimRenaissance.Helper;
// using VimRenaissance.Service;
namespace VimRenaissance;
internal static class MappingCommands
{
  internal static readonly CommandInfo[] _stuff =
  [
    new(Cmd.MoveHorizontalByPatternBigWordStartBackward, "Move horizontally by pattern 'Start of Big Word' backward", 'q', '"'),
    new(Cmd.MoveHorizontalByPatternBigWordEndBackward, "Move horizontally by pattern 'End of Big Word' backward", 'w', '<'),
    new(Cmd.MoveHorizontalByPatternBigWordStartForward, "Move horizontally by pattern 'Start of Big Word' forward", 'e', '>'),
    new(Cmd.MoveHorizontalByPatternBigWordEndForward, "Move horizontally by pattern 'End of Big Word' forward", 'r', 'p'),
    new(Cmd.MoveHorizontalByPatternSmallWordStartBackward, "Move horizontally by pattern 'Start of Small Word' backward", 'a', 'a'),
    new(Cmd.MoveHorizontalByPatternSmallWordEndBackward, "Move horizontally by pattern 'End of Small Word' backward", 's', 'o'),
    new(Cmd.MoveHorizontalByPatternSmallWordStartForward, "Move horizontally by pattern 'Start of Small Word' forward", 'd', 'e'),
    new(Cmd.MoveHorizontalByPatternSmallWordEndForward, "Move horizontally by pattern 'End of Small Word' forward", 'f', 'u'),
    new(Cmd.MoveHorizontal1uBackward, "Move horizontally by 1 unit backward", 'h', 'd'),
    new(Cmd.MoveVertical1uForward, "Move vertically by 1 unit forward", 'j', 'h'),
    new(Cmd.MoveVertical1uBackward, "Move vertically by 1 unit backward", 'k', 't'),
    new(Cmd.MoveHorizontal1uForward, "Move horizontally by 1 unit forward", 'l', 'n'),
    new(Cmd.MoveHorizontalFullScreenBackwardStop, "Move horizontally by full screen width backward, stop at the edge", 'H', 'D'),
    new(Cmd.MoveVerticalFullScreenForwardStop, "Move vertically by full screen height forward, stop at the edge", 'J', 'H'),
    new(Cmd.MoveVerticalFullScreenBackwardStop, "Move vertically by full screen height backward, stop at the edge", 'K', 'T'),
    new(Cmd.MoveHorizontalFullScreenForwardStop, "Move horizontally by full screen width forward, stop at the edge", 'L', 'N'),
    new(Cmd.MoveHorizontal45uBackward, "Experimental, move horizontally by 45 units backward", 'n', 'b'),
    new(Cmd.MoveHorizontal45uForward, "Experimental, move horizontally by 45 units forward", '.', 'v'),
  ];
  static readonly char[] _dvorakKeys = _stuff.Select(q => q.DvorakKey).ToArray();
  static readonly char[] _qwertyKeys = _stuff.Select(q => q.QwertyKey).ToArray();
  internal static Dictionary<char, Cmd> Run(ChooseLayoutResult result)
  {
    var normalCommands = _stuff.Select(q => q.Command).ToArray();
    return result switch
    {
      ChooseLayoutResult.UseDefaultQwerty => _qwertyKeys.Zip(normalCommands).ToDictionary(),
      ChooseLayoutResult.MapQwertyToDvorak => _dvorakKeys.Zip(normalCommands).ToDictionary(),
      ChooseLayoutResult.MapByUser => MapByUser().Zip(normalCommands).ToDictionary(),
      _ => throw new InvalidOperationException(),
    };
  }
  private static char[] MapByUser()
  {
    Console.CursorVisible = false;
    Ch.WriteLine("Press any key to map the following command to that key");
    Ch.WriteLine("Press Enter to cancel mapping and map the rest using QWERTY");
    Ch.WriteLine("Press Backspace to cancel mapping and map the rest using Dvorak");
    Ch.WriteLine("Press arrow keys to navigate");
    var table = new TableHelper(_stuff.To5ColTable());
    table.WriteToConsole();
    var isLooping = true;
    var useQwerty = false;
    while (isLooping)
    {
      var readkey = Console.ReadKey(true);
      var top = Console.CursorTop;
      switch (readkey)
      {
        case var q when q.Key is ConsoleKey.UpArrow:
          if (top <= table.StartLineIdx) break;
          Ch.Write(' ');
          --Console.CursorTop;
          Ch.Write('>');
          break;
        case var q when q.Key is ConsoleKey.DownArrow:
          if (top >= table.EndLineIdx) break;
          Ch.Write(' ');
          ++Console.CursorTop;
          Ch.Write('>');
          break;
        case var q when !char.IsControl(q.KeyChar):
          var currentIdx = top - table.StartLineIdx;
          var hasConflict = _stuff.Any((item, idx) => item.YourChoice == readkey.KeyChar && idx != currentIdx);
          if (hasConflict)
          {
            table.UpdateChoice("!!");
            break;
          }
          var item = _stuff[currentIdx];
          item.YourChoice = readkey.KeyChar;
          table.UpdateChoice(item.YourChoice + " ");
          if (top >= table.EndLineIdx) break;
          Ch.Write(' ');
          ++Console.CursorTop;
          Ch.Write('>');
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
    Console.CursorVisible = true;
    return _stuff.Select(q =>
    {
      if (q.YourChoice == ' ')
        q.YourChoice = useQwerty ? q.QwertyKey : q.DvorakKey;
      return q.YourChoice;
    }).ToArray(); ;
  }
  internal static IEnumerable<string[]> To5ColTable(this CommandInfo[] stuffs)
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
  internal class CommandInfo(Cmd command, string description, char qwertyKey, char dvorakKey)
  {
    public Cmd Command { get; } = command;
    public string Description { get; } = description;
    public char QwertyKey { get; } = qwertyKey;
    public char DvorakKey { get; } = dvorakKey;
    public char YourChoice { get; internal set; } = ' ';
  }
}