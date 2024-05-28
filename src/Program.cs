using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VimReimagination.Service;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services
  .AddHostedService<EditorHost>()
  .AddSingleton<CustomizingKeymapTask.IRun, CustomizingKeymapTask>()
  .AddSingleton<ITextRenderer, ConsoleRenderer>()
  .AddSingleton<TableRenderer.IPublic, TableRenderer>()
  .AddSingleton<ChoosingKeymapTask.IRun, ChoosingKeymapTask>()
  ;

using IHost host = builder.Build();
await host.RunAsync();

// Console.ReadKey();
// var lines = DarkMagic(File.ReadLines("./assets/template.txt"));
// // File.WriteAllLines("./out/test01.txt", lines);
// Console.WriteLine("Done!");
// static IEnumerable<string> DarkMagic(IEnumerable<string> lines)
// {
//   var width = Console.WindowWidth;
//   List<int> widths = [width];
//   foreach (var line in lines)
//   {
//     int idx = 0;
//     while (line.Length > widths[idx++])
//     {
//       if (idx < widths.Count) continue;
//       var newWidth = width * (widths.Count + 1);
//       widths.Add(newWidth);
//     }
//     yield return string.Create(widths[idx - 1], line, (span, state) =>
//     {
//       span.Fill(' ');
//       state.AsSpan().CopyTo(span);
//     });
//   }
// }