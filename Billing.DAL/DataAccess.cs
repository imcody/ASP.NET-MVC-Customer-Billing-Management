using System;
using System.Data;
using System.Data.SqlClient;

namespace Billing.DAL
{
    public static class DataAccess
    {

        private static String ConnectionString { get; set; }

        static DataAccess()
        {
            String dataPassKey = "IIM@cda2011";

            String encryptedConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["BillingCString"].ConnectionString;

            //SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder(CryptoLib.DecryptStringAES(encryptedConnectionString, dataPassKey));
            SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder(encryptedConnectionString);

            //for Nepal server
            //SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder(encryptedConnectionString);

            ConnectionString = connectionStringBuilder.ConnectionString;
            //GlobalSettings.DatabaseName = connectionStringBuilder.InitialCatalog;
            //GlobalSettings.DBServerName = connectionStringBuilder.DataSource;



        }

        public static SqlConnection CreateConnection()
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();
            return connection;
        }

        public static SqlCommand CreateCommand(SqlConnection connection)
        {
            return connection.CreateCommand();
        }



        public static void CreateStoredprocedure(SqlCommand command, String storedProcedureName)
        {
            command.Parameters.Clear();
            command.CommandText = storedProcedureName;
            command.CommandType = CommandType.StoredProcedure;
        }

        public static void AddInParameter(SqlCommand command, String parameterName, SqlDbType paramaterType, Object paramaterValue)
        {
            command.Parameters.Add(parameterName, paramaterType);
            command.Parameters[parameterName].Value = paramaterValue;
            command.Parameters[parameterName].Direction = ParameterDirection.Input;
        }

        public static void ClearParameter(SqlCommand command)
        {
            command.Parameters.Clear();
        }
        public static void AddOutParameter(SqlCommand command, String parameterName, SqlDbType paramaterType, Object paramaterValue)
        {
            command.Parameters.Add(parameterName, paramaterType);
            command.Parameters[parameterName].Value = paramaterValue;
            command.Parameters[parameterName].Direction = ParameterDirection.Output;
        }

        public static Object GetOutParameter(SqlCommand command, String parameterName)
        {
            return command.Parameters[parameterName].Value;
        }

        //public static int ExecuteNonQuery(SqlCommand command, SqlTransaction transction)
        //{
        //    command.Transaction = transction;
        //    return command.ExecuteNonQuery();
        //}

        public static int ExecuteNonQuery(SqlCommand command)
        {
            return command.ExecuteNonQuery();
        }
        public static int ExecuteNonQuery(SqlCommand command, SqlTransaction tran)
        {
            command.Transaction = tran;
            return command.ExecuteNonQuery();
        }

        public static SqlDataReader ExecuteReader(SqlCommand command)
        {
            return command.ExecuteReader();
        }

        public static Object ExecuteScalar(SqlCommand command)
        {
            Object returnObject = command.ExecuteScalar();
            return returnObject.Equals(DBNull.Value) ? null : returnObject;
        }

        public static Object ExecuteScalar(SqlCommand command, SqlTransaction transction)
        {
            command.Transaction = transction;
            Object returnObject = command.ExecuteScalar();
            return returnObject.Equals(DBNull.Value) ? null : returnObject;
        }
    }
}
