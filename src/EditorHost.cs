using Microsoft.Extensions.Hosting;
using VimRenaissance;
using VimRenaissance.Service;

internal class EditorHost(IMappingCommands mappingCommands, IChooseLayout chooseLayout, ITextRenderer tr) : IHostedService
{
  private readonly ITextRenderer _tr = tr;
  private readonly IMappingCommands _mappingCommands = mappingCommands;
  private readonly IChooseLayout _chooseLayout = chooseLayout;
  public Task StartAsync(CancellationToken cancellationToken)
  {
    Task.Delay(1000, cancellationToken).ContinueWith((_) =>
    {
      _tr.Clear();
      var result = _chooseLayout.Run();
      var layout = _mappingCommands.Run(result);
      new Editor().Run(layout);
    }, cancellationToken);
    return Task.CompletedTask;
  }
  public Task StopAsync(CancellationToken cancellationToken)
  {
    _tr.Clear();
    return Task.CompletedTask;
  }
}