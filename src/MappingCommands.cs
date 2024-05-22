using Cmd = VimRenaissance.NormalCommand;
internal static class MappingCommands
{
  internal static readonly Stuff[] _stuff =
  [
    new(Cmd.MoveHorizontal45uBackward, "Experimental, move horizontally by 45 units backward", 'n', 'b'),
    new(Cmd.MoveHorizontal45uForward, "Experimental, move horizontally by 45 units forward", '.', 'v'),
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
  ];
  static readonly char[] _dvorakKeys = _stuff.Select(q => q.DvorakKey).ToArray();
  static readonly char[] _qwertyKeys = _stuff.Select(q => q.QwertyKey).ToArray();
  // static char[] _yourChoices;
  internal static Dictionary<char, Cmd> Run(ChooseLayoutResult result)
  {
    var normalCommands = _stuff.Select(q => q.Command).ToArray();
    // var qwertyKeys = _stuff.Select(q => q.QwertyKey);
    // var dvorakKeys = _stuff.Select(q => q.DvorakKey);
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
    for (int i = 0; i < _stuff.Length; i++)
    {
      var item = _stuff[i];
      Console.Clear();
      Console.WriteLine("Press any key to map the following command to the key");
      Console.WriteLine("Press Enter to cancel mapping and use Qwerty");
      Console.WriteLine("Press Backspace to cancel mapping and use Dvorak");
      Console.WriteLine("Press arrow keys to navigate");
      Console.WriteLine($"Cmd: {i}. {item.Command}");
      Console.WriteLine($"Qwerty layout: {item.QwertyKey}");
      Console.WriteLine($"Dvorak layout: {item.DvorakKey}");
      Console.WriteLine($"Your choice: '{item.YourChoice}'");
      var key = Console.ReadKey(true);
      switch (key.Key)
      {
        case ConsoleKey.Enter: return _qwertyKeys;
        case ConsoleKey.Backspace: return _dvorakKeys;
        case ConsoleKey.LeftArrow:
          if (--i == -1 || --i == -1) { }
          break;
        case ConsoleKey.RightArrow:
          // if (++i == _normalCommands.Length - 1) break;
          // result[i] = result[i + 1];
          // result[i + 1] = key.KeyChar;
          break;
        default:
          item.YourChoice = key.KeyChar;
          break;
      }
    }
    Console.CursorVisible = true;
    return _stuff.Select(q => q.YourChoice).ToArray(); ;
  }
  internal static IEnumerable<string[]> To5ColTable(this Stuff[] stuffs)
  {
    yield return new[] { "NormalCommand", "Qwerty", "Dvorak", "YourChoice", "Description" };
    foreach (var item in stuffs)
    {
      yield return new[]
      {
        item.Command.ToString(),
        item.QwertyKey.ToString(),
        item.DvorakKey.ToString(),
        item.YourChoice.ToString(),
        item.Description,
      };
    }
  }
}

internal class Stuff(Cmd command, string description, char qwertyKey, char dvorakKey)
{
  public Cmd Command { get; } = command;
  public string Description { get; } = description;
  public char QwertyKey { get; } = qwertyKey;
  public char DvorakKey { get; } = dvorakKey;
  public char YourChoice { get; internal set; } = ' ';
}
// public override bool Equals(object? obj)
// {
//     return obj is Stuff other &&
//            Command == other.Command &&
//            Description == other.Description &&
//            QwertyKey == other.QwertyKey &&
//            DvorakKey == other.DvorakKey &&
//            YourChoice == other.YourChoice;
// }
// public override int GetHashCode()
// {
//     return HashCode.Combine(Command, Description, QwertyKey, DvorakKey, YourChoice);
// }