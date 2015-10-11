using System.Collections.Generic;
using System.Text;

namespace ReportingLib
{
    public abstract class AbstractSalesReport : ISalesReport
    {
        public int ItemCount { get; private set; }
        public decimal TotalPrice { get; private set; }
        public decimal TotalMargin { get; private set; }
        public List<OrderItem> OrderItems { get; private set; }
        public decimal RelativeMargin { get; protected set; }

        protected AbstractSalesReport(int totalItems, decimal totalPrice, decimal totalMargin)
        {
            ItemCount = totalItems;
            TotalPrice = totalPrice;
            TotalMargin = totalMargin;
            RelativeMargin = TotalPrice == 0 ? 0 : TotalMargin / TotalPrice * 100;
        }

        public override string ToString() {
            StringBuilder res = new StringBuilder();
            res.AppendLine("Items sold: " + ItemCount);
            res.AppendLine("Total price : " + TotalPrice + " CZK");
            res.AppendLine("Total margin: " + TotalMargin + " CZK (" + string.Format("{0:0.0}", RelativeMargin) + "%)");
            res.AppendLine("=========================================");
            return res.ToString();
        }
    }
}