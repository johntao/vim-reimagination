internal static class EnumerableHelper
{
  public static bool Any<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate)
  {
    // if (source is null)
    // {
    //   ThrowHelper.ThrowArgumentNullException(ExceptionArgument.source);
    // }
    // if (predicate is null)
    // {
    //   ThrowHelper.ThrowArgumentNullException(ExceptionArgument.predicate);
    // }
    int idx = -1;
    foreach (TSource element in source)
    {
      if (predicate(element, ++idx))
        return true;
    }
    return false;
  }
}