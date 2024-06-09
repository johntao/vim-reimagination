namespace VimReimagination.WordMotion;
using VimReimagination.Model;
using VimReimagination.Service;
internal interface IWordMotion
{
  Cursor2D ChargeUntilBlankExclusive(Cursor2D cursor2D, IBuffer buffer, Direction direction);
  Cursor2D ChargeUntilMatterInclusive(Cursor2D cursor2D, IBuffer buffer, Direction direction);
}
