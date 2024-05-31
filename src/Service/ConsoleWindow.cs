namespace VimReimagination.Service;

internal class ConsoleWindow : IWindow
{
  public (int Width, int Height) Window => (Console.WindowWidth, Console.WindowHeight);
  public int WindowWidth => Console.WindowWidth;
  public int WindowHeight => Console.WindowHeight;
}
internal interface IWindow
{  (int Width, int Height) Window { get; }
  int WindowWidth { get; }
  int WindowHeight { get; }
}