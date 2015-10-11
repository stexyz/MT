using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportingLib
{
    public class ReportGenerator
    {
        public enum ReportType
        {
            Summary,
            AllCategoriesSummary,
            MonthlySummary,
            MonthlyAllCategories
        }

        //private const string ordersPath = @"c:\Users\Ste\Desktop\report\orders(Jan1-Mar17).xml";
        private const string ordersPath = @"orders-allTime.xml";

        private const string productsPath = @"products.csv";

        public static IEnumerable<SummaryReport> GenerateMonthlySummaryReports(DateTime from, DateTime to)
        {
            List<Product> products = CsvProductParser.ReadCsvProducts(productsPath);
            List<OrderItem> orders = XmlOrdersParser.ReadOrders(ordersPath, products).ToList();

            List<OrderItem> dateFilteredOrders = orders.Where(o => o.Product != null
                                                                   && o.Date >= from
                                                                   && o.Date <= to).ToList();
            return CreateMonthlyTotalReport(from, to, dateFilteredOrders);
        }

        public static string GenerateReport(DateTime from, DateTime to, ReportType reportType)
        {
            List<Product> products = CsvProductParser.ReadCsvProducts(productsPath);
            List<OrderItem> orders = XmlOrdersParser.ReadOrders(ordersPath, products).ToList();

            List<OrderItem> dateFilteredOrders = orders.Where(o => o.Product != null
                                                                   && o.Date >= from
                                                                   && o.Date <= to).ToList();

            if (dateFilteredOrders.Count == 0)
            {
                return "No orders found for given start and end date to generate the report.";
            }

            if (reportType == ReportType.Summary)
            {

                StringBuilder orderList;
                return string.Format("Summary report: dates {0:yyyy dd MM} - {1:yyyy dd MM}: {2}", from,
                                     to,
                                     CreateSummaryReport(dateFilteredOrders, from, to, out orderList));
            }
            if (reportType == ReportType.AllCategoriesSummary)
            {
                return string.Format("Summary report: dates {0:yyyy dd MM} - {1:yyyy dd MM}: {2}", from,
                                     to,
                                     CreateCategoryReports(products, dateFilteredOrders));

            }
            if (reportType == ReportType.MonthlySummary)
            {
                return string.Format("Monthly report: dates {0:yyyy dd MM}- {1:yyyy dd MM}: {2}", from, to,
                                     string.Join("\n --- \n" , CreateMonthlyTotalReport(from, to, orders)));
            }
            throw new NotImplementedException("Report type not supported");
        }

        private static IEnumerable<SummaryReport> CreateMonthlyTotalReport(DateTime from, DateTime to, IEnumerable<OrderItem> orders)
        {
            List<SummaryReport> result = new List<SummaryReport>(); 
            Validate(from < to);
            DateTime fromFirstInMonth = new DateTime(from.Year, from.Month, 1);
            DateTime toLastInMonth = new DateTime(to.Year, to.Month, 1).AddMonths(1);

            while (fromFirstInMonth.AddMonths(1) <= toLastInMonth)
            {
                DateTime lastInCurrentMonth = fromFirstInMonth.AddMonths(1);
                List<OrderItem> ordersInMonth = orders.Where(o => o.Product != null
                                                                       && o.Date >= fromFirstInMonth &&
                                                                       o.Date <= lastInCurrentMonth).ToList();

                StringBuilder orderList;
                SummaryReport rep = CreateSummaryReport(ordersInMonth, fromFirstInMonth, lastInCurrentMonth, out orderList);
                result.Add(rep);
                fromFirstInMonth = fromFirstInMonth.AddMonths(1);
            }

            return result;
        }

        private static SummaryReport CreateSummaryReport(IEnumerable<OrderItem> ordersWithProducts, DateTime from, DateTime to, out StringBuilder orderList) {
            
            orderList = new StringBuilder();
            int totalItems = 0, totalPayments = 0, totalShippings = 0;
            decimal totalPrice = 0, totalMargin = 0, totalShippingPrice = 0, totalPaymentPrice = 0;
            StringBuilder report = new StringBuilder();
            report.AppendLine();
            foreach (OrderItem ordersWithProduct in ordersWithProducts) {
                orderList.AppendLine(ordersWithProduct.ToString());
                if (CsvProductParser.IsPayment(ordersWithProduct.Product)){
                    Validate(ordersWithProduct.Amount == 1);
                    totalPayments++;
                    totalPaymentPrice += ordersWithProduct.UnitPrice;
                } 
                else if (CsvProductParser.IsShipping(ordersWithProduct.Product)){
                    Validate(ordersWithProduct.Amount == 1);
                    totalShippings++;
                    totalShippingPrice += ordersWithProduct.UnitPrice;
                }
                else{
                    totalItems += ordersWithProduct.Amount;
                    decimal purchasePrice = ordersWithProduct.Product.PurchasePrice;
                    decimal price = ordersWithProduct.UnitPrice;
                    totalMargin += ordersWithProduct.Amount*(price - purchasePrice);
                    totalPrice += ordersWithProduct.Amount*price;
                }
            }

            SummaryReport result = new SummaryReport(totalItems, totalPayments, totalShippings, totalPrice, totalMargin,
                                                     totalShippingPrice, totalPaymentPrice, from, to);
            return result;
        }


        private static StringBuilder CreateCategoryReports(IEnumerable<Product> products, IEnumerable<OrderItem> ordersWithProducts) {
            string[] categories = products.SelectMany(p => p.Categories).Distinct().ToArray();

            StringBuilder report = new StringBuilder();

            List<CategoryReport> categoryReports = new List<CategoryReport>();
            foreach (string category in categories) {
                List<OrderItem> ordersFromCategory = ordersWithProducts.Where(o => o.Product.Categories.Contains(category)).ToList();

                decimal totalPrice = ordersFromCategory.Sum(o => o.UnitPrice * o.Amount);
                decimal totalMargin = ordersFromCategory.Sum(o => (o.UnitPrice - o.Product.PurchasePrice) * o.Amount);

                CategoryReport catRep = new CategoryReport(category, ordersFromCategory.Count(), totalPrice, totalMargin);
                categoryReports.Add(catRep);
                report.AppendLine(catRep.ToString());
            }
            return report;
        }

        private static void Validate(bool condition)
        {
            if (!condition)
            {
                throw new Exception("Validation failed");
            }
        }
    
    }
}
