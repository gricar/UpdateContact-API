using Prometheus;
using UpdateContact.API.Middlewares;
using UpdateContact.Application;
using UpdateContact.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .ConfigureApplicationServices(builder.Configuration)
    .ConfigurePersistenceServices(builder.Configuration);

builder.Services.AddControllers();

var app = builder.Build();

app.UseMiddleware<ErrorHandlerMiddleware>();

app.UseMetricServer();

app.UseHttpMetrics();

app.MapControllers();

app.Run();
