namespace MyWebApp.Controllers;

[ApiController]
[Route("[controller]")]
public class LookupController : ControllerBase
{
	private readonly ILogger<LookupController> _logger;
	private readonly ILookupRepository _lookupRepository;
	private readonly CCS.Messaging.Contract.IBus c_messagingBus;

	public LookupController(
		ILogger<LookupController> logger,
		ILookupRepository lookupRepository,
		CCS.Messaging.Contract.IBus c_messagingBus)
	{
		_logger = logger;
		_lookupRepository = lookupRepository;
		this.c_messagingBus = c_messagingBus;
	}

	[HttpGet(Name = "GetLookup")]
	public Models.LookupRequest Get()
	{
		return new Models.LookupRequest
		{
			Id = Guid.NewGuid(),
			ContinuumOrderIdentifier = "CONT-12348",
			MerchantOrderIdentifier = "M-ORDER-9999",
			MerchantId = 101,
			SaleCurrencyId = 978, // e.g., EUR
			SaleValue = 249.99m,
			ResultCodeId = 0,
			CreationTimestamp = DateTime.UtcNow,
			VersionSequence = 1,
			OrderSessionId = "SESSION-456789"
		};
	}

	[HttpPost]
	public async Task<IActionResult> Post([FromBody] Models.LookupRequest? lookupRequest)
	{
		if (lookupRequest == null)
		{
			return BadRequest("lookupRequest is null");
		}

		lookupRequest.Id = Guid.NewGuid();
		lookupRequest.ContinuumOrderIdentifier = this.GenerateRandomString(8);

		await _lookupRepository.InsertAsync(lookupRequest);

		var offerCreated = this.Build(lookupRequest);

		try
		{
			await c_messagingBus.PublishAsync(offerCreated).WaitAsync(TimeSpan.FromSeconds(2));
		}
		catch (TimeoutException)
		{
			return StatusCode(504, "Publishing to the message bus timed out.");
		}

		return CreatedAtAction(nameof(Get), new { id = lookupRequest.Id }, lookupRequest);
	}

	private OfferCreated Build(LookupRequest lookupRequest)
	{
		return new OfferCreated(
			lookupRequest.Id,
			lookupRequest.ContinuumOrderIdentifier,
			lookupRequest.MerchantOrderIdentifier,
			lookupRequest.MerchantId,
			lookupRequest.SaleCurrencyId,
			lookupRequest.SaleValue,
			lookupRequest.ResultCodeId,
			lookupRequest.CreationTimestamp,
			lookupRequest.VersionSequence,
			lookupRequest.OrderSessionId
		);
	}

	private string GenerateRandomString(int length)
	{
		const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
		var random = new Random();
		return new string(Enumerable.Repeat(chars, length)
			.Select(s => s[random.Next(s.Length)]).ToArray());
	}
}
