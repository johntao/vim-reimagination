using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VimReimagination;
using VimReimagination.Service;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services
  .AddHostedService<EditorHost>()
  .AddSingleton<CustomizingKeymapTask.IRun, CustomizingKeymapTask>()
  .AddSingleton<IReadWrite, ConsoleReadWrite>()
  .AddSingleton<IWindow, ConsoleWindow>()
  .AddSingleton<ICursor, ConsoleCursor>()
  .AddSingleton<StatusBar.IWrite, StatusBar>()
  .AddSingleton<ITableRenderer, TableRendererV2>()
  .AddSingleton<ChoosingKeymapTask.IRun, ChoosingKeymapTask>()
  .AddSingleton<PatternMotion.IGo, PatternMotion>()
  .AddSingleton<BasicMotion.IGo, BasicMotion>()
  .AddSingleton<Command.IGet, Command>()
  .AddSingleton<IBuffer, VRBuffer>()
  ;

using IHost host = builder.Build();
Console.WriteLine("Press any key to start");
Console.ReadKey();
await host.RunAsync();



// IEnumerable<int> qq = Qqq();

// var ww = qq.GetEnumerator();
// Console.WriteLine($"{ww.Current}\t{ww.MoveNext()}");
// Console.WriteLine($"{ww.Current}\t{ww.MoveNext()}");
// Console.WriteLine($"{ww.Current}\t{ww.MoveNext()}");
// Console.WriteLine($"{ww.Current}\t{ww.MoveNext()}");
// Console.WriteLine($"{ww.Current}\t{ww.MoveNext()}");


// // while (ww.MoveNext())
// // {
// //   Console.WriteLine(ww.Current);
// // }
// static IEnumerable<int> Qqq()
// {
//   yield return 1;
//   yield return 3;
//   yield return 7;
// }