using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VerticeSqlPoc.Web.Services.Interfaces;
using VerticeSqlPoc.Web.Services.Models;
using VerticeSqlPoc.Web.Services.Models.SQL;
using VerticeSqlPoc.Web.Services;
using System.Data.SqlClient;
using VerticeSqlPoc.Web.Api.Models;

namespace VerticeSqlPoc.Web.Api.Controllers
{
    public class StandardDeviationController : ApiController
    {
        public readonly ISqlService SqlService = new SqlService(AppConfig.PrimarySql);
        protected readonly IStandardDeviationService stdDeviationService = new StandardDeviationService();
        protected static string COMPANYCODE_AVGCAPITALBALANCE = "AvgCapitalBalance_";
        protected static string GLITEMS_COMPANYCODES = "glItemCompanyCodes";
        protected static string COMPANYCODE_GLITEMS = "glItems_";
        protected static string ALLGLITEMS = "allGLItems";

        public StandardDeviations Get()
        {
            // Get the Nav records from the database
      
            // retrieve the data and pass to service for calculation
            IEnumerable<CompanyNavInfo> navValues = SqlService.GetAllNav();

            //*******************
            // Get the standard deviation for each company
            //*******************
            //    // Call the StdDeviation service to obtain std deviation
            IEnumerable<StandardDeviation> stdDevs = stdDeviationService.CalculateByCompany(navValues);

            //    // convert to strongly typed collection
            StandardDeviations results = new StandardDeviations();
            results.AddRange(stdDevs);
            return results;
            //*******************

            //*******************
            // Get the standard deviation for all companies
            //*******************
            //double stdDev = stdDeviationService.Calculate(navValues.Select(o => o.Nav));
            //StandardDeviations results = new StandardDeviations();
            //results.Add(new StandardDeviation() { CompanyCode = "ALL", StdDev = stdDev });
            //return results;
            //*******************
        }
    }
}
