namespace OutboxProcessor;

public class AppSettings
{
	public string DatabaseConnectionString { get; private set; }
	public string MessagingBusConnectionStringSettings { get; private set; }
}
