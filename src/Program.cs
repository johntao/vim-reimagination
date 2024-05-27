using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VimRenaissance.Service;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services
  .AddHostedService<EditorHost>()
  .AddSingleton<ICustomizingKeymapTask, CustomizingKeymapTask>()
  .AddSingleton<ITextRenderer, ConsoleRenderer>()
  .AddSingleton<ITableRenderer, TableRenderer>()
  .AddSingleton<IChoosingKeymapTask, ChoosingKeymapTask>()
  ;

using IHost host = builder.Build();
await host.RunAsync();
