using OutboxProcessor.Data;
using System.Text.Json;

namespace OutboxProcessor;

public class OutboxMessageProcessor
{
	private ILogger<OutboxMessageProcessor> _logger;
	private readonly CCS.Common.DAL.IGateway _DALGateway;
	private IOutboxMessageRepository _outboxMessageRepository;
	private readonly CCS.Messaging.Contract.IBus _messagingBus;

	public OutboxMessageProcessor(
		ILogger<OutboxMessageProcessor> logger,
		CCS.Common.DAL.IGateway DALGateway,
		IOutboxMessageRepository outboxMessageRepository,
		CCS.Messaging.Contract.IBus messagingBus)
	{
		_logger = logger;
		_DALGateway = DALGateway;
		_outboxMessageRepository = outboxMessageRepository;
		_messagingBus = messagingBus;

	}
	public async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		_logger.LogInformation("OutboxProcessor ExecuteAsync running..... ");

		await using var connection = await _DALGateway.CreateConnectionAsync();
		using var transaction = connection.BeginTransaction();

		var _messages = await _outboxMessageRepository.FetchUnprocessedMessagesAsync(connection, transaction);

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
