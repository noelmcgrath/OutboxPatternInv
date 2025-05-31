using OutboxProcessor.Types;

public interface IOutboxMessageRepository
{
	Task<List<OutboxMessage>> FetchUnprocessedMessagesAsync(int maxCount = 100);
}