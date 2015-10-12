using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using mt_web_api.Services;

namespace mt_web_api.Controllers
{
    public class DbImportsController : ApiController
    {
        private readonly DbImporterService _dbImporterService;

        public DbImportsController()
        {
            _dbImporterService = new DbImporterService();
        }

        // GET api/DbImports
        /// <summary>
        /// Gets the list of currently running db imports (count should be atmost 1)
        /// </summary>
        /// <returns></returns>
        public string Get()
        {
            try
            {
                return _dbImporterService.DbImportRunning()
                           ? "Import running"
                           : "Import not running";
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.TraceError("Error: {0}", e.Message);
                throw;
            }
        }

        // POST api/DbImports
        /// <summary>
        /// If no db import is currently running, cause a new one to run
        /// </summary>
        public string Post()
        {
            try
            {
            new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    /* run your code here */
                    _dbImporterService.ImportDataFromShoppingCardToDb();
                }).Start();
                return "Db Import created.";
            } catch (Exception e) {
                System.Diagnostics.Trace.TraceError("Error: {0}", e.Message);
                throw;
            }
        }
    }
}