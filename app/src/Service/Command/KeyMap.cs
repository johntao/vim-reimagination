namespace VimReimagination.Service;

using VimReimagination.Model;
using ModalKeyMap = Dictionary<Modal, Dictionary<char, Model.CommandInfo>>;

internal interface IKeyMap
{
  Dictionary<char, CommandInfo> Get { get; }
}

internal class KeyMap : IKeyMap
{
  private static Modal _modal = Modal.Normal;
  private readonly ModalKeyMap _modalKeyMap = null!;
  public Dictionary<char, CommandInfo> Get
  {
    get
    {
      return _modalKeyMap[_modal];
    }
  }
}