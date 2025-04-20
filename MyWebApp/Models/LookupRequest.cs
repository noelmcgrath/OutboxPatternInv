namespace MyWebApp.Models;

public class LookupRequest
{
	public Guid Id { get; set; }

	public string ContinuumOrderIdentifier { get; set; } = string.Empty;

	public string? MerchantOrderIdentifier { get; set; }

	public int MerchantId { get; set; }

	public int SaleCurrencyId { get; set; }

	public decimal SaleValue { get; set; }

	public int ResultCodeId { get; set; }

	public DateTime CreationTimestamp { get; set; }

	public int VersionSequence { get; set; }

	public string? OrderSessionId { get; set; }
}
