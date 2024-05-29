namespace VimReimagination.WordMotion;
internal interface IWordMotionV3
{
  Cursor2D ChargeUntilBlankExclusive(Cursor2D cursor2D, BufferService buffer, Direction direction);
  Cursor2D ChargeUntilMatterInclusive(Cursor2D cursor2D, BufferService buffer, Direction direction);
}
