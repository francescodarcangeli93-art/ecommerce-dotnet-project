using System;
using System.Configuration;
using System.Data.SqlClient;

namespace DAL_Ecommerce
{
    public static class DbConnectionFactory
    {
        private static readonly string _connectionString;

        static DbConnectionFactory()
        {
            var cs = ConfigurationManager.ConnectionStrings["Ecommerce"];

            if (cs == null || string.IsNullOrWhiteSpace(cs.ConnectionString))
            {
                throw new Exception("Connection string 'Ecommerce' non trovata nel Web.config.");
            }

            _connectionString = cs.ConnectionString;
        }

        public static SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
