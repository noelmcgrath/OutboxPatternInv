using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using MyWebApp.Models;

namespace MyWebApp.Data
{
	// To load connection string from appsettings

	public class LookupRepository : ILookupRepository
	{
		private readonly string _connectionString;

		public LookupRepository(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection");
		}

		public async Task<List<LookupRequest>> GetAllAsync()
		{
			var lookups = new List<LookupRequest>();

			using var connection = new SqlConnection(_connectionString);
			await connection.OpenAsync();

			var command = new SqlCommand("SELECT * FROM dbo.Lookup", connection);

			using var reader = await command.ExecuteReaderAsync();
			while (await reader.ReadAsync())
			{
				var lookup = new LookupRequest
				{
					Id = reader.GetGuid(reader.GetOrdinal("Id")),
					ContinuumOrderIdentifier = reader.GetString(reader.GetOrdinal("ContinuumOrderIdentifier")),
					MerchantOrderIdentifier = reader.IsDBNull(reader.GetOrdinal("MerchantOrderIdentifier")) ? null : reader.GetString(reader.GetOrdinal("MerchantOrderIdentifier")),
					MerchantId = reader.GetInt32(reader.GetOrdinal("MerchantId")),
					SaleCurrencyId = reader.GetInt32(reader.GetOrdinal("SaleCurrencyId")),
					SaleValue = reader.GetDecimal(reader.GetOrdinal("SaleValue")),
					ResultCodeId = reader.GetInt32(reader.GetOrdinal("ResultCodeId")),
					CreationTimestamp = reader.GetDateTime(reader.GetOrdinal("CreationTimestamp")),
					VersionSequence = reader.GetInt32(reader.GetOrdinal("VersionSequence")),
					OrderSessionId = reader.IsDBNull(reader.GetOrdinal("OrderSessionId")) ? null : reader.GetString(reader.GetOrdinal("OrderSessionId"))
				};
				lookups.Add(lookup);
			}

			return lookups;
		}

		public async Task<LookupRequest?> GetByIdAsync(Guid id)
		{
			using var connection = new SqlConnection(_connectionString);
			await connection.OpenAsync();

			var command = new SqlCommand("SELECT * FROM dbo.Lookup WHERE Id = @Id", connection);
			command.Parameters.AddWithValue("@Id", id);

			using var reader = await command.ExecuteReaderAsync();
			if (await reader.ReadAsync())
			{
				return new LookupRequest
				{
					Id = reader.GetGuid(reader.GetOrdinal("Id")),
					ContinuumOrderIdentifier = reader.GetString(reader.GetOrdinal("ContinuumOrderIdentifier")),
					MerchantOrderIdentifier = reader.IsDBNull(reader.GetOrdinal("MerchantOrderIdentifier")) ? null : reader.GetString(reader.GetOrdinal("MerchantOrderIdentifier")),
					MerchantId = reader.GetInt32(reader.GetOrdinal("MerchantId")),
					SaleCurrencyId = reader.GetInt32(reader.GetOrdinal("SaleCurrencyId")),
					SaleValue = reader.GetDecimal(reader.GetOrdinal("SaleValue")),
					ResultCodeId = reader.GetInt32(reader.GetOrdinal("ResultCodeId")),
					CreationTimestamp = reader.GetDateTime(reader.GetOrdinal("CreationTimestamp")),
					VersionSequence = reader.GetInt32(reader.GetOrdinal("VersionSequence")),
					OrderSessionId = reader.IsDBNull(reader.GetOrdinal("OrderSessionId")) ? null : reader.GetString(reader.GetOrdinal("OrderSessionId"))
				};
			}

			return null;
		}

