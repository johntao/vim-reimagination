using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VimRenaissance;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services
  .AddHostedService<EditorHost>()
  .AddSingleton<IMappingCommands, MappingCommands>()
  .AddSingleton<IChooseLayout, ChooseLayout>()
  ;

using IHost host = builder.Build();
await host.RunAsync();
