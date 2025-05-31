namespace OutboxProcessor.Types;

public class OfferCreated : CCS.Messaging.Contract.Event
{
	public Guid OfferId { get; set; }
	public string ContinuumOrderIdentifier { get; set; }

	public string? MerchantOrderIdentifier { get; set; }

	public int MerchantId { get; set; }

	public int SaleCurrencyId { get; set; }

	public decimal SaleValue { get; set; }

	public int ResultCodeId { get; set; }

	public DateTime CreationTimestamp { get; set; }

	public int VersionSequence { get; set; }

	public string? OrderSessionId { get; set; }

	public OfferCreated(
		Guid id,
		string correlationId,
		Guid offerid,
		string continuumOrderIdentifier,
		string? merchantOrderIdentifier,
		int merchantId,
		int saleCurrencyId,
		decimal saleValue,
		int resultCodeId,
		DateTime creationTimestamp,
		int versionSequence,
		string? orderSessionId) : base(id, correlationId)
	{
		OfferId = offerid;
		ContinuumOrderIdentifier = continuumOrderIdentifier;
		MerchantOrderIdentifier = merchantOrderIdentifier;
		MerchantId = merchantId;
		SaleCurrencyId = saleCurrencyId;
		SaleValue = saleValue;
		ResultCodeId = resultCodeId;
		CreationTimestamp = creationTimestamp;
		VersionSequence = versionSequence;
		OrderSessionId = orderSessionId;
	}
}
