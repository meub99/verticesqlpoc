using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString =
            "Data Source=(local);Initial Catalog=XMLPerfDB;Integrated Security=true";

            // Provide the query string with a parameter placeholder.
            string queryString = "SELECT NAV from TableGL";

            List<double> values = new List<double> ();

            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                // Create the Command and Parameter objects.
                SqlCommand command = new SqlCommand(queryString, connection);
                             
                // Open the connection in a try/catch block. 
                // Create and execute the DataReader, writing the result
                // set to the console window.
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        double i = Convert.ToDouble(reader[0]);
                        values.Add(i);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }

            int mycount = values.Count();
            double myavg = values.Average();
            double mysum = values.Sum(d => (d - myavg) * (d - myavg));
            double mystddev = Math.Sqrt(mysum / mycount);

            Console.WriteLine(mystddev);

            Console.ReadLine();
        }
    }
}