		public async Task<int> InsertAsync(LookupRequest lookup)
		{
			try
			{
				using var connection = new SqlConnection(_connectionString);
				await connection.OpenAsync();

				var command = new SqlCommand(@"
					INSERT INTO dbo.Lookup
					(Id, ContinuumOrderIdentifier, MerchantOrderIdentifier, MerchantId, SaleCurrencyId, SaleValue, ResultCodeId, CreationTimestamp, VersionSequence, OrderSessionId)
					VALUES
					(@Id, @ContinuumOrderIdentifier, @MerchantOrderIdentifier, @MerchantId, @SaleCurrencyId, @SaleValue, @ResultCodeId, @CreationTimestamp, @VersionSequence, @OrderSessionId)", connection);

				command.Parameters.AddWithValue("@Id", lookup.Id);
				command.Parameters.AddWithValue("@ContinuumOrderIdentifier", lookup.ContinuumOrderIdentifier);
				command.Parameters.AddWithValue("@MerchantOrderIdentifier", (object?)lookup.MerchantOrderIdentifier ?? DBNull.Value);
				command.Parameters.AddWithValue("@MerchantId", lookup.MerchantId);
				command.Parameters.AddWithValue("@SaleCurrencyId", lookup.SaleCurrencyId);
				command.Parameters.AddWithValue("@SaleValue", lookup.SaleValue);
				command.Parameters.AddWithValue("@ResultCodeId", lookup.ResultCodeId);
				command.Parameters.AddWithValue("@CreationTimestamp", lookup.CreationTimestamp);
				command.Parameters.AddWithValue("@VersionSequence", lookup.VersionSequence);
				command.Parameters.AddWithValue("@OrderSessionId", (object?)lookup.OrderSessionId ?? DBNull.Value);

				var result = await command.ExecuteNonQueryAsync();
				return result;
			}
			catch (SqlException ex)
			{
				// Log or inspect the full exception
				Console.WriteLine($"SQL Error: {ex.Message}");
				throw;
			}

		}

		public async Task<int> UpdateAsync(LookupRequest lookup)
		{
			using var connection = new SqlConnection(_connectionString);
			await connection.OpenAsync();

			var command = new SqlCommand(@"
            UPDATE dbo.Lookup
            SET ContinuumOrderIdentifier = @ContinuumOrderIdentifier,
                MerchantOrderIdentifier = @MerchantOrderIdentifier,
                MerchantId = @MerchantId,
                SaleCurrencyId = @SaleCurrencyId,
                SaleValue = @SaleValue,
                ResultCodeId = @ResultCodeId,
                CreationTimestamp = @CreationTimestamp,
                VersionSequence = @VersionSequence,
                OrderSessionId = @OrderSessionId
            WHERE Id = @Id", connection);

			command.Parameters.AddWithValue("@Id", lookup.Id);
			command.Parameters.AddWithValue("@ContinuumOrderIdentifier", lookup.ContinuumOrderIdentifier);
			command.Parameters.AddWithValue("@MerchantOrderIdentifier", (object?)lookup.MerchantOrderIdentifier ?? DBNull.Value);
			command.Parameters.AddWithValue("@MerchantId", lookup.MerchantId);
			command.Parameters.AddWithValue("@SaleCurrencyId", lookup.SaleCurrencyId);
			command.Parameters.AddWithValue("@SaleValue", lookup.SaleValue);
			command.Parameters.AddWithValue("@ResultCodeId", lookup.ResultCodeId);
			command.Parameters.AddWithValue("@CreationTimestamp", lookup.CreationTimestamp);
			command.Parameters.AddWithValue("@VersionSequence", lookup.VersionSequence);
			command.Parameters.AddWithValue("@OrderSessionId", (object?)lookup.OrderSessionId ?? DBNull.Value);

			return await command.ExecuteNonQueryAsync();
		}

		public async Task<int> DeleteAsync(Guid id)
		{
			using var connection = new SqlConnection(_connectionString);
			await connection.OpenAsync();

			var command = new SqlCommand("DELETE FROM dbo.Lookup WHERE Id = @Id", connection);
			command.Parameters.AddWithValue("@Id", id);

			return await command.ExecuteNonQueryAsync();
		}
	}
}