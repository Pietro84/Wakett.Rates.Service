using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wakett.Rates.Service.Core.Interfaces;

namespace Wakett.Rates.Service.Infrastructure.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly string _connectionString;

        public LogRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Log(int taskId, string message, string type)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO AppLogs (TaskId, LogMessage, LogType) VALUES (@TaskId, @LogMessage, @LogType)", conn);
                cmd.Parameters.AddWithValue("@TaskId", taskId);
                cmd.Parameters.AddWithValue("@LogMessage", message);
                cmd.Parameters.AddWithValue("@LogType", type);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
