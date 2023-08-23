// See https://aka.ms/new-console-template for more information


using DingtalkStreamDemo;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDingtalkStream(options =>
{
    options.ClientId = "dingXXXXXXXXXXXX";
    options.ClientSecret = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx";
});


builder.Services.AddHostedService<Worker>();

var host = builder.Build();


await host.RunAsync();
