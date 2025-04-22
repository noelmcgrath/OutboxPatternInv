using MyConsumer;
using static System.Net.Mime.MediaTypeNames;

var builder = Host.CreateApplicationBuilder(args);
//builder.Services.AddHostedService<Worker>();
RegisterServices(builder.Services);



var host = builder.Build();

var _messageBusController = host.Services.GetRequiredService<CCS.Messaging.Contract.IBusController>();
_messageBusController.Connect();


host.Run();


void RegisterServices(
	IServiceCollection services)
{
	services.RegisterMessaging();
	services.AddTransient<MyConsumer.MessageHandler.Event>();
}