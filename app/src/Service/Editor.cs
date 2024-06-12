namespace VimReimagination.Service;

using VimReimagination.Model;
/// <summary>
/// </summary>
internal class Editor(
  IReadWrite rw,
  IBuffer buffer,
  StatusBar.IWrite status,
  IKeyMap modalKeyMap
) : Editor.IRun
{
  #region types and static
  internal interface IRun
  {
    void Run();
  }
  static Editor() { }
  #endregion
  private readonly IBuffer _buffer = buffer;
  private readonly IReadWrite _rw = rw;
  private readonly IKeyMap _modalKeyMap = modalKeyMap;
  private readonly StatusBar.IWrite _status = status;
  public void Run()
  {
    while (true)
    {
      _buffer.IfWindowResizedThenReloadBuffer();
      var readkey = _rw.ReadKey();
      var keychar = readkey.KeyChar;
      Dictionary<char, CommandInfo> keymap = _modalKeyMap.Get;
      if (keymap.TryGetValue(keychar, out var cmd))
      {
        cmd.Run();
        _status.Write($"{keychar} --> {cmd.Code} ");
      }
    }
  }
}

internal enum Modal
{
  None,
  Normal,
  Replace,
  Insert,
  Visual,
  Command,
}