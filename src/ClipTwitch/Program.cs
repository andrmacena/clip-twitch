using ClipTwitch.Service;
using ClipTwitch.Service.ExternalService;
using ClipTwitch.Service.Interfaces;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.Get<AppSettings>();

builder.Services.AddSingleton<AppSettings>();
//builder.Services.Configure<AppSettings>(builder.Configuration);
builder.Services.AddScoped<IOAuthService, TwitchService>();
builder.Services.AddScoped<ITwitchFunctions, TwitchService>();
builder.Configuration.AddEnvironmentVariables();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
