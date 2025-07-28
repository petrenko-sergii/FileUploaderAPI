using NotificationService;
using NotificationService.Config;
using NotificationService.Services;
using NotificationService.Services.Interfaces;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOptions<EmailServiceOptions>()
    .BindConfiguration("AzureEmailService");

builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapHub<NotificationHub>("/notificationHub");

app.MapMethods("/api/heartbeat", [HttpMethod.Get.ToString()],
    () => Results.Ok($"{Assembly.GetExecutingAssembly().GetName().Name} works"));

app.Run();
