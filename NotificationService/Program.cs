using NotificationService;
using NotificationService.Config;
using NotificationService.Services;
using NotificationService.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
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

app.MapControllers();
app.MapHub<NotificationHub>("/notificationHub");

app.Run();
