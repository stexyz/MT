using System;
using System.Collections.Generic;
using MT_REST_API.HttpStack;
using MT_REST_API.HttpStack.MTReporting;

namespace MT_REST_API.Models {
    public class MegatenisSiteCsvContext {
        public static OrdersReport GetDetailedOrderRows(ReportCategory reportCategory, DateTime from, DateTime to)
        {
            string csv = HttpConnector.GetOrderListCsv(from, to, reportCategory);
            IEnumerable<DetailedOrderRow> rows = CsvParser.ParseCsv(csv);
            string purchPrizes = HttpConnector.GetPurchPriceCsv();
            Dictionary<string, decimal?> purchDict = CsvParser.ParsePurchasePrices(purchPrizes);
            IDetailedOrderRowVisitor visitor = new DetailedOrderRowVisitor(purchDict);

            foreach (DetailedOrderRow detailedOrderRow in rows)
            {
                detailedOrderRow.Accept(visitor);
            }
            OrdersReport ordersReport = visitor.GenerateReport(from, to);
            return ordersReport;
        }
    }
}