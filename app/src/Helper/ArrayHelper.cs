namespace VimReimagination.Helper;
internal static class ArrayHelper
{
  internal static void ForEach<T>(this IEnumerable<T> seq, Action<T, int> action)
  {
    int idx = -1;
    foreach (var item in seq)
      action(item, ++idx);
  }
  internal static T[] ForEach<T>(this T[] seq, Action<T, int> action)
  {
    int idx = -1;
    foreach (var item in seq)
      action(item, ++idx);
    return seq;
  }
}