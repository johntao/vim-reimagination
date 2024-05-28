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
      new Editor(_tr).Run(layout);
      // Console.WriteLine($"Memory: {GC.GetTotalMemory(true)}");
      // Recursion(0);
    }, cancellationToken);
    return Task.CompletedTask;
  }
  /*
Depth: 13854 | 1024
Depth: 13867 | 2048
524288 is the limit of stackalloc
262144
  */
  // const int SIZE = 262144;
  // private static void Recursion(int depth)
  // {
  //   ReadOnlySpan<int> arr = stackalloc int[SIZE];
  //   Console.WriteLine($"Depth: {depth} | Array.Length {arr.Length} | Memory: {GC.GetTotalMemory(true)}");
  //   // Recursion(depth + 1);
  // }

  public Task StopAsync(CancellationToken cancellationToken)
  {
    _tr.Clear();
    return Task.CompletedTask;
  }
}