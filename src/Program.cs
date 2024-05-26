using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

var services = builder.Services;
services.AddHostedService<EditorHost>();

using IHost host = builder.Build();
await host.RunAsync();
