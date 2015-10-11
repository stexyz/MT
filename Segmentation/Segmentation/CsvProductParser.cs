using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Segmentation {
    public static class CsvProductParser
    {
        public static readonly List<Product> PaymentProducts = new List<Product>
            {
                new Product("", @"Dobírkou", "Payment", "", "", "", "0"),
                new Product("", @"Převodem na účet", "Payment", "", "", "", "0"),
                new Product("", @"Převodem", "Payment", "", "", "", "0"),
                new Product("", @"PayU", "Payment", "", "", "", "0"),
                new Product("", @"Hotově", "Payment", "", "", "", "0"),
                new Product("", @"Platba hotově", "Payment", "", "", "", "0"),
            };

        public static readonly List<Product> ShippingProducts = new List<Product>{ 
                new Product("", @"Česká pošta - standardní balík", "Shipping", "", "", "", "0"),
                new Product("", @"Osobní odběr", "Shipping", "", "", "", "0"),
                new Product("", @"Česká pošta - obchodní balík", "Shipping", "", "", "", "0"),
                new Product("", @"PPL", "Shipping", "", "", "", "0"),
                new Product("", @"Česká Pošta", "Shipping", "", "", "", "0"),
            };

        public static List<Product> ReadCsvProducts(string path) {
            List<Product> result = new List<Product>();
            result.AddRange(PaymentProducts);
            result.AddRange(ShippingProducts);
            StreamReader textReader = new StreamReader(path, Encoding.Default, true);
            //TextReader textReader = new StreamReader(path, new UTF8Encoding(true), true);
            string line = textReader.ReadLine();
            line = textReader.ReadLine();
            while (line != null) {
                string[] columns = line.Split(";".ToCharArray()).Select(c => c.Replace("\"", string.Empty)).ToArray();
                Product product = new Product(columns[0], columns[1], columns[2], columns[3], columns[4], columns[5], columns[6]);
                result.Add(product);
                line = textReader.ReadLine();
            }
            return result;
        }

    }
}
