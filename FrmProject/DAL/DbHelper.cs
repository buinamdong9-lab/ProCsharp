using Microsoft.Data.SqlClient;
using System.Configuration;

namespace FrmProject.DAL
{
    public static class DbHelper
    {
        private const string DefaultConnectionString =
            @"Server=localhost\SQLEXPRESS;Database=QuanLyThietBi;Trusted_Connection=True;TrustServerCertificate=True;";

        public static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["QuanLyThietBi"]?.ConnectionString
                ?? DefaultConnectionString;
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(GetConnectionString());
        }
    }
}

