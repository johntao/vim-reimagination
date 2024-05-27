using Microsoft.Extensions.Hosting;
using VimRenaissance;

internal class EditorHost(IMappingCommands mappingCommands, IChooseLayout chooseLayout) : IHostedService
{
  private readonly IMappingCommands _mappingCommands = mappingCommands;
  private readonly IChooseLayout _chooseLayout = chooseLayout;
  public Task StartAsync(CancellationToken cancellationToken)
  {
    Task.Delay(1000, cancellationToken).ContinueWith((_) =>
    {
      Console.Clear();
      var result = _chooseLayout.Run();
      var layout = _mappingCommands.Run(result);
      new Editor().Run(layout);
    }, cancellationToken);
    return Task.CompletedTask;
  }
  public Task StopAsync(CancellationToken cancellationToken)
  {
    Console.Clear();
    return Task.CompletedTask;
  }
}