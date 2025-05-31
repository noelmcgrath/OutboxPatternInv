using CCS.Messaging.Contract;
using OutboxProcessor.Types;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text.Json;

namespace OutboxProcessor;

public class OutboxMessageProcessor
{
	private ILogger<OutboxMessageProcessor> _logger;
	private IOutboxMessageRepository _outboxMessageRepository;
	private readonly CCS.Messaging.Contract.IBus _messagingBus;

	public OutboxMessageProcessor(
		ILogger<OutboxMessageProcessor> logger,
		IOutboxMessageRepository outboxMessageRepository,
		CCS.Messaging.Contract.IBus messagingBus)
	{
		_logger = logger;
		_outboxMessageRepository = outboxMessageRepository;
		_messagingBus = messagingBus;

	}
	public async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		_logger.LogInformation("OutboxProcessor ExecuteAsync running..... ");
		var _messages = await _outboxMessageRepository.FetchUnprocessedMessagesAsync();

		//await Task.Delay(100);
		var publishTasks = new List<Task>();
		foreach (var _message in _messages) {

			var messageType = typeof(OutboxMessageProcessor).Assembly.GetTypes().First(t => t.Name == _message.MessageType);
			var deserializedMessage = (OutboxProcessor.Types.OfferCreated) JsonSerializer.Deserialize(_message.MessageJSON, messageType, new JsonSerializerOptions { IncludeFields = true })!;

			try
			{
				var _task = await _messagingBus.PublishAsync(deserializedMessage).WaitAsync(TimeSpan.FromSeconds(2));
				//if (_task.Status == PublicationResultStatus.Published)
				//{
				//	publishTasks.Add(Task.F);
				//}
			}
			catch (Exception)
			{
				//publishTasks.Add(new PublicationResult(PublicationResultStatus.NotPublished, new Event(_message.Id, "")));
				_logger.LogInformation("EXCEPTION..... ");
			}

		}
	}
}
