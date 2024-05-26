using Microsoft.Extensions.Hosting;
using VimRenaissance;

internal class EditorHost : IHostedService
{
  public Task StartAsync(CancellationToken cancellationToken)
  {
    Task.Delay(1000, cancellationToken).ContinueWith((_) =>
    {
      Console.Clear();
      var result = ChooseLayout.Run();
      var layout = MappingCommands.Run(result);
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