using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using mt_web_api.Services;

namespace mt_web_api.Controllers
{
    public class DbSalesReportsController : ApiController
    {
        private readonly DbReportingService _dRsDbReportingService = new DbReportingService();
        public string[] Get()
        {
            return _dRsDbReportingService.GetManufacturerByMonth("Adidas");
        }
    }
}
