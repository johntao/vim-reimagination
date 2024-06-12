namespace VimReimagination;
using Microsoft.Extensions.Hosting;
using VimReimagination.Service;
// using VimReimagination.Model;
// using ModalKeyMap = Dictionary<Service.Modal, Dictionary<char, Model.CommandInfo>>;
internal class EditorHost(
  // CustomizingKeymapTask.IRun mappingCommands,
  // ChoosingKeymapTask.IRun chooseLayout,
  IReadWrite rw,
  Editor.IRun editor
  ) : IHostedService
{
  private readonly IReadWrite _rw = rw;
  // private readonly CustomizingKeymapTask.IRun _mappingCommands = mappingCommands;
  // private readonly ChoosingKeymapTask.IRun _chooseLayout = chooseLayout;
  private readonly Editor.IRun _editor = editor;
  public Task StartAsync(CancellationToken cancellationToken)
  {
    Task.Delay(1000, cancellationToken).ContinueWith((_) =>
    {
      // ChoosingKeymapTask.Result result = _chooseLayout.Run();
      // Dictionary<char, CommandInfo> layout = _mappingCommands.Run(result);
      // ModalKeyMap keymap = new()
      // {
      //   [Modal.Normal] = layout,
      //   [Modal.Replace] = [],
      // };
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