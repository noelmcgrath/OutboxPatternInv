namespace MyWebApp.Data;

public static class SQLText
{
	public const string InsertLookup = @"
		INSERT INTO dbo.Lookup(
			Id,
			ContinuumOrderIdentifier,
			MerchantOrderIdentifier,
			MerchantId,
			SaleCurrencyId,
			SaleValue,
			ResultCodeId,
			CreationTimestamp,
			VersionSequence,
			OrderSessionId)
		VALUES(
			@Id,
			@ContinuumOrderIdentifier,
			@MerchantOrderIdentifier,
			@MerchantId,
			@SaleCurrencyId,
			@SaleValue,
			@ResultCodeId,
			@CreationTimestamp,
			@VersionSequence,
			@OrderSessionId
		)";

	public const string InsertOutbox = @"
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
}
