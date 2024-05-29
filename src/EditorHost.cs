using Microsoft.Extensions.Hosting;
using VimReimagination.Service;

internal class EditorHost(
  CustomizingKeymapTask.IRun mappingCommands,
  ChoosingKeymapTask.IRun chooseLayout,
  ITextRenderer tr,
  EditorService.IRun editor
  ) : IHostedService
{
  private readonly ITextRenderer _tr = tr;
  private readonly CustomizingKeymapTask.IRun _mappingCommands = mappingCommands;
  private readonly ChoosingKeymapTask.IRun _chooseLayout = chooseLayout;
  private readonly EditorService.IRun _editor = editor;
  public Task StartAsync(CancellationToken cancellationToken)
  {
    Task.Delay(1000, cancellationToken).ContinueWith((_) =>
    {
      var result = _chooseLayout.Run();
      var layout = _mappingCommands.Run(result);
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