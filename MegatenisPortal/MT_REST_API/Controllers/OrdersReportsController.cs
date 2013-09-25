using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MT_REST_API.HttpStack;
using MT_REST_API.Models;

namespace MT_REST_API.Controllers
{
    public class OrdersReportsController : ApiController
    {
        // GET api/ordersreports/5
        public OrdersReport Get(ReportCategory reportCategory, DateTime from, DateTime to)
        {
            return MegatenisSiteCsvContext.GetDetailedOrderRows(reportCategory,from, to); 
        }
    }
}
