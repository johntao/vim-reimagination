namespace VimReimagination.Service;
using VimReimagination.Model;
using ModalKeyMap = Dictionary<Modal, Dictionary<char, Model.CommandInfo>>;
internal interface IKeyMap
{
  Dictionary<char, CommandInfo> Current { get; }
  void Initialize();
}
internal class KeyMap(
  ChoosingKeymapTask.IRun chooseLayout,
  CustomizingKeymapTask.IRun mappingCommands
) : IKeyMap
{
  private static Modal _modal = Modal.Normal;
  private static ModalKeyMap _modalKeyMap = null!;
  private readonly ChoosingKeymapTask.IRun _chooseLayout = chooseLayout;
  private readonly CustomizingKeymapTask.IRun _mappingCommands = mappingCommands;
  public void Initialize()
  {
    if (_modalKeyMap is not null) return;
    ChoosingKeymapTask.Result result = _chooseLayout.Run();
    Dictionary<char, CommandInfo> layout = _mappingCommands.Run(result);
    _modalKeyMap = new()
    {
      [Modal.Normal] = layout,
      [Modal.Replace] = [],
    };
  }
  public Dictionary<char, CommandInfo> Current => _modalKeyMap[_modal];
}