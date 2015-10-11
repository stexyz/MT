using System.Collections.Generic;
using System.Xml;

namespace ReportingLib
{
    public static class XmlOrdersParser
    {
        //public static IEnumerable<Product> ReadProducts(string productsPath) {
        //    List<Product> result = new List<Product>();
        //    XmlDocument doc = new XmlDocument();
        //    doc.Load(productsPath);

        //    foreach (XmlElement productElement in doc.DocumentElement.GetChildElements("PRODUCT")) {
        //        Product p = new Product { Name = productElement.GetChildElement("NAME").InnerText };

        //        XmlElement stockElement = productElement.GetChildElement("STOCK");

        //        IEnumerable<XmlElement> codeSuperElement =
        //            stockElement.GetChildElements("MASTER").Concat(stockElement.GetChildElements("VARIANT"));

        //        p.Variants = codeSuperElement.Select(e => new Variant(e.GetChildElement("CODE").InnerText,
        //                                                              e.GetChildElement("PRICELISTS")
        //                                                               .GetChildElements("PRICELIST").First()
        //                                                               .GetChildElement("PURCHASE_PRICE").InnerText))
        //                                     .ToList();

        //        XmlElement categoriesElement =
        //            productElement.ChildNodes.OfType<XmlElement>().Single(e => e.LocalName.Equals("CATEGORIES"));

        //        foreach (
        //            XmlElement categoryElement in
        //                categoriesElement.ChildNodes.OfType<XmlElement>().Where(e => e.LocalName.Equals("CATEGORY"))) {
        //            p.Categories.Add(categoryElement.InnerText);
        //        }

        //        result.Add(p);
        //    }
        //    return result;
        //}

        public static IEnumerable<OrderItem> ReadOrders(string ordersPath, IEnumerable<Product> products) {
            List<OrderItem> result = new List<OrderItem>();

            XmlDocument doc = new XmlDocument();
            doc.Load(ordersPath);

            foreach (XmlElement orderElement in doc.DocumentElement.GetChildElements("ORDER")) {
                string date = orderElement.GetChildElement("DATE").InnerText;
                string orderCode = orderElement.GetChildElement("ORDER_ID").InnerText;
                foreach (XmlElement orderItemElement in orderElement.GetChildElement("ORDER_ITEMS").GetChildElements("ITEM")) {
                    string code = orderItemElement.GetChildElement("CODE").InnerText;
                    string amount = orderItemElement.GetChildElement("AMOUNT").InnerText;
                    string name = orderItemElement.GetChildElement("NAME").InnerText;
                    string unitPrice = orderItemElement.GetChildElement("UNIT_PRICE").GetChildElement("WITH_VAT").InnerText;
                    OrderItem orderItem = new OrderItem(date, code, amount, unitPrice, products, orderCode, name);
                    result.Add(orderItem);
                }
            }
            return result;
        }

    }
}