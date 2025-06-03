namespace MyWebApp.Service;

public class SaveLookup : ISaveLookup
{
	private readonly ILogger<SaveLookup> _logger;
	private readonly CCS.Common.DAL.IGateway _DALGateway;
	private readonly ILookupRepository _lookupRepository;
	private readonly ILookupEventRepository _lookupEventRepository;

	public SaveLookup(
		ILogger<SaveLookup> logger,
		IConfiguration configuration,
		CCS.Common.DAL.IGateway DALGateway,
		ILookupRepository lookupRepository,
		ILookupEventRepository lookupEventRepository,
		IConfiguration config)
	{
		_logger = logger;
		_DALGateway = DALGateway;
		_lookupRepository = lookupRepository;
		_lookupEventRepository = lookupEventRepository;
	}

	public async Task SaveWithEventAsync(LookupRequest request, OfferCreated offerCreated)
	{
		_logger.LogInformation("in SaveWithEventAsync");
		await using var connection = await _DALGateway.CreateConnectionAsync();
		using var transaction = connection.BeginTransaction();

		try
		{
			await _lookupRepository.InsertAsync(request, connection, transaction);
			await _lookupEventRepository.InsertAsync(offerCreated, connection, transaction);

			await transaction.CommitAsync();
			_logger.LogInformation("in CommitAsync");
		}

		catch (Exception ex)
		{
			_logger.LogError(ex, "Exception occurred in SaveWithEventAsync");
			_logger.LogInformation("in RollbackAsync");
			await transaction.RollbackAsync();
			throw;
		}
	}
}
