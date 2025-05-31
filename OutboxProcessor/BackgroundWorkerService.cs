namespace OutboxProcessor;

public class BackgroundWorkerService : BackgroundService
{
	private ILogger<BackgroundWorkerService> _logger;
	private OutboxMessageProcessor _outboxProcessor;

	public BackgroundWorkerService(
		ILogger<BackgroundWorkerService> logger,
		OutboxMessageProcessor outboxProcessor)
	{
		_logger = logger;
		_outboxProcessor = outboxProcessor;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		while (!stoppingToken.IsCancellationRequested) {
			_logger.LogInformation("Worker ExecuteAsync running..... ");
			await _outboxProcessor.ExecuteAsync(stoppingToken);
			await Task.Delay(1000);
		}
	}
}