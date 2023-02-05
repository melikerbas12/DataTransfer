using System.Data;
using Npgsql;
namespace DataTransfer.Console.Service
{
    public class StateConnection
    {
        private NpgsqlConnection sourceConnection = new NpgsqlConnection("Server=localhost; Port=5432; User Id=postgres; Password=1; Database=BsnBasePortal");
        private NpgsqlConnection destinationConnection = new NpgsqlConnection("Server=ec2-34-249-224-252.eu-west-1.compute.amazonaws.com; Port=5432; User Id=postgres; Password=LW3nEcQ8d5VXu4hP; Database=BsnBasePortal");
        protected NpgsqlConnection SourceConnectionOpen()
        {
            if (sourceConnection.State == ConnectionState.Closed)
                sourceConnection.Open();
            return sourceConnection;
        }
        protected void SourceConnectionClosed()
        {
            if (sourceConnection.State == ConnectionState.Open)
                sourceConnection.Close();
        }
        protected NpgsqlConnection DestinationConnectionOpen()
        {
            if (destinationConnection.State == ConnectionState.Closed)
                destinationConnection.Open();
            return destinationConnection;
        }
        protected void DestinationConnectionClosed()
        {
            if (destinationConnection.State == ConnectionState.Open)
                destinationConnection.Close();
        }
    }
}