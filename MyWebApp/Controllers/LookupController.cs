using MyWebApp.Data;
using System.Text.Json;

namespace MyWebApp.Controllers;

[ApiController]
[Route("[controller]")]
public class LookupController : ControllerBase
{
	private readonly ILogger<LookupController> _logger;
	private readonly ILookupService _lookupService;

	public LookupController(
		ILogger<LookupController> logger,
		ILookupService lookupService)
	{
		_logger = logger;
		_lookupService = lookupService;
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
		_logger.LogInformation("in post");
		if (lookupRequest == null)
		{
			return BadRequest("lookupRequest is null");
		}

		lookupRequest.Id = Guid.NewGuid();
		lookupRequest.ContinuumOrderIdentifier = this.GenerateRandomString(8);

		var offerCreated = this.Build(lookupRequest);
		try
		{
			await _lookupService.SaveWithEventAsync(lookupRequest, offerCreated);
		}
		catch (Exception)
		{
			_logger.LogError("failed to save");

			throw;
		}


		return CreatedAtAction(nameof(Get), new { id = lookupRequest.Id }, lookupRequest);
	}

	private OfferCreated Build(LookupRequest lookupRequest)
	{
		return new OfferCreated(
			Guid.NewGuid(),
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
