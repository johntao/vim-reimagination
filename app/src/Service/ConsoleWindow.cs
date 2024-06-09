namespace VimReimagination.Service;

internal class ConsoleWindow : IWindow
{
  const int _statusBarHeight = 1;
  public (int Width, int Height) Window => (Console.WindowWidth, Console.WindowHeight);
  public int Width => Console.WindowWidth;
  public int Height => Console.WindowHeight - _statusBarHeight;
  public int OuterHeight => Console.WindowHeight;
}
public interface IWindow
{
  (int Width, int Height) Window { get; }
  int Width { get; }
  int Height { get; }
  int OuterHeight { get; }
}