using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wakett.Rates.Service.Core.Interfaces;
using Wakett.Rates.Service.Core.Models;

namespace Wakett.Rates.Service.Infrastructure.Repositories
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        private readonly string _connectionString;
        private readonly string CodApp = "Wakett.Rates.Service";

        public ConfigurationRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public TaskConfiguration GetTaskConfiguration()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"SELECT * FROM TaskConfiguration WHERE CodApp = '{CodApp}'", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new TaskConfiguration
                    {
                        TaskId = (int)reader["TaskId"],
                        TickTime = (int)reader["TickTime"],
                        LastRunTime = reader["LastRunTime"] as DateTime?
                    };
                }

                return null;
            }
        }

        public void UpdateLastRunTime(int taskId, DateTime lastRunTime)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"UPDATE TaskConfiguration SET LastRunTime = @LastRunTime WHERE TaskId = @TaskId AND CodApp = '{CodApp}'", conn);
                cmd.Parameters.AddWithValue("@LastRunTime", lastRunTime);
                cmd.Parameters.AddWithValue("@TaskId", taskId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}

