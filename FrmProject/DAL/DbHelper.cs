using Microsoft.Data.SqlClient;
using System.Configuration;

namespace FrmProject.DAL
{
    public static class DbHelper
    {
        private const string DefaultConnectionString =
            @"Server=localhost\SQLEXPRESS;Database=QuanLyThietBi;Trusted_Connection=True;TrustServerCertificate=True;";

        public static SqlConnection GetConnection()
        {
            string connectionString =
                ConfigurationManager.ConnectionStrings["QuanLyThietBi"]?.ConnectionString
                ?? DefaultConnectionString;

            return new SqlConnection(connectionString);
        }
    }
}

