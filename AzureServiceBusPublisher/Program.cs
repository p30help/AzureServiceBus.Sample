using AzureServiceBus.Contracts;
using AzureServiceBusPublisher;
using AzureServiceBusPublisher.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

/// for more information please check these urls:
/// https://www.nuget.org/packages/Azure.Messaging.ServiceBus/
/// https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-dotnet-get-started-with-queues
/// https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-messaging-overview

builder.Services.AddSingleton<IMessagePublisher, MessagePublisher>();

builder.Services.Configure<AppConfig>(builder.Configuration);

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
