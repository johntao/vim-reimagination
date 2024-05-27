namespace VimRenaissance.Service;

/// <summary>
/// enum is something like a indicator or middle product or smnall product
/// we could return the whole product directly if possible, then, there's no need for enum (at least in the interface level)
/// however, if we make it in one blow, we probably disobey the single responsibility principle
/// </summary>
enum ChooseLayoutResult
{
  None,
  UseDefaultQwerty,
  MapQwertyToDvorak,
  MapByUser,
}
internal class ChooseLayout(ITextRenderer tr) : IChooseLayout
{
  private readonly ITextRenderer _tr = tr;
  public ChooseLayoutResult Run()
  {
    var isChoosing = true;
    _tr.CursorVisible = !isChoosing;
    _tr.WriteLine("Choose your keyboard layout:");
    _tr.WriteLine("> QWERTY");
    _tr.WriteLine("  Dvorak");
    _tr.WriteLine("  Map commands by user");
    _tr.SetCursorPosition(0, 1);
    ChooseLayoutResult result = ChooseLayoutResult.None;
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
            1 => ChooseLayoutResult.UseDefaultQwerty,
            2 => ChooseLayoutResult.MapQwertyToDvorak,
            3 => ChooseLayoutResult.MapByUser,
            _ => ChooseLayoutResult.None,
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

internal interface IChooseLayout
{
  ChooseLayoutResult Run();
}