using Azure.Storage.Blobs;
using FileService.Services;
using FileService.Services.Interfaces;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.Limits.MaxRequestBodySize = null;
});

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = long.MaxValue;
});

builder.Services.AddSingleton<IBlobStorageService, BlobStorageService>();
builder.Services.AddSingleton(_ => new BlobServiceClient(
    builder.Configuration.GetConnectionString("BlobStorage")));

builder.Services.AddSingleton<INotifyService, NotifyService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.MapMethods("/api/heartbeat", [HttpMethod.Get.ToString()],
    () => Results.Ok($"{Assembly.GetExecutingAssembly().GetName().Name} works"));

app.Run();
