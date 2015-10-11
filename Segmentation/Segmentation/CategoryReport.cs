using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Segmentation
{
    public class CategoryReport
    {
        public readonly string CategoryName;
        public readonly int ItemCount;
        public decimal TotalPrice;
        public decimal TotalMargin;
        public readonly List<OrderItem> OrderItems;
        public readonly decimal RelativeMargin;

        public CategoryReport(string categoryName, List<OrderItem> orderItems)
        {
            CategoryName = categoryName;
            ItemCount = orderItems.Count;
            OrderItems = orderItems;
            TotalPrice = orderItems.Sum(o => o.UnitPrice * o.Amount);
            TotalMargin = orderItems.Sum(o => (o.UnitPrice - o.Product.PurchasePrice)*o.Amount);
            RelativeMargin = TotalPrice == 0 ? 0 : TotalMargin/TotalPrice*100;
        }

        public override string ToString() {
            StringBuilder res = new StringBuilder();
            res.AppendLine("Category: " + CategoryName);
            res.AppendLine("Items sold: " + ItemCount);
            res.AppendLine("Total price : " + TotalPrice + " CZK");
            res.AppendLine("Total margin: " + TotalMargin + " CZK (" + string.Format("{0:0.0}",RelativeMargin) + "%)");
            res.AppendLine("=========================================");
            return res.ToString();
        }
    }
}