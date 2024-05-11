namespace VimRenaissance;
internal class WordMotionV2 : IWordMotion
{
  public (int, int) GetSmallWordEndForward(int left2D, int top2D, DummyBuffer buffer)
  {
    var anchor1D = top2D * buffer.Width + left2D;
    buffer.Reset(anchor1D);
    var current = buffer.Current;
    var hasNext = buffer.MoveNext();
    var next = hasNext ? buffer.Current : CharKind.None;
    switch ((current, next))
    {
      case (CharKind.Primary, CharKind.Primary):
      case (CharKind.Secondary, CharKind.Secondary):
      case (CharKind.Space, CharKind.Primary):
      case (CharKind.Space, CharKind.Secondary):
      case (CharKind.Primary, CharKind.Secondary):
      case (CharKind.Secondary, CharKind.Primary):
        {
          while (buffer.MoveNext() && buffer.Current == next) ;
          return (buffer.Anchor1D - 1, top2D);
        }
      case (_, CharKind.Space):
        {
          while (buffer.MoveNext() && buffer.Current == next) ;
          var newKind = buffer.Current;
          while (buffer.MoveNext() && buffer.Current == newKind) ;
          return (buffer.Anchor1D - 1, top2D);
        }
      default:
        return (left2D, top2D);
    }
  }
  public (int, int) GetSmallWordBeginForward(int left2D, int top2D, DummyBuffer buffer)
  {
    var anchor1D = top2D * buffer.Width + left2D;
    buffer.Reset(anchor1D);
    var current = buffer.Current;
    var hasNext = buffer.MoveNext();
    var next = hasNext ? buffer.Current : CharKind.None;
    switch ((current, next))
    {
      case (_, CharKind.Space):
        while (buffer.MoveNext() && buffer.Current == next) ;
        return (buffer.Anchor1D, top2D);
      case (CharKind.Primary, CharKind.Secondary):
      case (CharKind.Secondary, CharKind.Primary):
      case (CharKind.Space, CharKind.Primary):
      case (CharKind.Space, CharKind.Secondary):
        return (left2D + 1, top2D);
      case (CharKind.Primary, CharKind.Primary):
      case (CharKind.Secondary, CharKind.Secondary):
        while (buffer.MoveNext() && buffer.Current == next) ;
        var newKind = buffer.Current;
        if (newKind != CharKind.Space) // this is the case of alternative kind
          return (buffer.Anchor1D, top2D);
        while (buffer.MoveNext() && buffer.Current == newKind) ;
        return (buffer.Anchor1D, top2D);
      default:
        return (left2D, top2D);
    }
  }
  public (int, int) GetSmallWordEndBackward(int left2D, int top2D, DummyBuffer buffer)
  {
    var anchor1D = top2D * buffer.Width + left2D;
    buffer.Reset(anchor1D);
    var current = buffer.Current;
    var hasPrev = buffer.MovePrev();
    var prev = hasPrev ? buffer.Current : CharKind.None;
    switch ((current, prev))
    {
      case (_, CharKind.Space):
        while (buffer.MovePrev() && buffer.Current == prev) ;
        return (buffer.Anchor1D, top2D);
      case (CharKind.Primary, CharKind.Secondary):
      case (CharKind.Secondary, CharKind.Primary):
      case (CharKind.Space, CharKind.Primary):
      case (CharKind.Space, CharKind.Secondary):
        return (left2D - 1, top2D);
      case (CharKind.Primary, CharKind.Primary):
      case (CharKind.Secondary, CharKind.Secondary):
        while (buffer.MovePrev() && buffer.Current == prev) ;
        var newKind = buffer.Current;
        if (newKind != CharKind.Space) // this is the case of alternative kind
          return (buffer.Anchor1D, top2D);
        while (buffer.MovePrev() && buffer.Current == newKind) ;
        return (buffer.Anchor1D, top2D);
      default:
        return (left2D, top2D);
    }
  }
  public (int, int) GetSmallWordBeginBackward(int left2D, int top2D, DummyBuffer buffer)
  {
    var anchor1D = top2D * buffer.Width + left2D;
    buffer.Reset(anchor1D);
    var current = buffer.Current;
    var hasPrev = buffer.MovePrev();
    var prev = hasPrev ? buffer.Current : CharKind.None;
    switch ((current, prev))
    {
      case (CharKind.Primary, CharKind.Primary):
      case (CharKind.Secondary, CharKind.Secondary):
      case (CharKind.Space, CharKind.Primary):
      case (CharKind.Space, CharKind.Secondary):
      case (CharKind.Primary, CharKind.Secondary):
      case (CharKind.Secondary, CharKind.Primary):
        {
          while (buffer.MovePrev() && buffer.Current == prev) ;
          return (buffer.Anchor1D + 1, top2D);
        }
      case (_, CharKind.Space):
        {
          while (buffer.MovePrev() && buffer.Current == prev) ;
          var newKind = buffer.Current;
          while (buffer.MovePrev() && buffer.Current == newKind) ;
          return (buffer.Anchor1D + 1, top2D);
        }
      default:
        return (left2D, top2D);
    }
  }
}