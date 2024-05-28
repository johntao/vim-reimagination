using Microsoft.Extensions.Hosting;
using VimReimagination;
using VimReimagination.Service;

internal class EditorHost(
  CustomizingKeymapTask.IRun mappingCommands,
  ChoosingKeymapTask.IRun chooseLayout,
  ITextRenderer tr
  ) : IHostedService
{
  private readonly ITextRenderer _tr = tr;
  private readonly CustomizingKeymapTask.IRun _mappingCommands = mappingCommands;
  private readonly ChoosingKeymapTask.IRun _chooseLayout = chooseLayout;
  public Task StartAsync(CancellationToken cancellationToken)
  {
    Task.Delay(1000, cancellationToken).ContinueWith((_) =>
    {
      _tr.Clear();
      var result = _chooseLayout.Run();
      var layout = _mappingCommands.Run(result);
      new EditorService(_tr).Run(layout);
    }, cancellationToken);
    return Task.CompletedTask;
  }
  public Task StopAsync(CancellationToken cancellationToken)
  {
    _tr.Clear();
    return Task.CompletedTask;
  }
}