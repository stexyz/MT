using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using MT_REST_API.HttpStack.MTReporting;

namespace MT_REST_API.Models {
    public class DetailedOrderRow
    {
        public int DetailedOrderRowId { get; set; }
        public decimal ItemPurchasePrize{ get; set; }
        public string OrderCode{ get; set; }
        public int Amount{ get; set; }
        public string ProductCode{ get; set; }
        public string ProductName{ get; set; }
        public decimal ItemPrice{ get; set; }

        public DetailedOrderRow(){}

        public DetailedOrderRow(string orderCode, string amountString, string productCode, string productName, string itemPriceString)
        {
            DetailedOrderRowId = 0;
            OrderCode = orderCode;
            int amount;
            if (!int.TryParse(amountString, out amount)) {
                throw new ArgumentException(string.Format("Couldn parse the amount of items ({0}) in an order.", amountString));
            }
            Amount = amount;
            ProductCode = productCode;
            ProductName = productName;
            decimal itemPrice;
            if (!decimal.TryParse(itemPriceString, NumberStyles.Float, CultureInfo.InvariantCulture, out itemPrice)) {
                throw new ArgumentException(string.Format("Couldn parse the price of an item ({0}) in the order.", itemPriceString));
            }
            ItemPrice = itemPrice;
        }

        public override string ToString() {
            return string.Format("Order code [{0}], {1}x item {2} [{3}] for {4} (purch {5}).", OrderCode, Amount, ProductName,
                                 ProductCode, ItemPrice, ItemPurchasePrize);
        }

        public void Accept(IDetailedOrderRowVisitor visitor) {
            visitor.Visit(this);
        }


        //public string Self {
        //    get {
        //        return string.Format(CultureInfo.CurrentCulture,
        //            "api/DetailedOrderRows/{0}", this.OrderRowId);
        //    }
        //    set { }
        //}
    }
}