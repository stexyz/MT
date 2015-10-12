using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using mt_web_api.Services;

namespace mt_web_api.Controllers {
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
        public string Get() {
            return _dbImporterService.DbImportRunning()? "Import running" : "Import not running";
        }

        // POST api/DbImports
        /// <summary>
        /// If no db import is currently running, cause a new one to run
        /// </summary>
        public string Post()
        {
            _dbImporterService.ImportDataFromShoppingCardToDb();
            return "Db Import created.";
        }
    }
}