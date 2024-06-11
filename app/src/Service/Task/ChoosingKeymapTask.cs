namespace VimReimagination.Service;

/// <summary>
/// enum is something like a indicator or middle product or smnall product
/// we could return the whole product directly if possible, then, there's no need for enum (at least in the interface level)
/// however, if we make it in one blow, we probably disobey the single responsibility principle
/// </summary>
internal class ChoosingKeymapTask(IReadWrite rw, ICursor cur) : ChoosingKeymapTask.IRun
{
  #region types
  internal interface IRun
  {
    Result Run();
  }
  internal enum Result
  {
    None,
    UseDefaultQwerty,
    MapQwertyToDvorak,
    MapByUser,
  }
  #endregion
  private readonly IReadWrite _rw = rw;
  private readonly ICursor _cur = cur;
  public Result Run()
  {
    _rw.Clear();
    var isChoosing = true;
    _cur.CursorVisible = !isChoosing;
    const string Message = """
Choose your keyboard layout:
> QWERTY
  Dvorak
  Map commands by yourself
""";
    _rw.WriteLine(Message);
    _cur.SetCursorPosition(0, 1);
    Result result = Result.None;
    while (isChoosing)
    {
      var readkey = _rw.ReadKey();
      var top = _cur.CursorTop;
      switch (readkey.Key)
      {
        case ConsoleKey.UpArrow:
          if (top <= 1) break;
          _rw.Write(' ');
          --_cur.CursorTop;
          _rw.Write('>');
          break;
        case ConsoleKey.DownArrow:
          if (top >= 3) break;
          _rw.Write(' ');
          ++_cur.CursorTop;
          _rw.Write('>');
          break;
        case ConsoleKey.Enter:
          isChoosing = false;
          result = top switch
          {
            1 => Result.UseDefaultQwerty,
            2 => Result.MapQwertyToDvorak,
            3 => Result.MapByUser,
            _ => Result.None,
          };
          break;
      }
    }
    _cur.CursorVisible = !isChoosing;
    return result;
  }
}