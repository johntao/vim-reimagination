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
    Console.WriteLine("Map keyboard layout from QWERTY to Dvorak?");
    Console.WriteLine("> NO, thanks");
    Console.WriteLine("  YES, please");
    Console.WriteLine("  NO, I would like to map it myself");
    Console.SetCursorPosition(0, 1);
    ChooseLayoutResult result = ChooseLayoutResult.None;
    while (isChoosing)
    {
      var readkey = Console.ReadKey(true);
      var (_, top) = Console.GetCursorPosition();
      switch (readkey.Key)
      {
        case ConsoleKey.UpArrow:
          if (top <= 1) break;
          Console.Write(' ');
          Console.SetCursorPosition(0, --top);
          Console.Write('>');
          Console.SetCursorPosition(0, top);
          break;
        case ConsoleKey.DownArrow:
          if (top >= 3) break;
          Console.Write(' ');
          Console.SetCursorPosition(0, ++top);
          Console.Write('>');
          Console.SetCursorPosition(0, top);
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
    Console.WriteLine(result);
    Console.CursorVisible = !isChoosing;
    return result;
  }
}