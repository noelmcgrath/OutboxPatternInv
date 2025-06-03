namespace MyWebApp.Data;

public interface ILookupRepository
{
	//Task<List<LookupRequest>> GetAllAsync();
	//Task<LookupRequest?> GetByIdAsync(Guid id);
	Task<int> InsertAsync(LookupRequest lookup, SqlConnection connection, SqlTransaction transaction);
	//Task<int> UpdateAsync(LookupRequest lookup);
}