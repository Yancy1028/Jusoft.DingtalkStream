// See https://aka.ms/new-console-template for more information


using DingtalkStreamDemo;

using Jusoft.DingtalkStream;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDingtalkStream(options =>
{
    options.ClientId = "dingXXXXXXXXXXXX";
    options.ClientSecret = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
});

builder.Services.AddTransient<DingtalkStreamMessageHandler, DefaultMessageHandler>();


builder.Services.AddHostedService<Worker>();

var host = builder.Build();


await host.RunAsync();
