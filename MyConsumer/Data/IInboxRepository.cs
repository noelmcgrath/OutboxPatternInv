using Microsoft.Data.SqlClient;

namespace MyConsumer.Data
{
	public interface IInboxRepository
	{
		SqlTransaction BeginTransaction();
		Task<bool> IsProcessedAsync(Guid messageId, SqlTransaction transaction);
		Task MarkAsFailedAsync(Guid messageId, string error, SqlTransaction transaction);
		Task MarkAsProcessedAsync(Guid messageId, SqlTransaction transaction);
		Task OpenAsync();
		Task UpsertMessageAsync(Guid messageId, string type, string json, string consumerId, SqlTransaction transaction);
	}
}