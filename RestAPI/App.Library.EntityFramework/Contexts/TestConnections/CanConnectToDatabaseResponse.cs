using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace App.Library.EntityFramework.Contexts.TestConnections
{
    public class CanConnectToDatabaseResponse:ICanConnectToDatabase
    {
        public DatabaseConnectionTestResponse status { get; set; }
        public CanConnectToDatabaseResponse verify(string conection_string)
        {
            status_response = (TestConnectionString(conection_string)) ? DatabaseConnectionTestResponse.ConnectionEstablished : DatabaseConnectionTestResponse.FailedToEstablishConnection;
            return new CanConnectToDatabaseResponse()
            {
                status = status_response
            };
        }
        private bool TestConnectionString(string connectionString)
        {

            var defulat_value = false;
            try
            {
                var connection = new SqlConnection(connectionString);
                try
                {
                    if (connection.State != ConnectionState.Open )
                    {
                        connection.Open();
                        defulat_value = connection.State == ConnectionState.Open;
                    }
                }
                finally
                {
                    connection.Close();
                }

            }
            catch
            {
                return defulat_value;
            }
            return defulat_value;


        }
        private DatabaseConnectionTestResponse status_response;
    }
}