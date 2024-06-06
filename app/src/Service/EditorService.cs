namespace VimReimagination.Service;
/// <summary>
/// Can't tell the advantage of using ref struct, but it's required to use ref struct for `Buffer1D _buffer`
/// I wonder the perf difference between ref struct and static class
/// should benchmark it someday
/// </summary>
internal class EditorService(IReadWrite tr, IBufferService buffer, CommandService.IExe cmd) : EditorService.IRun
{
  #region types and static
  internal interface IRun
  {
    void Run(Dictionary<char, CommandInfo> keymap);
  }
  static EditorService() { }
  #endregion
  private readonly IBufferService _buffer = buffer;
  private readonly IReadWrite _tr = tr;
  private readonly CommandService.IExe _cmd = cmd;
  public void Run(Dictionary<char, CommandInfo> keymap)
  {
    while (true)
    {
      _buffer.IfWindowResizedThenReloadBuffer();
      var readkey = _tr.ReadKey();
      var keychar = readkey.KeyChar;
      if (keymap.TryGetValue(keychar, out var cmd))
        cmd.Run();
    }
  }
}