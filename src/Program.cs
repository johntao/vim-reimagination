using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VimReimagination.Service;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services
  .AddHostedService<EditorHost>()
  .AddSingleton<CustomizingKeymapTask.IRun, CustomizingKeymapTask>()
  .AddSingleton<IReadWrite, ConsoleReadWrite>()
  .AddSingleton<IWindow, ConsoleWindow>()
  .AddSingleton<ICursor, ConsoleCursor>()
  .AddSingleton<TableRenderer.IPublic, TableRenderer>()
  .AddSingleton<ChoosingKeymapTask.IRun, ChoosingKeymapTask>()
  .AddSingleton<EditorService.IRun, EditorService>()
  .AddSingleton<IBufferService, BufferService>()
  ;

using IHost host = builder.Build();
await host.RunAsync();


/*
string, newline by WriteLine
string, newline by rawString
char[], \0
char[], '.'
char[], \0 and '.'
char[], \0 and \r\n suffix
char[], '.' and \r\n suffix
char[], \0 and \r\n prefix
char[], '.' and \r\n prefix
*/
// using System.Drawing;
// using System.Text;

// while (true)
// {
//   var q = Console.ReadKey();
//   switch (q)
//   {
//     case var w when w.KeyChar is 'q':
//       Console.SetCursorPosition(0, 0);
//       Console.Clear();
//       Console.WriteLine($"{Console.WindowWidth},{Console.WindowHeight}");
//       Console.SetCursorPosition(0, 0);
//       break;
//     case var w when w.KeyChar is 'w': //string, newline by WriteLine
//       Console.SetCursorPosition(0, 0);
//       Console.Clear();
//       for (int i = 0; i < 4; i++)
//         Console.WriteLine($"|{new string('.', 18)}|");
//       Console.Write($"|{new string('.', 18)}|");
//       Console.SetCursorPosition(0, 0);
//       break;
//     case var w when w.KeyChar is 'e': //string, newline by rawString
//       {
//         Console.SetCursorPosition(0, 0);
//         Console.Clear();
//         string haha = """
// |..................|
// |..................|
// |..................|
// |..................|
// |..................|
// """;
//         Console.Write(haha);
//         Console.SetCursorPosition(0, 0);
//         break;
//       }
//     case var w when w.KeyChar is 'r': //char[], zero newline
//       {
//         Console.SetCursorPosition(0, 0);
//         Console.Clear();
//         const string row = "|..................|";
//         ReadOnlySpan<char> rowSpan = row;
//         char[] buffer = new char[100];
//         Span<char> bufferSpan = buffer;
//         for (int i = 0; i < buffer.Length; i += 20)
//           rowSpan.CopyTo(bufferSpan[i..]);
//         Console.Write(buffer);
//         Console.SetCursorPosition(0, 0);
//         break;
//       }
//     case var w when w.KeyChar is 't': //char[], \r\n 4x
//       {
//         Console.SetCursorPosition(0, 0);
//         Console.Clear();
//         const string row = "|..................|";
//         char[] buffer = new char[108];
//         var row5x = Enumerable.Repeat(row, 5);
//         string.Join("\r\n", row5x).AsSpan().CopyTo(buffer);
//         Console.Write(buffer);
//         Console.SetCursorPosition(0, 0);
//         break;
//       }
//     case var w when w.KeyChar is 'a':
//       {
//         Console.SetCursorPosition(0, 0);
//         Console.Clear();
//         const string row = "|.......\0\0\0\0\0\0\0\0\0\0\r\n";
//         ReadOnlySpan<char> rowSpan = row;
//         char[] buffer = new char[100];
//         Span<char> bufferSpan = buffer;
//         int i;
//         for (i = 0; i < buffer.Length - 20; i += 20)
//           rowSpan.CopyTo(bufferSpan[i..]);
//         "|.......".AsSpan().CopyTo(bufferSpan[i..]);
//         Console.Write(buffer);
//         using StreamWriter xx = File.CreateText("./assets/output2.txt");
//         xx.Write(buffer.Where(static q => q != '\0').ToArray());
//         Console.SetCursorPosition(0, 0);
//         break;
//       }
//     case var w when w.KeyChar is 's':
//       {
//         Console.SetCursorPosition(0, 0);
//         Console.Clear();
//         const string row = "|.......\r\n";
//         ReadOnlySpan<char> rowSpan = row;
//         char[] buffer = new char[100];
//         Span<char> bufferSpan = buffer;
//         int i;
//         for (i = 0; i < buffer.Length - 20; i += 20)
//           rowSpan.CopyTo(bufferSpan[i..]);
//         "|.......".AsSpan().CopyTo(bufferSpan[i..]);
//         Console.Write(buffer);
//         File.WriteAllText("./assets/output3.txt", new string(buffer));
//         Console.SetCursorPosition(0, 0);
//         break;
//       }
//     case var w when w.KeyChar is 'd':
//       {
//         Console.SetCursorPosition(0, 0);
//         Console.Clear();
//         const string row = "|......";
//         ReadOnlySpan<char> rowSpan = row;
//         char[] buffer = new char[100];
//         Span<char> bufferSpan = buffer;
//         int i;
//         for (i = 0; i < buffer.Length - 20; i += 20)
//           rowSpan.CopyTo(bufferSpan[i..]);
//         Console.Write(buffer);
//         Console.SetCursorPosition(0, 0);
//         break;
//       }
//     case var w when w.KeyChar is 'f':
//       {
//         Console.SetCursorPosition(0, 0);
//         Console.Clear();
//         const string row = "|.......\0\0\0\0\0\0\0\0\0\0\r\n";
//         Console.Write("DONE");
//         File.WriteAllText("./assets/output4.txt", row.TrimEnd('\r', '\n', '\0', ' '));
//         Console.SetCursorPosition(0, 0);
//         break;
//       }
//     case var w when w.KeyChar is 'z':
//       {
//         Console.SetCursorPosition(0, 0);
//         Console.Clear();
//         Console.WriteLine($"Top,Left: {Console.WindowLeft},{Console.WindowTop}");
//         Console.WriteLine($"Width,Height: {Console.WindowWidth},{Console.WindowHeight}");
//         Console.WriteLine($"Width,Height: {Console.LargestWindowWidth},{Console.LargestWindowHeight}");
//         Console.SetCursorPosition(0, 0);
//         break;
//       }
//     case var w when w.KeyChar is 'x':
//       {
//         Console.SetCursorPosition(0, 0);
//         Console.Clear();
//         // Bitmap memoryImage = new Bitmap(1000, 900);
//         // Size s = new Size(memoryImage.Width, memoryImage.Height);
//         // Graphics memoryGraphics = Graphics.FromImage(memoryImage);
//         // memoryGraphics.CopyFromScreen(0, 0, 0, 0, s);
//         Console.SetWindowSize(60, 20);
//         Console.SetCursorPosition(0, 0);
//         break;
//       }
//     case var w when w.Key is ConsoleKey.LeftArrow:
//       if (Console.CursorLeft > 0) --Console.CursorLeft;
//       break;
//     case var w when w.Key is ConsoleKey.RightArrow:
//       if (Console.CursorLeft < Console.WindowWidth - 1) ++Console.CursorLeft;
//       break;
//     case var w when w.Key is ConsoleKey.UpArrow:
//       if (Console.CursorTop > 0) --Console.CursorTop;
//       break;
//     case var w when w.Key is ConsoleKey.DownArrow:
//       if (Console.CursorTop < Console.WindowHeight - 1) ++Console.CursorTop;
//       break;
//     case var w when w.KeyChar is 'b':
//       Console.Clear();
//       Console.WriteLine("bye");
//       return;
//   }
// }