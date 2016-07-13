using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using VerticeSqlPoc.Web.Api.Models;
using VerticeSqlPoc.Web.Services;
using VerticeSqlPoc.Web.Services.Interfaces;
using VerticeSqlPoc.Web.Services.Models;
using VerticeSqlPoc.Web.Services.Models.SQL;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace VerticeSqlPoc.Web.Api.Controllers
{
    public class AverageCapitalBalanceController : ApiController
    {
        public readonly ISqlService SqlService = new SqlService(AppConfig.PrimarySql);
        //public readonly ISqlService dbService = new SqlService(AppConfig.PrimarySql);
        public readonly IRedisService RedisService = new RedisService(AppConfig.RedisConnection);
        protected readonly IAvgCapitalBalanceService avgCapitalBalanceService = new AvgCapitalBalanceService();
        protected static string COMPANYCODE_AVGCAPITALBALANCE = "AvgCapitalBalance_";
        protected static string GLITEMS_COMPANYCODES = "glItemCompanyCodes";
        protected static string COMPANYCODE_GLITEMS = "glItems_";
        protected static string ALLGLITEMS = "allGLItems";

        public CompanyInfos Get(bool useDatabase = true)
        {
            // get the company codes from the cache
            DateTime start = DateTime.Now;
            List<CompanyInfo> infos = GetCompanyCodes(useDatabase);
            System.Diagnostics.Debug.WriteLine("TIMING CODE: " + DateTime.Now.Subtract(start));

            CompanyInfos results = new CompanyInfos();
            results.AddRange(infos);
            return results;
            //return GetCompanyCodes();
        }

        public AvgCapitalBalances Get(string id)
        {
            // get the GL Item Records from the Cache
            List<TableGL> glItems = GetGLItemsFromCache(id);

            // use calculate service to calculate result passing it the list of 
            // glitems that we retrieved from the cache
            List<AvgCapitalBalance> balances = avgCapitalBalanceService.Calculate(glItems).ToList();

            AvgCapitalBalances results = new AvgCapitalBalances();
            results.AddRange(balances);
            return results;
        }

        private List<CompanyInfo> GetCompanyCodes(bool useDatabase)
        {
            if (useDatabase)
            {
                //return GetFromCache<CompanyInfo>(GLITEMS_COMPANYCODES, () => { return SqlService.GetCompanyCodes().ToList(); });
                return SqlService.GetCompanyCodes().ToList();
                //return avgCapitalBalanceService.Calculate(glItems).ToList();
            }
            else
            {
                List<CompanyInfo> companyInfos = SqlService.GetCompanyCodes().ToList();

                List<CompanyInfo> results = new List<CompanyInfo>();
                foreach (CompanyInfo ci in companyInfos)
                {
                    List<TableGL> glItems = SqlService.GetGLItems(ci.CompanyCode).ToList();
                    results.AddRange(avgCapitalBalanceService.GetCompanyCodes(glItems));
                }

                return results;

                // get the list of company codes from the cache
                //GetFromCache<CompanyInfo>(GLITEMS_COMPANYCODES, () => { return SqlService.GetCompanyCodes().ToList(); });

                // loop through company codes getting all gl items for each company code
                // then calling AvgCapitalBalance service GetCompanyCOdes on just that company code
                // build up a list of return results
                // return it

                //List<CompanyInfo> companyInfos = GetFromCache<CompanyInfo>(GLITEMS_COMPANYCODES, () => { return SqlService.GetCompanyCodes().ToList(); });
                //List<CompanyInfo> results = new List<CompanyInfo>();
                //foreach(CompanyInfo ci in companyInfos)
                //{
                //    List<TableGL> glItems = GetGLItemsFromCache(ci.CompanyCode);
                //    results.AddRange(avgCapitalBalanceService.GetCompanyCodes(glItems));
                //}

                //return results;

                //List<TableGL> glItems = GetFromCache<TableGL>(ALLGLITEMS, () => { return SqlService.GetAllGLItems().ToList(); });

                //return avgCapitalBalanceService.GetCompanyCodes(glItems).ToList();
            }
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
