using System.Collections.Generic;

namespace ReportingLib
{
    public class Product
    {
        public readonly string Code;
        public readonly List<string> Categories = new List<string>();
        public readonly string Name;
        public readonly decimal PurchasePrice;

        public Product(string code, string name, string cat1, string cat2, string cat3, string cat4,
                       string purchasePrice)
        {
            Code = code;
            Name = name;
            if (!cat1.Equals(string.Empty))
            {
                Categories.Add(cat1);
            }
            if (!cat2.Equals(string.Empty))
            {
                Categories.Add(cat2);
            }
            if (!cat3.Equals(string.Empty))
            {
                Categories.Add(cat3);
            }
            if (!cat4.Equals(string.Empty))
            {
                Categories.Add(cat4);
            }
            PurchasePrice = purchasePrice == "" ? 0 : decimal.Parse(purchasePrice);
        }


        public override string ToString()
        {
            return string.Format("Code:{0},  Name: {1}, Purchase UnitPrice: {2}, Cats: {3}.", Code, Name, PurchasePrice, string.Join(",", Categories));
        }
    }
}