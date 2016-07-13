using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using VerticeSqlPoc.Web.Api.Models;
using VerticeSqlPoc.Web.Services;
using VerticeSqlPoc.Web.Services.Interfaces;
using VerticeSqlPoc.Web.Services.Models;
using VerticeSqlPoc.Web.Services.Models.SQL;
using System.Web.OData;

namespace VerticeSqlPoc.Web.Api.Controllers
{
    public class TableGLODController : ODataController
    {
        public readonly ISqlService SqlService = new SqlService(AppConfig.PrimarySql);
        //public readonly ISqlService dbService = new SqlService(AppConfig.PrimarySql);
        public readonly IRedisService RedisService = new RedisService(AppConfig.RedisConnection);
        protected readonly IAvgCapitalBalanceService avgCapitalBalanceService = new AvgCapitalBalanceService();
        protected static string COMPANYCODE_AVGCAPITALBALANCE = "AvgCapitalBalance_";
        protected static string GLITEMS_COMPANYCODES = "glItemCompanyCodes";
        protected static string COMPANYCODE_GLITEMS = "glItems_";

        public List<CompanyInfo> Get()
        {
            // get the company codes from the cache
            return GetCompanyCodes();
        }

        public IEnumerable<AvgCapitalBalance> Get(string id)
        {
            // get the GL Item Records from the Cache
            List<TableGL> glItems = GetGLItemsFromCache(id);

            // use calculate service to calculate result passing it the list of 
            // glitems that we retrieved from the cache
            return avgCapitalBalanceService.Calculate(glItems);
        }

        private List<CompanyInfo> GetCompanyCodes()
        {
            return GetFromCache<CompanyInfo>(GLITEMS_COMPANYCODES, () => { return SqlService.GetCompanyCodes().ToList(); });
        }

        private GLItemsForCompany GetGLItems(string companyCode)
        {
            GLItemsForCompany vm = new GLItemsForCompany();
            vm.CompanyCode = companyCode;
            vm.GLItems = GetGLItemsFromCache(companyCode);
            return vm;
        }

        private List<TableGL> GetGLItemsFromCache(string companyCode)
        {
            return GetFromCache<TableGL>(buildGLItemsCacheKey(companyCode), () => SqlService.GetGLItems(companyCode).ToList());
        }

        private List<T> GetFromCache<T>(string cacheKey, Func<List<T>> fillDataFunc)
            where T : class
        {
            ClientResponse Response = RedisService.GetList<T>(cacheKey);
            List<T> items = Response.Payload.Contains("KeyValue") ? (List<T>)Response.Payload["KeyValue"] : null;
            if (items == null)
            {
                items = fillDataFunc();
                RedisService.InsertList<T>(cacheKey, items);
            }

            return items;
        }

        private string buildGLItemsCacheKey(string companyCode)
        {
            return COMPANYCODE_GLITEMS + companyCode;
        }
    }
}