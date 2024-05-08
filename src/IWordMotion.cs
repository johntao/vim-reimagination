namespace VimRenaissance;
public interface IWordMotion
{
  (int newLeft, int newTop) GetSmallWordBeginBackward(int left, int top, DummyBuffer buffer);
  (int newLeft, int newTop) GetSmallWordBeginForward(int left, int top, DummyBuffer buffer);
  (int newLeft, int newTop) GetSmallWordEndBackward(int left, int top, DummyBuffer buffer);
  (int newLeft, int newTop) GetSmallWordEndForward(int left, int top, DummyBuffer buffer);
}
