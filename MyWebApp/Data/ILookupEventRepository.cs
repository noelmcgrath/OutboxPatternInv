
namespace MyWebApp.Data
{
	public interface ILookupEventRepository
	{
		Task<int> InsertAsync(OfferCreated offerCreated, SqlConnection connection, SqlTransaction transaction);
	}
}