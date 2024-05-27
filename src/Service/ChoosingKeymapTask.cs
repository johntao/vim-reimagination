namespace VimRenaissance.Service;

/// <summary>
/// enum is something like a indicator or middle product or smnall product
/// we could return the whole product directly if possible, then, there's no need for enum (at least in the interface level)
/// however, if we make it in one blow, we probably disobey the single responsibility principle
/// </summary>
enum ChoosingKeymapTaskResult
{
  None,
  UseDefaultQwerty,
  MapQwertyToDvorak,
  MapByUser,
}
internal class ChoosingKeymapTask(ITextRenderer tr) : IChoosingKeymapTask
{
  private readonly ITextRenderer _tr = tr;
  public ChoosingKeymapTaskResult Run()
  {
    var isChoosing = true;
    _tr.CursorVisible = !isChoosing;
    const string Message = """
Choose your keyboard layout:
> QWERTY
  Dvorak
  Map commands by yourself
""";
    _tr.WriteLine(Message);
    _tr.SetCursorPosition(0, 1);
    ChoosingKeymapTaskResult result = ChoosingKeymapTaskResult.None;
    while (isChoosing)
    {
      var readkey = _tr.ReadKey();
      var top = _tr.CursorTop;
      switch (readkey.Key)
      {
        case ConsoleKey.UpArrow:
          if (top <= 1) break;
          _tr.Write(' ');
          --_tr.CursorTop;
          _tr.Write('>');
          break;
        case ConsoleKey.DownArrow:
          if (top >= 3) break;
          _tr.Write(' ');
          ++_tr.CursorTop;
          _tr.Write('>');
          break;
        case ConsoleKey.Enter:
          isChoosing = false;
          result = top switch
          {
            1 => ChoosingKeymapTaskResult.UseDefaultQwerty,
            2 => ChoosingKeymapTaskResult.MapQwertyToDvorak,
            3 => ChoosingKeymapTaskResult.MapByUser,
            _ => ChoosingKeymapTaskResult.None,
          };
          break;
      }
    }
    _tr.Clear();
    _tr.WriteLine(result);
    _tr.CursorVisible = !isChoosing;
    return result;
  }
}

internal interface IChoosingKeymapTask
{
  ChoosingKeymapTaskResult Run();
}