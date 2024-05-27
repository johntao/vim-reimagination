using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VimRenaissance.Service;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services
  .AddHostedService<EditorHost>()
  .AddSingleton<IMappingCommands, MappingCommands>()
  .AddSingleton<ITextRenderer, ConsoleRenderer>()
  .AddSingleton<ITableRenderer, TableRenderer>()
  .AddSingleton<IChooseLayout, ChooseLayout>()
  ;

using IHost host = builder.Build();
await host.RunAsync();
