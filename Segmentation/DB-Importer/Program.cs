using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DB_Importer {
    class Program {
        const string SqlConnectionString = "Server=tcp:bv9hf12q2c.database.windows.net,1433;Database=mtapi;User ID=stexyz@bv9hf12q2c;Password=3300Spawn3300;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        private const string OrdersTableName = "dbo.Orders";
        private const string ProductsTableName = "dbo.Products";
        private const string ErrorsTableName = "dbo.Errors";
        static void Main(string[] args)
        {
            DateTime time = DateTime.Now;
            Console.WriteLine("Getting orders from web.");
  //          StreamReader csvReaderOrders = GetReaderFromUrl(@"http://www.megatenis.cz/export/orders.csv?patternId=16&hash=aafffe4d6e988b999d0061e455a2912d93bf63f4ad2a5ecacc960ed4576af788");
            Console.WriteLine("{0}: Getting orders from web done in {0}s.", (DateTime.Now - time).TotalSeconds);
//            ImportCsvBulk(new StreamReader(@"C:\Users\Ste\Downloads\orders.csv", Encoding.Default, true), "dbo.Orders");
//            ImportCsv(new StreamReader(@"C:\Users\Ste\Downloads\orders.csv", Encoding.Default, true), "dbo.Orders");
//            ImportCsv(csvReaderOrders, OrdersTableName);
//            StreamReader csvReaderProducts = GetReaderFromUrl(@"http://www.megatenis.cz/export/products.csv?visibility=-1&patternId=17&hash=26f694754fc716de890834e0c92904972f3c73938bbdb06f901bbd5326eb6abd");
            ImportCsv(new StreamReader(@"C:\Users\Ste\Downloads\products.csv", Encoding.Default, true), "dbo.Products");
//            ImportCsv(csvReaderProducts, ProductsTableName);
        }

        private static StreamReader GetReaderFromUrl(string url) {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Timeout = 600000;
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            StreamReader sr = new StreamReader(resp.GetResponseStream(), Encoding.Default, true);
            return sr;
        }


        private static void ImportCsvBulk(StreamReader textReader, string tableName) {
            using (textReader) {
                string line = textReader.ReadLine();
                int id = 1;
                string[] columns = line.Split(";".ToCharArray()).Select(c => c.Replace("\"", string.Empty)).ToArray();
                using (SqlConnection connection = new SqlConnection(SqlConnectionString)) {
                    connection.Open();
                    ExecuteCommand(connection, "DELETE FROM " + tableName);

                    SqlBulkCopy bulkCopy = new SqlBulkCopy(connection);
                    bulkCopy.DestinationTableName = tableName;
                    DataTable dataTable = new DataTable(tableName);

                    foreach (string column in columns.Take(columns.Length - 1)) {
                        dataTable.Columns.Add(column);
                    }

                    line = textReader.ReadLine();
                    string[] values;
                    try
                    {
                        while (line != null && id++ < 5)
                        {
                            values = line.Split(";".ToCharArray())
                                                  .Select(c => c.Replace("'", "-"))
                                //escape the ' (in pro's pro for instance)
                                                  .Select(c => c == "" ? "''" : c) //replace empty for ''
                                                  .Select(c => c.Replace("\"", "'")) //replace " for '
                                                  .Select(c => c.Replace(",", "."))
                                //replace decimal , for . This may change some texts too, but rather hack it like that..
                                                  .ToArray();

                            //in the csv file there is a trailing delimiter resulting in an empty last item -> leave out the last one

                            DataRow row = dataTable.NewRow();
                            for (int i = 0; i < values.Count(); i++)
                            {
                                row[i] = values[i];
                            }
                            dataTable.Rows.Add(row);
                            line = textReader.ReadLine();
                        }

                        bulkCopy.WriteToServer(dataTable);
                    } catch (Exception e) {
                        ExecuteCommand(connection, String.Format("INSERT INTO dbo.Errors (shortDescription, longDescription) VALUES ({0}, {1})", e, e));
//                        Console.WriteLine("\n--------------\nExecuting command: \n" + cmd + "\n--------------\n");
                        Console.WriteLine(e);
                    }
                }
            }
        }

        private static void ImportCsv(StreamReader textReader, string tableName)
        {
            DateTime time = DateTime.Now;
            using (textReader)
            {
                string line = textReader.ReadLine();
                int id = 1;
                string[] columns = line.Split(";".ToCharArray()).Select(c => c.Replace("\"", string.Empty)).ToArray();
                string columsString = string.Join(",", columns.Take(columns.Length - 1));
                using (SqlConnection connection = new SqlConnection(SqlConnectionString)){
                    connection.Open();
                    ExecuteCommand(connection, "DELETE FROM " + tableName);
                    string cmd = "";
                    line = textReader.ReadLine();
                    while (line != null)
                    {
                        try
                        {
                            string[] values = line.Split(";".ToCharArray())
                                                  .Select(c => c.Replace("'", "-"))     //escape the ' (in pro's pro for instance)
                                                  .Select(c => c == "" ? "''" : c)      //replace empty for ''
                                                  .Select(c => c.Replace("\"", "'"))    //replace " for '
                                                  .Select(c => c.Replace(",", "."))
                                //replace decimal , for . This may change some texts too, but rather hack it like that..
                                                  .ToArray();

                            //in the csv file there is a trailing delimiter resulting in an empty last item -> leave out the last one
                            string valuesString = string.Join(", ", values.Take(values.Length - 1));

                            cmd = String.Format("INSERT {0} ({1}) values ({2})", tableName, columsString, valuesString);
                            ExecuteCommand(connection, cmd);

                            line = textReader.ReadLine();
                        }
                        catch (Exception e)
                        {
                            //ExecuteCommand(connection, String.Format("INSERT INTO dbo.Errors (shortDescription, longDescription) VALUES ({0}, {1})", cmd, e));
                            Console.WriteLine("\n--------------\nExecuting command: \n" + cmd + "\n--------------\n");
                            Console.WriteLine(e);
                        }

                        if (id++ % 100 == 0) {
                            Console.WriteLine("Imported lines: {0}. Last 100 processed in {1}s.", id, (DateTime.Now - time).TotalSeconds);
                            time = DateTime.Now;
                            Console.SetCursorPosition(0, Console.CursorTop);
                        }
                    }
                }
            }
        }

        private static void ExecuteCommand(SqlConnection connection, string commandText)
        {
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = commandText;
                command.ExecuteNonQuery();
        }
    }
}




// Read data from the query.
//                SqlDataReader reader = command.ExecuteReader();
//                while (reader.Read()) {
//                    // Formatting will depend on the contents of the query.
//                    Console.WriteLine("Value: {0}, {1}",
//                        reader[0], reader[1]);
//                }
