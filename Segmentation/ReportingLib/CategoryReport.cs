using System.Text;

namespace ReportingLib
{
    public class CategoryReport : AbstractSalesReport
    {
        public readonly string CategoryName;


        public CategoryReport(string categoryName, int totalItems, decimal totalPrice, decimal totalMargin)
            : base(totalItems, totalPrice, totalMargin){
            CategoryName = categoryName;
        }

        public override string ToString() {
            StringBuilder res = new StringBuilder();
            res.AppendLine("AllCategoriesSummary: " + CategoryName);
            res.Append(base.ToString());
            return res.ToString();
        }
    }
}