using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyConsumer.Data
{
	public class InboxRepository : IInboxRepository
	{
		private readonly string _connectionString;
		private readonly SqlConnection _connection;

		public InboxRepository(IConfiguration configuration)
		{
			_connectionString = configuration["databaseConnectionString"]!;
			_connection = new SqlConnection(_connectionString);
		}

		public async Task<bool> IsProcessedAsync(Guid messageId, SqlTransaction transaction)
		{
			var cmd = new SqlCommand("SELECT Status FROM dbo.InboxMessages WHERE Id = @Id", _connection, transaction);
			cmd.Parameters.AddWithValue("@Id", messageId);

			var result = await cmd.ExecuteScalarAsync();
			return result != null && result.ToString() == "Processed";
		}

		public async Task UpsertMessageAsync(Guid messageId, string type, string json, string consumerId, SqlTransaction transaction)
		{
			var cmd = new SqlCommand(@"
            MERGE dbo.InboxMessages AS target
            USING (SELECT @Id AS Id) AS source
            ON (target.Id = source.Id)
            WHEN MATCHED THEN
                UPDATE SET MessageType = @Type, MessageJSON = @Json, ConsumerId = @ConsumerId, Status = 'Pending'
            WHEN NOT MATCHED THEN
                INSERT (Id, MessageType, MessageJSON, ReceivedAt, ConsumerId, Status)
                VALUES (@Id, @Type, @Json, SYSDATETIMEOFFSET(), @ConsumerId, 'Pending');", _connection, transaction);

			cmd.Parameters.AddWithValue("@Id", messageId);
			cmd.Parameters.AddWithValue("@Type", type);
			cmd.Parameters.AddWithValue("@Json", json);
			cmd.Parameters.AddWithValue("@ConsumerId", consumerId ?? (object)DBNull.Value);

			await cmd.ExecuteNonQueryAsync();
		}

		public async Task MarkAsProcessedAsync(Guid messageId, SqlTransaction transaction)
		{
			var cmd = new SqlCommand(@"
            UPDATE dbo.InboxMessages
            SET ProcessedAt = SYSDATETIMEOFFSET(), Status = 'Processed'
            WHERE Id = @Id", _connection, transaction);

			cmd.Parameters.AddWithValue("@Id", messageId);
			await cmd.ExecuteNonQueryAsync();
		}

		public async Task MarkAsFailedAsync(Guid messageId, string error, SqlTransaction transaction)
		{
			var cmd = new SqlCommand(@"
            UPDATE dbo.InboxMessages
            SET Status = 'Failed', Retries = Retries + 1, ErrorDetails = @Error
            WHERE Id = @Id", _connection, transaction);

			cmd.Parameters.AddWithValue("@Id", messageId);
			cmd.Parameters.AddWithValue("@Error", error);
			await cmd.ExecuteNonQueryAsync();
		}

		public async Task OpenAsync() => await _connection.OpenAsync();

		public SqlTransaction BeginTransaction() => _connection.BeginTransaction();
	}
}
