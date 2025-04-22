var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Logging.AddConsole();
builder.Services.AddTransient<ILookupRepository, LookupRepository>();
builder.Services.AddTransient<ILookupEventRepository, LookupEventRepository>();
builder.Services.AddTransient<ILookupService, LookupService>();
builder.Services.AddControllers();
RegisterServices(builder.Services);
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var _messageBusController = app.Services.GetRequiredService<CCS.Messaging.Contract.IBusController>();

app.Lifetime.ApplicationStarted.Register(() =>
{
	//app.Logger.LogApplicationStarted();
	//Instrumentation.ApplicationStarted.Add(1);
	_messageBusController.Connect();
});

app.Lifetime.ApplicationStopped.Register(() =>
{
	//app.Logger.LogApplicationStopped();
	//Instrumentation.ApplicationStopped.Add(1);
	_messageBusController.Close();
});

app.Run();


void RegisterServices(
	IServiceCollection services)
{
	services.RegisterMessaging();
}