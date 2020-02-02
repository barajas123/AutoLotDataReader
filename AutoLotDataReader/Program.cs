using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Management.Instrumentation;
using System.Security.Cryptography.X509Certificates;
using static System.Console;

namespace AutoLotDataReader
{
    class Program
    {
        public enum ConnectionState
        {
            Broken,
            Closed,
            Connecting,
            Executing,
            Fetching,
            Open
        }

        public enum CommandType
        {
            StoredProcedure,
            TableDirect,
            Text // Default value.
        }
        static void Main(string[] args)
        { 
            WriteLine("**** Fun with Data Readers ****\n"); 
            
            //Create a connection string using the stringbuilder
            var cnStringBuilder = new SqlConnectionStringBuilder
            {
                InitialCatalog = "AutoLot",
                DataSource = "masterdb.cc4nxjhwbbqs.us-east-2.rds.amazonaws.com,1433",
                ConnectTimeout = 30,
                UserID = "admin",
                Password = "507916ab"

            };



            // Create and open a connection.

            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = cnStringBuilder.ConnectionString;
                connection.Open();

                // Create a SQL command object
                string sql = "SELECT * FROM Inventory; SELECT * FROM Customers";
                SqlCommand myCommand = new SqlCommand(sql,connection);
                // Create another command object via properties
                SqlCommand testCommand = new SqlCommand();
                testCommand.Connection = connection;
                testCommand.CommandText = sql;

                // obtain a data reader a la ExecuteReader()
                using (SqlDataReader myDataReader = myCommand.ExecuteReader())
                {
                    // loop over the results
                    while (myDataReader.Read())
                    {
                        WriteLine("*** Record ***");
                        for (int i = 0;i < myDataReader.FieldCount; i++ )
                        {
                            WriteLine($"{myDataReader.GetName(i)} = {myDataReader.GetValue(i)}");
                        }

                        WriteLine();
                    }

                    while (myDataReader.NextResult()) ;
                }
            }
            ReadLine();
        }

        public static void ShowConnectionStatus(SqlConnection connection)
        {
            // show various stats about the current connection object
            WriteLine("**** Info about your connection *****");
            WriteLine($"Database location: {connection.DataSource}");
            WriteLine($"Database name: {connection.Database}");
            WriteLine($"Timeout: {connection.ConnectionTimeout}");
            WriteLine($"Connection state: {connection.State}\n");
        }
    }
}
