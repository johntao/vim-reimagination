namespace VimRenaissance;
internal class WordMotionV2 : IWordMotion
{
  public (int, int) GetSmallWordEndForward(int left2D, int top2D, Buffer1D buffer)
  {
    buffer.Reset(left2D, top2D);
    var prev = buffer.Current;
    var hasNext = buffer.MoveNext();
    var current = hasNext ? buffer.Current : CharKind.None;
    switch ((prev, current))
    {
      case (CharKind.Primary, CharKind.Primary):
      case (CharKind.Secondary, CharKind.Secondary):
      case (CharKind.Space, CharKind.Primary):
      case (CharKind.Space, CharKind.Secondary):
      case (CharKind.Primary, CharKind.Secondary):
      case (CharKind.Secondary, CharKind.Primary):
        {
          while (buffer.FetchNext()) ;
          return (buffer.Left2D - 1, buffer.Top2D);
        }
      case (_, CharKind.Space):
        {
          while (buffer.FetchNext()) ;
          while (buffer.FetchNext()) ;
          return (buffer.Left2D - 1, buffer.Top2D);
        }
      default:
        return (left2D, top2D);
    }
  }
  public (int, int) GetSmallWordBeginForward(int left2D, int top2D, Buffer1D buffer)
  {
    buffer.Reset(left2D, top2D);
    var prev = buffer.Current;
    var hasNext = buffer.MoveNext();
    var current = hasNext ? buffer.Current : CharKind.None;
    switch ((prev, current))
    {
      case (_, CharKind.Space):
        while (buffer.FetchNext()) ;
        return (buffer.Left2D, buffer.Top2D);
      case (CharKind.Primary, CharKind.Secondary):
      case (CharKind.Secondary, CharKind.Primary):
      case (CharKind.Space, CharKind.Primary):
      case (CharKind.Space, CharKind.Secondary):
        return (buffer.Left2D, buffer.Top2D);
      case (CharKind.Primary, CharKind.Primary):
      case (CharKind.Secondary, CharKind.Secondary):
        while (buffer.FetchNext()) ;
        var newKind = buffer.Current;
        if (newKind != CharKind.Space) // this is the case of alternative kind
          return (buffer.Left2D, buffer.Top2D);
        while (buffer.FetchNext()) ;
        return (buffer.Left2D, buffer.Top2D);
      default:
        return (left2D, top2D);
    }
  }
  public (int, int) GetSmallWordEndBackward(int left2D, int top2D, Buffer1D buffer)
  {
    buffer.Reset(left2D, top2D);
    var prev = buffer.Current;
    var hasNext = buffer.MovePrev();
    var current = hasNext ? buffer.Current : CharKind.None;
    switch ((prev, current))
    {
      case (_, CharKind.Space):
        while (buffer.FetchPrev()) ;
        return (buffer.Left2D, buffer.Top2D);
      case (CharKind.Primary, CharKind.Secondary):
      case (CharKind.Secondary, CharKind.Primary):
      case (CharKind.Space, CharKind.Primary):
      case (CharKind.Space, CharKind.Secondary):
        return (buffer.Left2D, buffer.Top2D);
      case (CharKind.Primary, CharKind.Primary):
      case (CharKind.Secondary, CharKind.Secondary):
        while (buffer.FetchPrev()) ;
        var newKind = buffer.Current;
        if (newKind != CharKind.Space) // this is the case of alternative kind
          return (buffer.Left2D, buffer.Top2D);
        while (buffer.FetchPrev()) ;
        return (buffer.Left2D, buffer.Top2D);
      default:
        return (left2D, top2D);
    }
  }
  public (int, int) GetSmallWordBeginBackward(int left2D, int top2D, Buffer1D buffer)
  {
    buffer.Reset(left2D, top2D);
    var prev = buffer.Current;
    var hasNext = buffer.MovePrev();
    var current = hasNext ? buffer.Current : CharKind.None;
    switch ((prev, current))
    {
      case (CharKind.Primary, CharKind.Primary):
      case (CharKind.Secondary, CharKind.Secondary):
      case (CharKind.Space, CharKind.Primary):
      case (CharKind.Space, CharKind.Secondary):
      case (CharKind.Primary, CharKind.Secondary):
      case (CharKind.Secondary, CharKind.Primary):
        {
          while (buffer.FetchPrev()) ;
          return (buffer.Left2D + 1, buffer.Top2D);
        }
      case (_, CharKind.Space):
        {
          while (buffer.FetchPrev()) ;
          while (buffer.FetchPrev()) ;
          return (buffer.Left2D + 1, buffer.Top2D);
        }
      default:
        return (left2D, top2D);
    }
  }
}