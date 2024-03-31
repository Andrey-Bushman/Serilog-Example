using Serilog;
using WorkerService1;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSerilog(config =>{
    config.ReadFrom.Configuration(builder.Configuration);
});

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
