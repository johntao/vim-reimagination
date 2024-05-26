using Ch = VimRenaissance.Helper.ConsoleHelper;
namespace VimRenaissance;

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
internal static class ChooseLayout
{
  internal static ChooseLayoutResult Run()
  {
    var isChoosing = true;
    Console.CursorVisible = !isChoosing;
    Ch.WriteLine("Choose your keyboard layout:");
    Ch.WriteLine("> QWERTY");
    Ch.WriteLine("  Dvorak");
    Ch.WriteLine("  Map commands by user");
    Console.SetCursorPosition(0, 1);
    ChooseLayoutResult result = ChooseLayoutResult.None;
    while (isChoosing)
    {
      var readkey = Console.ReadKey(true);
      var top = Console.CursorTop;
      switch (readkey.Key)
      {
        case ConsoleKey.UpArrow:
          if (top <= 1) break;
          Ch.Write(' ');
          --Console.CursorTop;
          Ch.Write('>');
          break;
        case ConsoleKey.DownArrow:
          if (top >= 3) break;
          Ch.Write(' ');
          ++Console.CursorTop;
          Ch.Write('>');
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
    Console.Clear();
    Ch.WriteLine(result);
    Console.CursorVisible = !isChoosing;
    return result;
  }
}