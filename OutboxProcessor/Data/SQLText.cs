namespace OutboxProcessor.Data;

public static class SQLText
{
	public const string FetchUnprocessedMessages = @"
		SELECT TOP (@MaxCount) 
			Id,
			MessageType,
			MessageJSON,
			OccurredTimestamp,
			ProcessedTimestamp,
			VersionSequence,
			Error
		FROM dbo.OutboxMessages
		WHERE ProcessedTimestamp IS NULL
		ORDER BY OccurredTimestamp ASC";
}
