namespace VimReimagination;
using Microsoft.Extensions.Hosting;
using VimReimagination.Model;
using VimReimagination.Service;
internal class EditorHost(
  CustomizingKeymapTask.IRun mappingCommands,
  ChoosingKeymapTask.IRun chooseLayout,
  IReadWrite tr,
  EditorService.IRun editor
  ) : IHostedService
{
  private readonly IReadWrite _tr = tr;
  private readonly CustomizingKeymapTask.IRun _mappingCommands = mappingCommands;
  private readonly ChoosingKeymapTask.IRun _chooseLayout = chooseLayout;
  private readonly EditorService.IRun _editor = editor;
  public Task StartAsync(CancellationToken cancellationToken)
  {
    Task.Delay(1000, cancellationToken).ContinueWith((_) =>
    {
      ChoosingKeymapTask.Result result = _chooseLayout.Run();
      Dictionary<char, CommandInfo> layout = _mappingCommands.Run(result);
      _editor.Run(layout);
    }, cancellationToken);
    return Task.CompletedTask;
  }
  public Task StopAsync(CancellationToken cancellationToken)
  {
    _tr.Clear();
    return Task.CompletedTask;
  }
}