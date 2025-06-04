using CCS.Messaging.Client.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OutboxProcessor.ServiceRegistration
{
	public static class Messaging
	{
		public static void RegisterMessaging(
			this IServiceCollection collection,
			IConfiguration configuration) // Added IConfiguration as a parameter
		{
			var _appSettings = configuration.Get<OutboxProcessor.AppSettings>(options => options.BindNonPublicProperties = true);
			var _busConfigurationBuilder = new BusConfigurationBuilder(_appSettings?.MessagingBusConnectionStringSettings);
			_busConfigurationBuilder.ExchangeDeclare("pcs.events", ExchangeType.fanout);
			_busConfigurationBuilder.QueueDeclare("reporting.events", "pcs.events");
			_busConfigurationBuilder.QueueDeclare("hsbcfxs.events", "pcs.events");
			_busConfigurationBuilder.QueueDeclare("jpmorganfxs.events", "pcs.events");

			_busConfigurationBuilder.RegisterPublication<OutboxProcessor.Types.OfferCreated>(
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
