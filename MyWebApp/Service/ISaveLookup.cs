namespace MyWebApp.Service
{
	public interface ISaveLookup
	{
		Task SaveWithEventAsync(LookupRequest request, OfferCreated offerCreated);
	}
}