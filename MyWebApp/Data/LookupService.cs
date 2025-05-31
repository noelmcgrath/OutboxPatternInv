
using MyWebApp.Controllers;

namespace MyWebApp.Data;

public class LookupService : ILookupService
{
	private readonly ILogger<LookupService> _logger;
	private readonly ILookupRepository _lookupRepo;
	private readonly ILookupEventRepository _eventRepo;
	private readonly string _connectionString;

	public LookupService(
		ILogger<LookupService> logger,
		IConfiguration configuration,
		ILookupRepository lookupRepo,
		ILookupEventRepository eventRepo,
		IConfiguration config)
	{
		_logger = logger;
		_lookupRepo = lookupRepo;
		_eventRepo = eventRepo;
		_connectionString = configuration["databaseConnectionString"]!;
	}

	public async Task SaveWithEventAsync(LookupRequest request, OfferCreated offerCreated)
	{
		_logger.LogInformation("in SaveWithEventAsync");
		using var connection = new SqlConnection(_connectionString);
		_logger.LogInformation("in SaveWithEventAsync2");
		await connection.OpenAsync();
		_logger.LogInformation("in SaveWithEventAsync3");


		using var transaction = connection.BeginTransaction();

		try
		{
			await _lookupRepo.InsertAsync(request, connection, transaction);
			await _eventRepo.InsertAsync(offerCreated, connection, transaction);

			await transaction.CommitAsync();
			_logger.LogInformation("in CommitAsync");
		}
		catch
		{
			_logger.LogInformation("in RollbackAsync");
			await transaction.RollbackAsync();
			throw;
		}
	}

}
