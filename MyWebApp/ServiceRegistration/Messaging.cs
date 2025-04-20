using CCS.Messaging.Client.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MyWebApp.ServiceRegistration
{
	//public class Messaging
	//{
	//	private  readonly IConfiguration _configuration;

	//	public Messaging(IConfiguration configuration)
	//	{
	//		_configuration = configuration;
	//	}

	//	public IServiceCollection Create(
	//		IServiceCollection services)
	//	{
	//		services.AddSingleton(BuildBusConfiguration);

	//		var x = services.Any(descriptor => descriptor.ServiceType == typeof(CCS.Messaging.Client.Configuration.BusConfigurationBuilder));
	//		services.AddSingleton<CCS.Messaging.Client.Bus>();
	//		services.AddSingleton<CCS.Messaging.Contract.IBus>(services => services.GetRequiredService<CCS.Messaging.Client.Bus>());
	//		services.AddSingleton<CCS.Messaging.Contract.IBusController>(services => services.GetRequiredService<CCS.Messaging.Client.Bus>());

	//		return services;
	//	}

	//	private IBusConfiguration BuildBusConfiguration()
	//	{
	//		//var _busConfigurationBuilder = new CCS.Messaging.Client.Configuration.BusConfigurationBuilder(_configuration.GetValue<string>("messagingBusConnectionStringSettings"));
	//		var _busConfigurationBuilder = new CCS.Messaging.Client.Configuration.BusConfigurationBuilder("hosts=localhost;virtualhost=/;clientprovidedname=pcsprivate_at;username=ccsappuser;ssl=false");
	//		_busConfigurationBuilder.ExchangeDeclare("pcs.events", CCS.Messaging.Client.Configuration.ExchangeType.fanout);
	//		_busConfigurationBuilder.ExchangeDeclare("pcs.events.merchantconfigurationupdated", CCS.Messaging.Client.Configuration.ExchangeType.fanout);
	//		_busConfigurationBuilder.QueueDeclare("citifxs.events", "pcs.events");
	//		_busConfigurationBuilder.QueueDeclare("hsbcfxs.events", "pcs.events");
	//		_busConfigurationBuilder.QueueDeclare("jpmorganfxs.events", "pcs.events");
	//		_busConfigurationBuilder.QueueDeclare("reporting.events", "pcs.events");
	//		_busConfigurationBuilder.QueueDeclare("visafxs.events", "pcs.events");


	//		//_busConfigurationBuilder.RegisterPublication<CCS.PCS.Private.Messages.OfferBilled>(
	//		//	"pcs.events",
	//		//	"pcs.offerbilled.v1");
	//		_busConfigurationBuilder.RegisterPublication<MyWebApp.Messaging.Events.OfferCreated>(
	//			"pcs.events",
	//			"pcs.offercreated.v1");

	//		return _busConfigurationBuilder.Build();
	//	}
	//}



	public static class Messaging
	{
		public static void RegisterMessaging(
			this IServiceCollection collection)
		{
			var _busConfigurationBuilder = new BusConfigurationBuilder("hosts=localhost:5672;virtualhost=/;clientprovidedname=pcsprivate_at;username=ccsappuser;ssl=false");
			_busConfigurationBuilder.ExchangeDeclare("pcs.events", ExchangeType.fanout);
			_busConfigurationBuilder.QueueDeclare("reporting.events", "pcs.events");

			_busConfigurationBuilder.RegisterPublication<MyWebApp.Messaging.Events.OfferCreated>(
				"pcs.events",
				"pcs.offercreated.v1");
			

			var _messagingBusConfiguration = _busConfigurationBuilder.Build();

			collection.AddSingleton(_messagingBusConfiguration);
			collection.AddSingleton<CCS.Messaging.Client.Bus>();
			collection.AddSingleton<CCS.Messaging.Contract.IBus>(services => services.GetRequiredService<CCS.Messaging.Client.Bus>());
			collection.AddSingleton<CCS.Messaging.Contract.IBusController>(services => services.GetRequiredService<CCS.Messaging.Client.Bus>());
		}
	}
}
