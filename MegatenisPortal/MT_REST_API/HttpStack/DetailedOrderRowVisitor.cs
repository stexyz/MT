using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using MT_REST_API.Models;

namespace MT_REST_API.HttpStack {
    namespace MTReporting {
        public interface IDetailedOrderRowVisitor {
            OrdersReport GenerateReport(DateTime from, DateTime to);
            void Visit(DetailedOrderRow detailedOrderRow);
        }

        public class DetailedOrderRowVisitor : IDetailedOrderRowVisitor {
            private List<DetailedOrderRow> rows = new List<DetailedOrderRow>();
            private readonly StringBuilder errorList = new StringBuilder();
            private int orderCount;
            private decimal totalPrize;
            private decimal totalPurchase;

            public DetailedOrderRowVisitor(Dictionary<string, decimal?> purchasePrices) {
                PurchasePrices = purchasePrices;
            }

            protected Dictionary<string, decimal?> PurchasePrices { get; private set; }

            public void Visit(DetailedOrderRow detailedOrderRow) {
                rows.Add(detailedOrderRow);
                orderCount++;
                totalPrize += detailedOrderRow.ItemPrice * detailedOrderRow.Amount;
                if (!PurchasePrices.ContainsKey(detailedOrderRow.ProductCode) || !PurchasePrices[detailedOrderRow.ProductCode].HasValue) {
                    errorList.AppendFormat("For order #{0} there is no purchase price for item #{1} ({2})\n.",
                                           detailedOrderRow.OrderCode, detailedOrderRow.ProductCode,
                                           detailedOrderRow.ProductName);
                } else {
                    decimal purchasePrize = PurchasePrices[detailedOrderRow.ProductCode].Value;
                    detailedOrderRow.ItemPurchasePrize = purchasePrize;
                    totalPurchase += purchasePrize * detailedOrderRow.Amount;
                }
            }

            public OrdersReport GenerateReport(DateTime from, DateTime to) {
                return new OrdersReport(errorList.ToString(), orderCount, totalPrize, totalPurchase, rows, from, to);
            }
        }
    }

}