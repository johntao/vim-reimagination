namespace VimReimagination;

using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using VimReimagination.Service;

/// <summary>
/// </summary>
internal class EditorHost(
  IReadWrite rw,
  // IKeyMap keyMap,
  ITableRenderer tbl
// IBuffer buffer,
// IKeyMap modalKeyMap,
// StatusBar.IWrite status
) : BackgroundService
{
  #region types and static
  static EditorHost() { }
  #endregion
  private readonly ITableRenderer _tbl = tbl;
  // private readonly IBuffer _buffer = buffer;
  private readonly IReadWrite _rw = rw;
  // private readonly IKeyMap _modalKeyMap = modalKeyMap;
  // private readonly StatusBar.IWrite _status = status;
  protected override Task ExecuteAsync(CancellationToken stoppingToken)
  {
    // _keymap.Initialize();
    // while (!stoppingToken.IsCancellationRequested)
    // {
    //   _buffer.IfWindowResizedThenReloadBuffer();
    //   switch (_modalKeyMap.Modal)
    //   {
    //     case Modal.Normal:
    //       {
    //         var readkey = _rw.ReadKey();
    //         var keychar = readkey.KeyChar;
    //         Dictionary<char, CommandInfo> keymap = _modalKeyMap.Current;
    //         if (keymap.TryGetValue(keychar, out var cmd))
    //         {
    //           cmd.Run();
    //           _status.Write($"{keychar} --> {cmd.Code} ");
    //         }
    //       }
    //       break;
    //     case Modal.Replace:
    //       {
    //         // var readkey = _rw.ReadKey();
    //         // _rw.WriteMove(_buffer.CurrentLine);
    //         // var readkey = _rw.ReadKey();
    //         // var keychar = readkey.KeyChar;
    //         // _buffer.Replace(keychar);
    //         continue;
    //       }
    //     default: throw new InvalidOperationException();
    //   }
    // }
    _rw.Clear();
    _tbl.Initialize(
    [
      ["NormalCommand", "Description", "Qwerty", "Dvorak", "YourChoice"],
      ["a", "b", "c", "d", "e"],
      ["f", "g", "h", "i", "j"],
      ["k", "l", "m", "n", "o"],
      ["p", "q", "r", "s", "t"],
      ["u", "v", "w", "x", "y"],
      ["z", "1", "2", "3", "4"],
      ["5", "6", "7", "8", "9"],
      ["0", "!", "@", "#", "$"],
      ["%", "^", "&", "*", "("],
      ["-", "_", "=", "+", "[["],
      ["{", "}", ";", ":", "'"],
      ["<", ">", ",", ".", "/"],
      ["\\", "|", "`", "~", " "],
    ], 3);
    while (!stoppingToken.IsCancellationRequested)
    {
      Console.ReadLine();
    }
    return Task.CompletedTask;
  }
  public override Task StopAsync(CancellationToken cancellationToken)
  {
    // _rw.Clear();
    return base.StopAsync(cancellationToken);
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