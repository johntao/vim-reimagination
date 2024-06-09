namespace VimReimagination.Model;
using Cmd = CmdEnum.All;
internal class CommandInfo(Cmd code, string description, char qwertyKey, char dvorakKey, Action run)
{
  public readonly Cmd Code = code;
  public readonly string Description = description;
  public readonly char QwertyKey = qwertyKey;
  public readonly char DvorakKey = dvorakKey;
  public readonly Action Run = run;
  public char YourChoice { get; internal set; } = ' ';
}
