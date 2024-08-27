using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wakett.Rates.Service.Core.Interfaces;
using System.Diagnostics;
using Wakett.Rates.Service.Core.Models;

namespace Wakett.Rates.Service.Infrastructure.Repositories
{
    public class CryptocurrencyRepository : ICryptocurrencyRepository
    {
        private readonly string _connectionString;

        public CryptocurrencyRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task UpsertCryptocurrencyQuotesAsync(Dictionary<string, decimal> prices)
        {
            try
            {
                // Crea un DataTable per memorizzare i dati
                var quotesTable = new DataTable();
                quotesTable.Columns.Add("Symbol", typeof(string));
                quotesTable.Columns.Add("Price", typeof(decimal));
                quotesTable.Columns.Add("LastUpdated", typeof(DateTime));

                // Popola il DataTable
                foreach (var kvp in prices)
                {
                    quotesTable.Rows.Add(kvp.Key, kvp.Value, DateTime.UtcNow);
                }

                // Utilizza SqlBulkCopy per inserire i dati nella tabella temporanea
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    // Esegui la stored procedure per aggiornare e inserire i dati
                    using (var command = new SqlCommand("dbo.UpsertCryptocurrencyQuotes", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Configura il parametro per il tipo di tabella
                        var parameter = new SqlParameter
                        {
                            ParameterName = "@Quotes",
                            SqlDbType = SqlDbType.Structured,
                            TypeName = "CryptocurrencyQuoteTableType",
                            Value = quotesTable
                        };

                        command.Parameters.Add(parameter);

                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO LOG
            }
        }


        public async Task<IEnumerable<CryptocurrencyQuoteUpdated>> GetNewQuotesAsync()
        {
            var quotes = new List<CryptocurrencyQuoteUpdated>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("SELECT Symbol, Price FROM CryptocurrencyQuotes WHERE Status = 'New'", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            quotes.Add(new CryptocurrencyQuoteUpdated
                            {
                                Symbol = reader.GetString(0),
                                NewPrice = reader.GetDecimal(1)
                            });
                        }
                    }
                }
            }

            return quotes;
        }
    }
}
