using System.Collections.Generic;

namespace ReportingLib
{
    public interface ISalesReport
    {
        int ItemCount { get; }
        decimal TotalPrice { get; }
        decimal TotalMargin { get; }
        List<OrderItem> OrderItems { get; }
        decimal RelativeMargin { get; }
    }
}