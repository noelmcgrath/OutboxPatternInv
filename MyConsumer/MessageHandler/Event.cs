using CCS.Messaging.Client;
using CCS.Messaging.Contract;
using MyConsumer.Data;
using System.Collections.Concurrent;
using System.Text.Json;

namespace MyConsumer.MessageHandler;

public class Event
{
	private readonly ILogger<Event> c_logger;
	private readonly IInboxRepository c_repository;


	public Event(
		ILogger<Event> logger,
		IInboxRepository repository)
	{
		this.c_logger = logger;
		this.c_repository = repository;

	}


	public async Task<CCS.Messaging.Contract.ConsumerHandlerResult> HandleAsync(
		MyConsumer.MessageHandler.Events.OfferCreated @event)
	{
		await this.c_repository.OpenAsync();
		using var transaction = this.c_repository.BeginTransaction();

		try
		{
			var messageId = @event.Id;
			var messageType = @event.GetType().FullName!;
			var messageJson = JsonSerializer.Serialize(@event, new JsonSerializerOptions { IncludeFields = true });
			var consumerId = "my-consumer-id"; // This can be passed in or resolved from DI/context.

			var alreadyProcessed = await this.c_repository.IsProcessedAsync(messageId, transaction);
			if (alreadyProcessed)
				return CCS.Messaging.Contract.ConsumerHandlerResult.Completed;

			await this.c_repository.UpsertMessageAsync(messageId, messageType, messageJson, consumerId, transaction);

			//await handler(@event); // process the message using the passed-in handler

			await this.c_repository.MarkAsProcessedAsync(messageId, transaction);

			transaction.Commit();
			return CCS.Messaging.Contract.ConsumerHandlerResult.Completed;
		}
		catch (Exception ex)
		{
			await this.c_repository.MarkAsFailedAsync(@event.Id, ex.ToString(), transaction);
			transaction.Rollback();
			return CCS.Messaging.Contract.ConsumerHandlerResult.Errored;
		}
	}
}