using System.Data.Common;
using System.Text.Json;

namespace MyWebApp.Data
{
	public class LookupEventRepository : ILookupEventRepository
	{
		private readonly CCS.Common.DAL.IGateway c_DALGateway;

		public LookupEventRepository(
			CCS.Common.DAL.IGateway DALGateway)
		{
			this.c_DALGateway = DALGateway;
		}


		public async Task InsertAsync(
			OfferCreated offerCreated,
			SqlConnection connection,
			SqlTransaction transaction)
		{
			var messageJson = JsonSerializer.Serialize(offerCreated, new JsonSerializerOptions { IncludeFields = true });
			var occurred = DateTime.UtcNow;

			var _parameters = new List<DbParameter>();
			_parameters.Add(this.c_DALGateway.CreateParameter("@Id", DbType.Guid, offerCreated.Id));
			_parameters.Add(this.c_DALGateway.CreateStringParameter("@MessageType", DbType.String, 50, nameof(OfferCreated)));
			_parameters.Add(this.c_DALGateway.CreateStringParameter("@MessageJSON", DbType.String, 5000, messageJson));
			_parameters.Add(this.c_DALGateway.CreateParameter("@OccurredTimestamp", DbType.DateTimeOffset, offerCreated.CreationTimestamp));
			_parameters.Add(this.c_DALGateway.CreateParameter("@VersionSequence", DbType.Int32, offerCreated.VersionSequence));

			await this.c_DALGateway.ExecuteNonQueryAsyncNew(connection, transaction, MyWebApp.Data.SQLText.InsertOutbox, parameters: _parameters);
			//_logger.LogInformation("Inserted OfferCreated event into OutboxMessages.");
		}
	}
}
