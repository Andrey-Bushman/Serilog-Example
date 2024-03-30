using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseSerilog((hostingContext, loggerConfiguration) => {
    loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
});

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
