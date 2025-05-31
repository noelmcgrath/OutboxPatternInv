namespace OutboxProcessor.Types;

public class OutboxMessage
{
	public Guid Id { get; set; }
	public string MessageType { get; set; }
	public string MessageJSON { get; set; }
	public DateTime OccurredTimestamp { get; set; }
	public DateTime? ProcessedTimestamp { get; set; }
	public int VersionSequence { get; set; }
	public string Error { get; set; }
}
