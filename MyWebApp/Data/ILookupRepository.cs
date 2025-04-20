namespace MyWebApp.Data;

public interface ILookupRepository
{
	Task<int> DeleteAsync(Guid id);
	Task<List<LookupRequest>> GetAllAsync();
	Task<LookupRequest?> GetByIdAsync(Guid id);
	Task<int> InsertAsync(LookupRequest lookup);
	Task<int> UpdateAsync(LookupRequest lookup);
}