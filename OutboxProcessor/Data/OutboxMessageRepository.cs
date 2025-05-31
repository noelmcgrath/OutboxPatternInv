using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using OutboxProcessor.Types;

public class OutboxMessageRepository : IOutboxMessageRepository
{
	private readonly string _connectionString;

	public OutboxMessageRepository(IConfiguration configuration)
	{
		_connectionString = configuration.GetConnectionString("DefaultConnection");
	}

	public async Task<List<OutboxMessage>> FetchUnprocessedMessagesAsync(int maxCount = 100)
	{
		var messages = new List<OutboxMessage>();
		var query = @"
            SELECT TOP (@MaxCount) 
                Id,
                MessageType,
                MessageJSON,
                OccurredTimestamp,
                ProcessedTimestamp,
                VersionSequence,
                Error
            FROM dbo.OutboxMessages
            WHERE ProcessedTimestamp IS NULL
            ORDER BY OccurredTimestamp ASC";

		using (var connection = new SqlConnection(_connectionString))
		using (var command = new SqlCommand(query, connection))
		{
			command.Parameters.Add(new SqlParameter("@MaxCount", SqlDbType.Int) { Value = maxCount });

			await connection.OpenAsync();

			using (var reader = await command.ExecuteReaderAsync())
			{
				while (await reader.ReadAsync())
				{
					var message = new OutboxMessage
					{
						Id = reader.GetGuid(reader.GetOrdinal("Id")),
						MessageType = reader.GetString(reader.GetOrdinal("MessageType")),
						MessageJSON = reader.IsDBNull(reader.GetOrdinal("MessageJSON")) ? null : reader.GetString(reader.GetOrdinal("MessageJSON")),
						OccurredTimestamp = reader.GetDateTime(reader.GetOrdinal("OccurredTimestamp")),
						ProcessedTimestamp = reader.IsDBNull(reader.GetOrdinal("ProcessedTimestamp")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ProcessedTimestamp")),
						VersionSequence = reader.GetInt32(reader.GetOrdinal("VersionSequence")),
						Error = reader.IsDBNull(reader.GetOrdinal("Error")) ? null : reader.GetString(reader.GetOrdinal("Error"))
					};

					messages.Add(message);
				}
			}
		}

		return messages;
	}
}
