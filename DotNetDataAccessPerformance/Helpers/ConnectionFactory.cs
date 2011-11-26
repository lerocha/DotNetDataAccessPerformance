using System.Data.SqlClient;

namespace DotNetDataAccessPerformance.Helpers
{
	public class ConnectionFactory
	{
		private const string ConnectionString = "data source=localhost;initial catalog=Chinook;integrated security=True;multipleactiveresultsets=True;";

		public static SqlConnection OpenConnection(string connectionString)
		{
			var connection = new SqlConnection(connectionString);
			connection.Open();
			return connection;
		}

		public static SqlConnection OpenConnection()
		{
			return OpenConnection(ConnectionString);
		}
	}
}
