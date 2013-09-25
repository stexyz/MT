using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MT_REST_API.Models
{
    public class OrdersReport
    {
        public string Summary { get {return ToString(); } }
        public DateTime From { get; private set; }
        public DateTime To { get; private set; }
        public decimal TotalPrize { get; private set; }
        public decimal TotalPurchase { get; private set; }
        public decimal TotalMargin {
            get { return TotalPrize - TotalPurchase; }
        }
        public string ErrorList { get; private set; }
        public int OrderCount { get; private set; }
        public List<DetailedOrderRow> Rows { get; set; }


        public OrdersReport(string errorList, int orderCount, decimal totalPrize, decimal totalPurchase,
                            List<DetailedOrderRow> rows, DateTime from, DateTime to)
        {
            Rows = rows;
            ErrorList = errorList;
            OrderCount = orderCount;
            TotalPrize = totalPrize;
            TotalPurchase = totalPurchase;
            From = from;
            To = to;
        }

        public override string ToString()
        {
            return
                string.Format("Reporting: sold {0} items for {1} CZK with {2} CZK margin.\n Errors encountered: {3}",
                              OrderCount, TotalPrize, TotalMargin, ErrorList);
        }

        public string GetCSV()
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine("ORDERS REPORT:; TOTAL ITEMS SOLD; TOTAL PRICE; TOTAL MARGIN");
            result.AppendFormat(";{0};{1};{2}\n", OrderCount, TotalPrize, TotalMargin);

            result.AppendFormat("FROM:;{0};TO:;{1}", From, To);


            result.AppendLine(";ERRORS");
            result.AppendLine(ErrorList);

            result.AppendLine(
                "ORDERS CODE; PRODUCT CODE; AMOUNT; PRODUCT NAME; ITEM PRICE; ITEM PURCHASE PRICE; MARGIN; TOTAL MARGIN");

            foreach (DetailedOrderRow detailedOrderRow in Rows)
            {
                result.AppendFormat("{0};{1};{2};{3};{4};{5};{6};{7}\n",
                                    detailedOrderRow.OrderCode,
                                    detailedOrderRow.ProductCode,
                                    detailedOrderRow.Amount,
                                    detailedOrderRow.ProductName,
                                    detailedOrderRow.ItemPrice,
                                    detailedOrderRow.ItemPurchasePrize,
                                    detailedOrderRow.ItemPrice - detailedOrderRow.ItemPurchasePrize,
                                    detailedOrderRow.Amount*
                                    (detailedOrderRow.ItemPrice - detailedOrderRow.ItemPurchasePrize));
            }
            return result.ToString();

        }
    }
}
