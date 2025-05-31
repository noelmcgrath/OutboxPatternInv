
namespace MyWebApp.Data
{
	public interface ILookupEventRepository
	{
		Task InsertAsync(OfferCreated offerCreated, SqlConnection connection, SqlTransaction transaction);
	}
}