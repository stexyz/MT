using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mt_web_api.Services {
    public class DbReportingService : SqlService
    {
        private string manufacturer_month_sales_query = @"
            -- Sales by manufacturer and month
            set language czech
            select 
	            manufacturer, 
	            left(date, 7),
	            sum(itemTotalPriceWithVat) as 'total sold',
	            sum(purchasePrice * itemAmount) as 'total purchase',
	            sum(itemTotalPriceWithVat - purchasePrice * itemAmount) as 'total margin',
	            format(sum(itemTotalPriceWithVat - purchasePrice * itemAmount)/sum(itemTotalPriceWithVat), 'P') as 'relative margin'
            from dbo.products as p join dbo.orders as o on o.itemCode = p.code
            where stock < 900 and manufacturer = 'Wilson' and itemAmount > 0 and purchasePrice > 0 
            group by manufacturer, left(date, 7)";

        public string[] GetManufacturerByMonth(string manufacturer)
        {
            return new[] {""};
        }
    }
}