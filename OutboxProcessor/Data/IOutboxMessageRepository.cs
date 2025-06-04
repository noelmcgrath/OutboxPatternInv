using OutboxProcessor.Types;

namespace OutboxProcessor.Data;

public interface IOutboxMessageRepository
{
	Task<List<OutboxMessage>> FetchUnprocessedMessagesAsync(SqlConnection connection, SqlTransaction transaction, int maxCount = 100);
}