using Microsoft.Extensions.Configuration;

namespace MyWebApp.Data
{
	public class LookupService : ILookupService
	{
		private readonly ILookupRepository _lookupRepo;
		private readonly ILookupEventRepository _eventRepo;
		private readonly string _connectionString;

		public LookupService(
			IConfiguration configuration,
			ILookupRepository lookupRepo,
			ILookupEventRepository eventRepo,
			IConfiguration config)
		{
			_lookupRepo = lookupRepo;
			_eventRepo = eventRepo;
			_connectionString = configuration.GetConnectionString("DefaultConnection");
		}

		public async Task SaveWithEventAsync(LookupRequest request, OfferCreated offerCreated)
		{
			using var connection = new SqlConnection(_connectionString);
			await connection.OpenAsync();

			using var transaction = connection.BeginTransaction();

			try
			{
				await _lookupRepo.InsertAsync(request, connection, transaction);
				await _eventRepo.InsertAsync(offerCreated, connection, transaction);

				await transaction.CommitAsync();
			}
			catch
			{
				await transaction.RollbackAsync();
				throw;
			}
		}

	}
}
