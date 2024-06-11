namespace VimReimagination.Model;
using Cmd = CmdEnum.All;
internal static class CommandHelper
{
  internal static IEnumerable<string[]> To5ColTable(this IList<CommandInfo> stuffs)
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
}
internal class CommandInfo(Cmd code, string description, char qwertyKey, char dvorakKey, Action run)
{
  public readonly Cmd Code = code;
  public readonly string Description = description;
  public readonly char QwertyKey = qwertyKey;
  public readonly char DvorakKey = dvorakKey;
  public readonly Action Run = run;
  public char YourChoice { get; internal set; } = ' ';
}
