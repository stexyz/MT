﻿using System;
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
            System.Diagnostics.Trace.TraceInformation("Information from GET.");
            System.Diagnostics.Trace.TraceWarning("Warning from GET.");
            System.Diagnostics.Trace.TraceError("Error from GET");
            return _dbImporterService.DbImportRunning() ? "Import running" : "Import not running";
        }

        // POST api/DbImports
        /// <summary>
        /// If no db import is currently running, cause a new one to run
        /// </summary>
        public string Post()
        {
            new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    /* run your code here */
                    _dbImporterService.ImportDataFromShoppingCardToDb();
                }).Start();
            return "Db Import created.";
        }
    }
}