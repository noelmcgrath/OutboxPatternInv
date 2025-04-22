using MyWebApp.Messaging.Events;
using System.Text.Json;

namespace MyWebApp.Data
{
	public class LookupEventRepository : ILookupEventRepository
	{
		public async Task<int> InsertAsync(
			OfferCreated offerCreated,
			SqlConnection connection,
			SqlTransaction transaction)
		{
			var messageJson = JsonSerializer.Serialize(offerCreated);
			var occurred = DateTime.UtcNow;

			var query = @"
                INSERT INTO dbo.OutboxMessages (
                    Id,
                    MessageType,
                    MessageJSON,
                    OccurredTimestamp,
                    VersionSequence
                )
                VALUES (
                    @Id,
                    @MessageType,
                    @MessageJSON,
                    @OccurredTimestamp,
                    @VersionSequence
                )";

			using var command = new SqlCommand(query, connection, transaction);

			command.Parameters.Add(new SqlParameter("@Id", SqlDbType.UniqueIdentifier) { Value = offerCreated.Id });
			command.Parameters.Add(new SqlParameter("@MessageType", SqlDbType.VarChar, 50) { Value = nameof(OfferCreated) });
			command.Parameters.Add(new SqlParameter("@MessageJSON", SqlDbType.NVarChar) { Value = messageJson });
			command.Parameters.Add(new SqlParameter("@OccurredTimestamp", SqlDbType.DateTime) { Value = offerCreated.CreationTimestamp });
			command.Parameters.Add(new SqlParameter("@VersionSequence", SqlDbType.Int) { Value = offerCreated.VersionSequence });

			var result = await command.ExecuteNonQueryAsync();
			return result;

			//_logger.LogInformation("Inserted OfferCreated event into OutboxMessages.");
		}
	}
}
