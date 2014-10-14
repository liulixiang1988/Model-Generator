using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using Dapper;

namespace GenerateModel
{
    public class DbUtil
    {
        private static readonly DbUtil _default = new DbUtil();
        public static DbUtil Default { get { return _default; } }

        public string ConnectionString { get; set; }

        public string DatabaseType { get; set; }
        public IDbConnection GetConnection()
        {
            IDbConnection connection;
            switch (DatabaseType)
            {
                case DBTypes.SqlServer:
                    connection = new SqlConnection(ConnectionString);
                    break;
                case DBTypes.OleDb:
                    connection = new OdbcConnection(ConnectionString);
                    break;
                default:
                    connection = new SqlConnection(ConnectionString);
                    break;
            }
            connection.Open();
            return connection;
        }

        public DbUtil()
        {
            
        }

        public DbUtil(string connectionString, string databaseType)
        {
            ConnectionString = connectionString;
            DatabaseType = databaseType;
        }

        public IEnumerable<T> Query<T>(string sql, object param)
        {
            using (var conn = GetConnection())
            {
                return conn.Query<T>(sql, param);
            }
        }

        public IEnumerable<T> Query<T>(string sql, object param, CommandType commandType)
        {
            using (var conn = GetConnection())
            {
                return conn.Query<T>(sql, param, commandType: commandType);
            }
        }
    }
}
