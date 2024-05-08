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
        {
          var newKind = buffer.Current;
          while (buffer.MoveNext() && buffer.Current == newKind)
            ++left2D;
          return (left2D, top2D);
        }
      case (CharKind.Primary, CharKind.Secondary):
      case (CharKind.Secondary, CharKind.Primary):
        return (left2D + 1, top2D);
      case (_, CharKind.Space):
        {
          while (buffer.MoveNext() && buffer.Current == CharKind.Space)
            ++left2D;
          var newKind = buffer.Current;
          if (newKind == CharKind.Space)
            return (left2D, top2D);
          while (buffer.MoveNext() && buffer.Current == newKind)
            ++left2D;
          return (left2D, top2D);
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
        while (buffer.MoveNext() && buffer.Current == CharKind.Space)
          ++left2D;
        return (left2D + 1, top2D);
      case (CharKind.Primary, CharKind.Secondary):
      case (CharKind.Secondary, CharKind.Primary):
      case (CharKind.Space, CharKind.Primary):
      case (CharKind.Space, CharKind.Secondary):
        return (left2D + 1, top2D);
      case (CharKind.Primary, CharKind.Primary):
      case (CharKind.Secondary, CharKind.Secondary):
        var sameKind = buffer.Current;
        while (buffer.MoveNext() && buffer.Current == sameKind)
          ++left2D;
        var newKind = buffer.Current;
        if (newKind != CharKind.Space)
          return (left2D + 1, top2D);
        while (buffer.MoveNext() && buffer.Current == newKind)
          ++left2D;
        return (left2D + 1, top2D);
      default:
        return (left2D, top2D);
    }
  }
  public (int, int) GetSmallWordEndBackward(int left2D, int top2D, DummyBuffer buffer)
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
        {
          var newKind = buffer.Current;
          while (buffer.MoveNext() && buffer.Current == newKind)
            ++left2D;
          return (left2D, top2D);
        }
      case (CharKind.Primary, CharKind.Secondary):
      case (CharKind.Secondary, CharKind.Primary):
        return (left2D + 1, top2D);
      case (_, CharKind.Space):
        {
          while (buffer.MoveNext() && buffer.Current == CharKind.Space)
            ++left2D;
          var newKind = buffer.Current;
          if (newKind == CharKind.Space)
            return (left2D, top2D);
          while (buffer.MoveNext() && buffer.Current == newKind)
            ++left2D;
          return (left2D, top2D);
        }
      default:
        return (left2D, top2D);
    }
  }
  public (int, int) GetSmallWordBeginBackward(int left2D, int top2D, DummyBuffer buffer)
  {
    var anchor1D = top2D * buffer.Width + left2D;
    buffer.Reset(anchor1D);
    var current = buffer.Current;
    var hasNext = buffer.MoveNext();
    var next = hasNext ? buffer.Current : CharKind.None;
    switch ((current, next))
    {
      case (_, CharKind.Space):
        while (buffer.MoveNext() && buffer.Current == CharKind.Space)
          ++left2D;
        return (left2D + 1, top2D);
      case (CharKind.Primary, CharKind.Secondary):
      case (CharKind.Secondary, CharKind.Primary):
      case (CharKind.Space, CharKind.Primary):
      case (CharKind.Space, CharKind.Secondary):
        return (left2D + 1, top2D);
      case (CharKind.Primary, CharKind.Primary):
      case (CharKind.Secondary, CharKind.Secondary):
        var sameKind = buffer.Current;
        while (buffer.MoveNext() && buffer.Current == sameKind)
          ++left2D;
        var newKind = buffer.Current;
        if (newKind != CharKind.Space)
          return (left2D + 1, top2D);
        while (buffer.MoveNext() && buffer.Current == newKind)
          ++left2D;
        return (left2D + 1, top2D);
      default:
        return (left2D, top2D);
    }
  }
}