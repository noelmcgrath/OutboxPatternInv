using System.Data.Common;

namespace MyWebApp.Data;

public class LookupRepository : ILookupRepository
{
	private readonly CCS.Common.DAL.IGateway c_DALGateway;

	public LookupRepository(
		CCS.Common.DAL.IGateway DALGateway)
	{
		this.c_DALGateway = DALGateway;
	}

	public async Task<int> InsertAsync(
		LookupRequest lookup,
		SqlConnection connection,
		SqlTransaction transaction)
	{
		var _parameters = new List<DbParameter>();
		_parameters.Add(this.c_DALGateway.CreateParameter("@Id", DbType.Guid, lookup.Id));
		_parameters.Add(this.c_DALGateway.CreateStringParameter("@ContinuumOrderIdentifier", DbType.AnsiString, 50, lookup.ContinuumOrderIdentifier));
		_parameters.Add(this.c_DALGateway.CreateStringParameter("@MerchantOrderIdentifier", DbType.String, 50, lookup.MerchantOrderIdentifier));
		_parameters.Add(this.c_DALGateway.CreateParameter("@MerchantId", DbType.Int32, lookup.MerchantId));
		_parameters.Add(this.c_DALGateway.CreateParameter("@SaleCurrencyId", DbType.Int32, lookup.SaleCurrencyId));
		_parameters.Add(this.c_DALGateway.CreateDecimalParameter("@SaleValue", DbType.Decimal, 19, 4, lookup.SaleValue));
		_parameters.Add(this.c_DALGateway.CreateParameter("@ResultCodeId", DbType.Int32, lookup.ResultCodeId));
		_parameters.Add(this.c_DALGateway.CreateParameter("@CreationTimestamp", DbType.DateTimeOffset, lookup.CreationTimestamp));
		_parameters.Add(this.c_DALGateway.CreateParameter("@VersionSequence", DbType.Int32, lookup.VersionSequence));
		_parameters.Add(this.c_DALGateway.CreateStringParameter("@OrderSessionId", DbType.String, 300, lookup.OrderSessionId));

		return await this.c_DALGateway.ExecuteNonQueryAsyncNew(connection, transaction, MyWebApp.Data.SQLText.InsertLookup, parameters: _parameters);
	}
}