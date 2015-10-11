using System;
using System.Collections.Generic;
using System.Text;

namespace ReportingLib
{
    public class SummaryReport : AbstractSalesReport
    {
        public DateTime From { get; private set; }
        public DateTime To { get; private set; }
        public int ShippingCount { get; private set; }
        public int PaymentCount { get; private set; }
        public decimal TotalShippingPrice { get; private set; }
        public decimal TotalPaymentPrice { get; private set; }

        public SummaryReport(int totalItems, int totalPayments, int totalShippings, decimal totalPrice, decimal totalMargin, decimal totalShippingPrice, decimal totalPaymentPrice, DateTime from, DateTime to) 
        :base(totalItems, totalPrice, totalMargin)
        {
            From = from;
            To = to;
            PaymentCount = totalPayments;
            ShippingCount = totalShippings;
            TotalShippingPrice = totalShippingPrice;
            TotalPaymentPrice = totalPaymentPrice;
        }

        public override string ToString() {
            StringBuilder report = new StringBuilder();
            report.AppendLine();
            report.AppendLine("Total items: " + ItemCount);
            report.AppendLine(string.Format("Total price: {0:C}", TotalPrice));
            report.AppendLine(string.Format("Total margin: {0:C} ({1:0.00}%).", TotalMargin, RelativeMargin));
            report.AppendLine(string.Format("Shipping: {0}x, total price: {1:C}; Payments {2}x, total price: {3:C}.", ShippingCount, TotalShippingPrice, PaymentCount, TotalPaymentPrice));
            return report.ToString();
        }

    }
}