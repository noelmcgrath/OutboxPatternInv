
namespace MyWebApp.Data
{
	public interface ILookupService
	{
		Task SaveWithEventAsync(LookupRequest request, OfferCreated offerCreated);
	}
}