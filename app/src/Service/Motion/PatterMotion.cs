namespace VimReimagination.Service;
using VimReimagination.Model;
using VimReimagination.WordMotion;

internal class PatternMotion(
  IWindow win,
  ICursor cur,
  IBuffer buffer
) : PatternMotion.IGo
{
  #region types and static
  internal interface IGo
  {
    void Row(TextPattern pattern, Direction dir);
    void Repeat(Direction dir);
    void Repeat();
    void RepeatReverse();
  }
  private static readonly SmallWordMotionPattern _smallWord = new();
  private static readonly BigWordMotionPattern _bigWord = new();
  private static TextPattern _prevPattern;
  private static Direction _prevDirection;
  #endregion
  private readonly IWindow _win = win;
  private readonly ICursor _cur = cur;
  private readonly IBuffer _buffer = buffer;
  public void Row(TextPattern textPattern, Direction direction)
  {
    _prevPattern = textPattern;
    _prevDirection = direction;
    var cursor = new Cursor2D(_win, _cur);
    var (newLeft, newTop) = (textPattern, direction) switch
    {
      (TextPattern.SmallWordStart, Direction.RowBackward) => _smallWord.ChargeUntilBlankExclusive(cursor, _buffer, Direction.RowBackward),
      (TextPattern.SmallWordEnd, Direction.RowBackward) => _smallWord.ChargeUntilMatterInclusive(cursor, _buffer, Direction.RowBackward),
      (TextPattern.SmallWordStart, Direction.RowForward) => _smallWord.ChargeUntilMatterInclusive(cursor, _buffer, Direction.RowForward),
      (TextPattern.SmallWordEnd, Direction.RowForward) => _smallWord.ChargeUntilBlankExclusive(cursor, _buffer, Direction.RowForward),
      (TextPattern.BigWordStart, Direction.RowBackward) => _bigWord.ChargeUntilBlankExclusive(cursor, _buffer, Direction.RowBackward),
      (TextPattern.BigWordEnd, Direction.RowBackward) => _bigWord.ChargeUntilMatterInclusive(cursor, _buffer, Direction.RowBackward),
      (TextPattern.BigWordStart, Direction.RowForward) => _bigWord.ChargeUntilMatterInclusive(cursor, _buffer, Direction.RowForward),
      (TextPattern.BigWordEnd, Direction.RowForward) => _bigWord.ChargeUntilBlankExclusive(cursor, _buffer, Direction.RowForward),
      _ => throw new NotImplementedException(),
    };
    _cur.SetCursorPosition(newLeft, newTop);
  }
  public void Repeat(Direction dir)
  {
    Row(_prevPattern, dir);
  }
  public void Repeat()
  {
    Row(_prevPattern, _prevDirection);
  }
  public void RepeatReverse()
  {
    Row(_prevPattern, _prevDirection switch
    {
      Direction.RowBackward => Direction.RowForward,
      Direction.RowForward => Direction.RowBackward,
      _ => throw new NotImplementedException(),
    });
  }
}