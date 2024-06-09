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
  .AddSingleton<TableRenderer.IPublic, TableRenderer>()
  .AddSingleton<ChoosingKeymapTask.IRun, ChoosingKeymapTask>()
  .AddSingleton<PatternMotion.IGo, PatternMotion>()
  .AddSingleton<BasicMotion.IGo, BasicMotion>()
  .AddSingleton<Command.IGet, Command>()
  .AddSingleton<Editor.IRun, Editor>()
  .AddSingleton<IBuffer, VRBuffer>()
  ;

using IHost host = builder.Build();
await host.RunAsync();