using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;

namespace mt_web_api.Services {
    public class DbImporterService : SqlService
    {
        private bool _dbReportRunning;

        public bool DbImportRunning()
        {
            return _dbReportRunning;
        }

        const string SqlConnectionString = "Server=tcp:bv9hf12q2c.database.windows.net,1433;Database=mtapi;User ID=stexyz@bv9hf12q2c;Password=3300Spawn3300;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public void ImportDataFromShoppingCardToDb()
        {
            // TODO: make it async
            // TODO: implement sql table locking
            // TODO: download the whole csv first not to exceed the http request timeout
            // cannot do via locking as the controller is created for each request. 

            _dbReportRunning = true;
//            ImportCsv(new StreamReader(@"C:\Users\Ste\Downloads\orders.csv", Encoding.Default, true), "dbo.Orders");
//            ImportCsv(new StreamReader(@"C:\Users\Ste\Downloads\products.csv", Encoding.Default, true), "dbo.Products");
            _dbReportRunning = false;

            new Thread(() =>{
                    StreamReader csvReaderOrders = GetReaderFromUrl(@"http://www.megatenis.cz/export/orders.csv?patternId=16&hash=aafffe4d6e988b999d0061e455a2912d93bf63f4ad2a5ecacc960ed4576af788");
                    ImportCsv(csvReaderOrders, OrdersTableName);
                }).Start();

            new Thread(() => {
            StreamReader csvReaderProducts = GetReaderFromUrl(@"http://www.megatenis.cz/export/products.csv?visibility=-1&patternId=17&hash=26f694754fc716de890834e0c92904972f3c73938bbdb06f901bbd5326eb6abd");
            ImportCsv(csvReaderProducts, ProductsTableName);
            }).Start();
        }

        private static void ImportCsv(StreamReader textReader, string tableName) {
            DateTime time = DateTime.Now;
            using (textReader) {
                string line = textReader.ReadLine();
                int id = 1;
                string[] columns = line.Split(";".ToCharArray()).Select(c => c.Replace("\"", string.Empty)).ToArray();
                string columsString = string.Join(",", columns.Take(columns.Length - 1));
                using (SqlConnection connection = new SqlConnection(SqlConnectionString)) {
                    System.Diagnostics.Trace.TraceInformation("DB connection open.");
                    connection.Open();
                    ExecuteCommand(connection, "TRUNCATE TABLE " + tableName);
                    System.Diagnostics.Trace.TraceInformation("DB deletion on table" + tableName +" finished.");
                    string cmd = "";
                    line = textReader.ReadLine();
                    while (line != null) {
                        try {
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
                        } catch (Exception e) {
                            //ExecuteCommand(connection, String.Format("INSERT INTO dbo.Errors (shortDescription, longDescription) VALUES ({0}, {1})", cmd, e));
                            System.Diagnostics.Trace.TraceError("\n--------------\nExecuting command: \n" + cmd + "\n--------------\n");
                            System.Diagnostics.Trace.TraceError(e.Message);
                        }

                        if (id++ % 500 == 0) {
                            System.Diagnostics.Trace.TraceInformation("Imported lines: {0}. Last 100 processed in {1}s.", id, (DateTime.Now - time).TotalSeconds);
                            time = DateTime.Now;
                        }
                    }
                }
            }
        }
        
        private static StreamReader GetReaderFromUrl(string url) {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Timeout = 600000;
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            StreamReader sr = new StreamReader(resp.GetResponseStream(), Encoding.Default, true);
            return sr;
        }
    }
}