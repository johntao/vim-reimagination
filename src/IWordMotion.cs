namespace VimRenaissance;
internal interface IWordMotion
{
  (int newLeft, int newTop) GetSmallWordBeginBackward(int left, int top, Buffer1D buffer);
  (int newLeft, int newTop) GetSmallWordBeginForward(int left, int top, Buffer1D buffer);
  (int newLeft, int newTop) GetSmallWordEndBackward(int left, int top, Buffer1D buffer);
  (int newLeft, int newTop) GetSmallWordEndForward(int left, int top, Buffer1D buffer);
}
internal interface IWordMotionV2
{
  Cursor2D GetSmallWordBeginBackward(Cursor2D cursor2D, Buffer1D buffer);
  Cursor2D GetSmallWordBeginForward(Cursor2D cursor2D, Buffer1D buffer);
  Cursor2D GetSmallWordEndBackward(Cursor2D cursor2D, Buffer1D buffer);
  Cursor2D GetSmallWordEndForward(Cursor2D cursor2D, Buffer1D buffer);
}
internal interface IWordMotionV3
{
  Cursor2D ChargeUntilSpaceExclusive(Cursor2D cursor2D, Buffer1D buffer, Direction direction);
  Cursor2D ChargeUntilBeingInclusive(Cursor2D cursor2D, Buffer1D buffer, Direction direction);
}