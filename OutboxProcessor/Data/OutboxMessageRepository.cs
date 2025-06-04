
using OutboxProcessor.Types;
using System.Data.Common;

namespace OutboxProcessor.Data;

public class OutboxMessageRepository : IOutboxMessageRepository
{
	private readonly CCS.Common.DAL.IGateway c_DALGateway;

	public OutboxMessageRepository(
		CCS.Common.DAL.IGateway DALGateway)
	{
		this.c_DALGateway = DALGateway;
	}

	public async Task<List<OutboxMessage>> FetchUnprocessedMessagesAsync(
		SqlConnection connection,
		SqlTransaction transaction,
		int maxCount = 100)
	{
		var messages = new List<OutboxMessage>();

		var _parameters = new List<DbParameter>
		{
			this.c_DALGateway.CreateParameter("@MaxCount", DbType.Int32, maxCount)
		};

		var unprocessedMessagesDataSet = await this.c_DALGateway.ExecuteDataSetAsyncNew(connection, transaction, OutboxProcessor.Data.SQLText.FetchUnprocessedMessages, parameters: _parameters);
		return this.Map(unprocessedMessagesDataSet.Tables[0]);

		//var query = @"
  //                  SELECT TOP (@MaxCount) 
  //                      Id,
  //                      MessageType,
  //                      MessageJSON,
  //                      OccurredTimestamp,
  //                      ProcessedTimestamp,
  //                      VersionSequence,
  //                      Error
  //                  FROM dbo.OutboxMessages
  //                  WHERE ProcessedTimestamp IS NULL
  //                  ORDER BY OccurredTimestamp ASC";

		//using (var connection = new SqlConnection(_connectionString))
		//using (var command = new SqlCommand(query, connection))
		//{
		//	command.Parameters.Add(new SqlParameter("@MaxCount", SqlDbType.Int) { Value = maxCount });

		//	await connection.OpenAsync();

		//	using (var reader = await command.ExecuteReaderAsync())
		//	{
		//		while (await reader.ReadAsync())
		//		{
		//			var message = new OutboxMessage
		//			{
		//				Id = reader.GetGuid(reader.GetOrdinal("Id")),
		//				MessageType = reader.GetString(reader.GetOrdinal("MessageType")),
		//				MessageJSON = reader.IsDBNull(reader.GetOrdinal("MessageJSON")) ? null : reader.GetString(reader.GetOrdinal("MessageJSON")),
		//				OccurredTimestamp = reader.GetDateTime(reader.GetOrdinal("OccurredTimestamp")),
		//				ProcessedTimestamp = reader.IsDBNull(reader.GetOrdinal("ProcessedTimestamp")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("ProcessedTimestamp")),
		//				VersionSequence = reader.GetInt32(reader.GetOrdinal("VersionSequence")),
		//				Error = reader.IsDBNull(reader.GetOrdinal("Error")) ? null : reader.GetString(reader.GetOrdinal("Error"))
		//			};

		//			messages.Add(message);
		//		}
		//	}
		//}

		//return messages;
	}

	private List<OutboxMessage> Map(DataTable dataTable)
	{
		var messages = new List<OutboxMessage>();

		foreach (DataRow row in dataTable.Rows)
		{
			var message = new OutboxMessage
			{
				Id = (Guid)row["Id"],
				MessageType = (string)row["MessageType"],
				MessageJSON = row["MessageJSON"] as string ?? string.Empty, // Fix: Provide a default value to avoid null assignment
				OccurredTimestamp = (DateTime)row["OccurredTimestamp"],
				ProcessedTimestamp = row["ProcessedTimestamp"] as DateTime?,
				VersionSequence = (int)row["VersionSequence"],
				Error = row["Error"] as string ?? string.Empty // Fix: Provide a default value to avoid null assignment
			};

			messages.Add(message);
		}

		return messages;
	}
}
