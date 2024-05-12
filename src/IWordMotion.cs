namespace VimRenaissance;
public interface IWordMotion
{
  (int newLeft, int newTop) GetSmallWordBeginBackward(int left, int top, Buffer1D buffer);
  (int newLeft, int newTop) GetSmallWordBeginForward(int left, int top, Buffer1D buffer);
  (int newLeft, int newTop) GetSmallWordEndBackward(int left, int top, Buffer1D buffer);
  (int newLeft, int newTop) GetSmallWordEndForward(int left, int top, Buffer1D buffer);
}
public interface IWordMotionV2
{
  Cursor2D GetSmallWordBeginBackward(int left, int top, Buffer1D buffer);
  Cursor2D GetSmallWordBeginForward(int left, int top, Buffer1D buffer);
  Cursor2D GetSmallWordEndBackward(int left, int top, Buffer1D buffer);
  Cursor2D GetSmallWordEndForward(int left, int top, Buffer1D buffer);
}
