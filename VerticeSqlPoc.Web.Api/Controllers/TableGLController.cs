using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VerticeSqlPoc.Web.Services.Models;
using VerticeSqlPoc.Web.Services.Models.SQL;
using VerticeSqlPoc.Web.Api.Models;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using System.Xml;
using System.Xml.Serialization;


namespace VerticeSqlPoc.Web.Api.Controllers
{
    public class TableGLController : BaseController
    {
        // GET: TableGL
        public ActionResult Index()
        {
            // get the company codes from the cache
            List<CompanyInfo> companyCodes = GetCompanyCodes();
            //var json = JsonConvert.SerializeObject(companyCodes);

            ////XmlDocument xdoc = JsonConvert.DeserializeXmlNode(“{\”root\”:” +JsonString + “}”, “root”);
            //XmlDocument doc = JsonConvert.DeserializeXmlNode(json);
            //var xmlString = doc.ToString();
            //return this.Content(xmlString, "text/xml");
            // display the view
            return View(companyCodes);
        }

        public ActionResult AvgCapitalBalance(string id)
        {
            // get the GL Item Records from the Cache
            List<TableGL> glItems = GetGLItemsFromCache(id);

            // use calculate service to calculate result passing it the list of 
            // glitems that we retrieved from the cache
            IEnumerable<AvgCapitalBalance> calcResult = avgCapitalBalanceService.Calculate(glItems);

            // create viewmodel
            AvgCapitalBalanceForCompany vm = new AvgCapitalBalanceForCompany();
            vm.CompanyCode = id;
            vm.Balances = calcResult;

            // display view
            return View(vm);
        }

        public ActionResult GLItems(string id)
        {
            return View(GetGLItems(id));
        }

        public ActionResult GLItemsWithDate(string id, string start, string end)
        {
            return View(GetGLItems(id));
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
            if (Response.Payload.Contains("ErrorMessage"))
            {
                ViewBag.Msg += "ERROR!!" + Response.Payload["ErrorMessage"];
                return new List<T>();
            }
            List<T> items = Response.Payload.Contains("KeyValue") ? (List<T>) Response.Payload["KeyValue"] : null;
            if (items != null)
            {
                ViewBag.Msg += "List read from cache. ";
            }
            else
            {
                ViewBag.Msg += "Items List cache miss. ";
                items = fillDataFunc();
                ViewBag.Msg += "Storing results to cache. ";
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