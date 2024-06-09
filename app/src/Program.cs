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
  .AddSingleton<StatusBarService.IWrite, StatusBarService>()
  .AddSingleton<TableRenderer.IPublic, TableRenderer>()
  .AddSingleton<ChoosingKeymapTask.IRun, ChoosingKeymapTask>()
  .AddSingleton<PatternMotionService.IGo, PatternMotionService>()
  .AddSingleton<BasicMotionService.IGo, BasicMotionService>()
  .AddSingleton<CommandService.IGet, CommandService>()
  .AddSingleton<EditorService.IRun, EditorService>()
  .AddSingleton<IBufferService, BufferService>()
  ;

using IHost host = builder.Build();
await host.RunAsync();