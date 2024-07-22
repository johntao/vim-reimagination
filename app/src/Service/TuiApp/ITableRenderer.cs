namespace VimReimagination.Service;

/*
future enhancement:
- column style
- row style
*/
internal interface ITableRenderer
{
  void Initialize(IEnumerable<string[]> rows, int hOffset = 1);
  void UpdateChoice(string yourChoice);
  int StartLineIdx { get; }
  int EndLineIdx { get; }
}
