using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Segmentation
{
    public class OrderItem
    {
        public DateTime Date;
        public string Code;
        public int Amount;
        public Decimal UnitPrice;
        public Product Product;
        public string Name;
        public static StringBuilder failures = new StringBuilder();
        public static StringBuilder noCode = new StringBuilder();
        public static StringBuilder gotCode = new StringBuilder();
        public static StringBuilder noCodeOrder = new StringBuilder();
        public static StringBuilder postAndPayment = new StringBuilder();

        public override string ToString()
        {
            return string.Format("Date:{0}, ItemId:{1}, ItemName{2}, Amount:{3}, UnitPrice:{4}.", Date.Date, Code, Name,
                                 Amount, UnitPrice);
        }

        public OrderItem(string date, string code, string amount, string unitPrice, IEnumerable<Product> products,
                         string orderCode, string name)
        {
            Name = name;
            Date = DateTime.Parse(date);
            Code = code.ToUpper();
            Amount = int.Parse(amount);
            UnitPrice = Decimal.Parse(unitPrice);
            try
            {
                //Product = products.SingleOrDefault(p => p.Variants.Any(v => v.Id.StartsWith(Code)));
                if (Code.Equals(string.Empty))
                {
                    Product = products.FirstOrDefault(p => p.Name.Equals(Name));
                    postAndPayment.AppendLine(name + ", " + code);
                }
                else
                {
                    Product = products.FirstOrDefault(p => p.Code.StartsWith(Code));
                }
                if (Product == null)
                {
                    noCodeOrder.AppendLine(orderCode);
                    noCode.AppendLine(code);
                }
                else
                {
                    gotCode.AppendLine(code);
                }

            }
            catch (Exception e)
            {
                failures.AppendLine(code);
                Product = null;
            }
        }
    }
}