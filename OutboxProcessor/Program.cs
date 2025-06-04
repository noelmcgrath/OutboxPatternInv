using OutboxProcessor.Data;

var builder = WebApplication.CreateBuilder(args);

var _appSettings = builder.Configuration.Get<OutboxProcessor.AppSettings>(options => options.BindNonPublicProperties = true);
builder.Services.AddSingleton(CCS.Common.DAL.Factory.GetGatewayForSqlClientConnectionString(_appSettings!.DatabaseConnectionString!));
builder.Services.AddTransient<IOutboxMessageRepository, OutboxMessageRepository>();
builder.Services.AddSingleton<OutboxProcessor.OutboxMessageProcessor>();
RegisterServices(builder.Services);
builder.Services.AddHostedService<OutboxProcessor.BackgroundWorkerService>();

// Add services to the container.

var app = builder.Build();

var _messageBusController = app.Services.GetRequiredService<CCS.Messaging.Contract.IBusController>();
await _messageBusController.ConnectAsync();

//app.Lifetime.ApplicationStarted.Register(() =>
//{
//	//app.Logger.LogApplicationStarted();
//	//Instrumentation.ApplicationStarted.Add(1);
//	_messageBusController.Connect();
//});

//app.Lifetime.ApplicationStopped.Register(() =>
//{
//	//app.Logger.LogApplicationStopped();
//	//Instrumentation.ApplicationStopped.Add(1);
//	_messageBusController.Close();
//});

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.Run();

void RegisterServices(
	IServiceCollection services)
{
	services.RegisterMessaging();
}