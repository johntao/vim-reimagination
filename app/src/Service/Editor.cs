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
      switch (_modalKeyMap.Modal)
      {
        case Modal.Normal:
          {
            var readkey = _rw.ReadKey();
            var keychar = readkey.KeyChar;
            Dictionary<char, CommandInfo> keymap = _modalKeyMap.Current;
            if (keymap.TryGetValue(keychar, out var cmd))
            {
              cmd.Run();
              _status.Write($"{keychar} --> {cmd.Code} ");
            }
          }
          break;
        case Modal.Replace:
          {
            // var readkey = _rw.ReadKey();
            // _rw.WriteMove(_buffer.CurrentLine);
            // var readkey = _rw.ReadKey();
            // var keychar = readkey.KeyChar;
            // _buffer.Replace(keychar);
            continue;
          }
        default: throw new InvalidOperationException();
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