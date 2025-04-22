using System.Collections.Concurrent;

namespace MyConsumer.MessageHandler;

public class Event
{
	private readonly ILogger<Event> c_logger;
	private static readonly ConcurrentDictionary<string, byte> _uniqueContinuumOrderIds = new();


	public Event(
		ILogger<Event> logger)
	{
		this.c_logger = logger;
		
	}


	public async Task<CCS.Messaging.Contract.ConsumerHandlerResult> HandleAsync(
		MyWebApp.ServiceRegistration.OfferCreated @event)
	{
		// Attempt to add the ContinuumOrderIdentifier to the dictionary
		if (_uniqueContinuumOrderIds.TryAdd(@event.ContinuumOrderIdentifier, 0))
		{
			//c_logger.LogInformation($"New unique ContinuumOrderIdentifier added: {@event.ContinuumOrderIdentifier}");
		}
		else
		{
			Console.WriteLine($"Duplicate ContinuumOrderIdentifier detected: {@event.ContinuumOrderIdentifier}");
			// Depending on your requirements, you might choose to return early
			// return CCS.Messaging.Contract.ConsumerHandlerResult.Completed;
		}

		return await this.HandleGenericAsync(@event);
	}

	private async Task<CCS.Messaging.Contract.ConsumerHandlerResult> HandleGenericAsync(
		CCS.Messaging.Contract.Event @event)
	{
		var _sourceEvent = (dynamic)@event;
		var _sourceEventName = _sourceEvent.GetType().FullName;


		try
		{
			await Task.Delay(TimeSpan.FromMicroseconds(1));
			//if (this.DoesTypeHaveIsSyntheticField(@event.GetType()) && _sourceEvent.IsSynthetic) { return Messaging.Contract.ConsumerHandlerResult.Completed; }
			return CCS.Messaging.Contract.ConsumerHandlerResult.Completed;
		}
		catch (Exception exception)
		{
			Console.WriteLine(exception.ToString());
			return CCS.Messaging.Contract.ConsumerHandlerResult.Errored;
		}
	}
}