using CCS.Messaging.Client.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static System.Net.Mime.MediaTypeNames;
using System;

namespace MyWebApp.ServiceRegistration
{
	public static class Messaging
	{
		public static void RegisterMessaging(
		this IServiceCollection services)
		{
			services
				.AddSingleton(BootstrapBusConfiguration)
				.AddSingleton<CCS.Messaging.Client.Bus>()
				.AddSingleton<CCS.Messaging.Contract.IBus>(services => services.GetRequiredService<CCS.Messaging.Client.Bus>())
				.AddSingleton<CCS.Messaging.Contract.IBusController>(services => services.GetRequiredService<CCS.Messaging.Client.Bus>());
		}

		private static CCS.Messaging.Client.Configuration.IBusConfiguration BootstrapBusConfiguration(
		IServiceProvider serviceProvider)
		{
			var _busConfigurationBuilder = new CCS.Messaging.Client.Configuration.BusConfigurationBuilder("hosts=myrabbit:5672;virtualhost=/;clientprovidedname=myconsumer;username=ccsappuser;ssl=false")
			{
				//ConsumerReplicas = _appSettings.MessagingBusConsumerReplicas,
				ConsumerMessagePrefetchCount = 10
			};

			_busConfigurationBuilder.ExchangeDeclare("pcs.events", CCS.Messaging.Client.Configuration.ExchangeType.fanout);
			_busConfigurationBuilder.QueueDeclare("reporting.events", "pcs.events");
			_busConfigurationBuilder
				.RegisterConsumer<MyConsumer.MessageHandler.Events.OfferCreated>("reporting.events", "pcs.offercreated.v1",
					message =>
					{
						var _handler = serviceProvider.GetRequiredService<MyConsumer.MessageHandler.Event>();
						return _handler.HandleAsync(message).Result;
					});
			return _busConfigurationBuilder.Build();
		}
	}

}
