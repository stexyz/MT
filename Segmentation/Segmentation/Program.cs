using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Segmentation
{
    internal class Program
    {
        //private const string ordersPath = @"c:\Users\Ste\Desktop\report\orders2013.xml";
        private const string ordersPath = @"c:\Users\Ste\Desktop\report\orders(Jan1-Mar17).xml";
       
        private const string productsPath = @"c:\Users\Ste\Desktop\report\Segmentation\products.csv";

        private static void Main(string[] args)
        {
            List<Product> products = CsvProductParser.ReadCsvProducts(productsPath);
            List<OrderItem> orders = XmlOrdersParser.ReadOrders(ordersPath, products).ToList();

            StringBuilder rep2013 = new StringBuilder();
            //for (int i = 2; i < 12; i++)
            //{
            //    DateTime end = new DateTime(2013,i,1);
            //    DateTime start = new DateTime(2013, i - 1, 1);
            //    DateTime t = end.AddMonths(1);
            //    List<OrderItem> ordersInMonth = orders.Where(o => o.Product != null
            //                                                           && o.Date >= start &&
            //                                                           o.Date <= end).ToList();
            //    rep2013.AppendLine(string.Format("{0} - {1}: {2}", start, end,
            //                                     CreateTotalReport(products, ordersInMonth)));
            //}
            List<OrderItem> or = orders.Where(o => o.Product != null
                                                   && o.Date >= new DateTime(2014, 2, 1) &&
                                                   o.Date <= new DateTime(2014, 2, 28)).ToList();
            rep2013.AppendLine(string.Format("{0} - {1}: {2}", "1.1.2014", "31.1.2013",
                                             CreateTotalReport(products, or)));

            var monthRep = CreateTotalReport(products, or);
            string s = rep2013.ToString();
            //StringBuilder report = CreateCategoryReports(products, ordersWithProducts);

            //int i = ordersWithProducts.Count();

            //string failures = OrderItem.failures.ToString();
            //string noCodes = OrderItem.noCode.ToString();
            //string gotCodes = OrderItem.gotCode.ToString();
            //string noCodesOrderIds = OrderItem.noCodeOrder.ToString();
            //string postAndPayment = OrderItem.postAndPayment.ToString();
            //string repStr = report.ToString();

            //string totalReport = CreateTotalReport(products, ordersWithProducts).ToString();
            //Console.WriteLine(totalReport);
            Console.ReadKey();
        }

        private static StringBuilder CreateTotalReport(IEnumerable<Product> products,
                                                           IEnumerable<OrderItem> ordersWithProducts){
            int totalItems = 0;
            decimal totalPrice = 0, totalMargin = 0;
            StringBuilder report = new StringBuilder();

            foreach (OrderItem ordersWithProduct in ordersWithProducts)
            {
                //report.AppendLine(ordersWithProduct.ToString());
                totalItems += ordersWithProduct.Amount;
                decimal purchasePrice = ordersWithProduct.Product.PurchasePrice;
                decimal price = ordersWithProduct.UnitPrice;
                totalMargin += ordersWithProduct.Amount*(price - purchasePrice);
                totalPrice += ordersWithProduct.Amount * price ;
            }
            decimal relativeMargin = totalMargin / totalPrice * 100;
            report.AppendLine(string.Format("Total items: {0}, total price: {1:C}, total margin: {2:C} ({3:0.00}%).", totalItems, totalPrice, totalMargin, relativeMargin));
            return report;
        }


        private static StringBuilder CreateCategoryReports(IEnumerable<Product> products, IEnumerable<OrderItem> ordersWithProducts)
        {
            int totalItems = 0;
            decimal totalPrice = 0, totalMargin = 0;
            string[] categories = products.SelectMany(p => p.Categories).Distinct().ToArray();

            StringBuilder report = new StringBuilder();

            List<CategoryReport> categoryReports = new List<CategoryReport>();
            foreach (string category in categories)
            {
                List<OrderItem> ordersFromCategory = ordersWithProducts.Where(o => o.Product.Categories.Contains(category)).ToList();
                CategoryReport catRep = new CategoryReport(category, ordersFromCategory);
                categoryReports.Add(catRep);
                report.AppendLine(catRep.ToString());
                totalItems += catRep.ItemCount;
                totalMargin += catRep.TotalMargin;
                totalPrice += catRep.TotalPrice;
            }
            decimal relativeMargin = totalMargin/totalPrice*100;
            report.AppendLine(string.Format("Total items: {0}, total price: {1:C}, total margin: {2:C} ({3:0.00}%).", totalItems, totalPrice, totalMargin, relativeMargin));
            return report;
        }
    }
}
