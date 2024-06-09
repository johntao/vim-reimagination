namespace VimReimagination.Service;
using VimReimagination.Model;
/// <summary>
/// </summary>
internal class EditorService(
  IReadWrite tr,
  IBufferService buffer,
  StatusBarService.IWrite status
) : EditorService.IRun
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
  private readonly StatusBarService.IWrite _status = status;
  public void Run(Dictionary<char, CommandInfo> keymap)
  {
    while (true)
    {
      _buffer.IfWindowResizedThenReloadBuffer();
      var readkey = _tr.ReadKey();
      var keychar = readkey.KeyChar;
      if (keymap.TryGetValue(keychar, out var cmd))
      {
        cmd.Run();
        _status.Write($"{keychar} --> {cmd.Code} ");
      }
    }
  }
}