namespace VimReimagination;
using Microsoft.Extensions.Hosting;
using VimReimagination.Service;
internal class EditorHost(
  IReadWrite rw,
  Editor.IRun editor,
  IKeyMap keyMap
) : IHostedService
{
  private readonly IReadWrite _rw = rw;
  private readonly IKeyMap _keymap = keyMap;
  private readonly Editor.IRun _editor = editor;
  public Task StartAsync(CancellationToken cancellationToken)
  {
    Task.Delay(1000, cancellationToken).ContinueWith(q =>
    {
      _keymap.Initialize();
      _editor.Run();
    }, cancellationToken);
    return Task.CompletedTask;
  }
  public Task StopAsync(CancellationToken cancellationToken)
  {
    _rw.Clear();
    return Task.CompletedTask;
  }
}