using MyConsumer;
using MyConsumer.Data;
using static System.Net.Mime.MediaTypeNames;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddTransient<IInboxRepository, InboxRepository>();
//builder.Services.AddHostedService<Worker>();
RegisterServices(builder.Services);



var host = builder.Build();

var _messageBusController = host.Services.GetRequiredService<CCS.Messaging.Contract.IBusController>();
await _messageBusController.ConnectAsync();


host.Run();


void RegisterServices(
	IServiceCollection services)
{
	services.RegisterMessaging();
	services.AddTransient<MyConsumer.MessageHandler.Event>();
}