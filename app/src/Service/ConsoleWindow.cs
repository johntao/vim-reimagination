namespace VimReimagination.Service;

internal class ConsoleWindow : IWindow
{
  public (int Width, int Height) Window => (Console.WindowWidth, Console.WindowHeight);
  public int Width => Console.WindowWidth;
  public int Height => Console.WindowHeight;
}
public interface IWindow
{
  (int Width, int Height) Window { get; }
  int Width { get; }
  int Height { get; }
}