using Microsoft.Extensions.Hosting;
using VimReimagination;
using VimReimagination.Service;

internal class EditorHost(ICustomizingKeymapTask mappingCommands, IChoosingKeymapTask chooseLayout, ITextRenderer tr) : IHostedService
{
  private readonly ITextRenderer _tr = tr;
  private readonly ICustomizingKeymapTask _mappingCommands = mappingCommands;
  private readonly IChoosingKeymapTask _chooseLayout = chooseLayout;
  public Task StartAsync(CancellationToken cancellationToken)
  {
    Task.Delay(1000, cancellationToken).ContinueWith((_) =>
    {
      _tr.Clear();
      var result = _chooseLayout.Run();
      var layout = _mappingCommands.Run(result);
      new Editor(_tr).Run(layout);
    }, cancellationToken);
    return Task.CompletedTask;
  }
  public Task StopAsync(CancellationToken cancellationToken)
  {
    _tr.Clear();
    return Task.CompletedTask;
  }
}