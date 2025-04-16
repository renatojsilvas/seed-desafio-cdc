using Application;
using Application.UseCases;
using DotNetEnv;
using FastEndpoints;
using FastEndpoints.Swagger;
using Infrastructure;
using WebApi;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfiguration();

var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")!;

builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(connectionString);

var app = builder.Build();

app.UseFastEndpoints(c =>
{
    c.Versioning.Prefix = "v";
    c.Versioning.PrependToRoute = true;
});
app.UseSwaggerGen(); 

app.Run();

