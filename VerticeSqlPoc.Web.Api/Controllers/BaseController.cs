using System.Web.Mvc;
using VerticeSqlPoc.Web.Api.Models;
using VerticeSqlPoc.Web.Services;
using VerticeSqlPoc.Web.Services.Interfaces;

namespace VerticeSqlPoc.Web.Api.Controllers
{
    public class BaseController : Controller
    {

        public readonly ISqlService SqlService = new SqlService(AppConfig.PrimarySql);
        //public readonly ISqlService dbService = new SqlService(AppConfig.PrimarySql);
        public readonly IRedisService RedisService = new RedisService(AppConfig.RedisConnection);
        protected readonly IAvgCapitalBalanceService avgCapitalBalanceService = new AvgCapitalBalanceService();
        protected static string COMPANYCODE_AVGCAPITALBALANCE = "AvgCapitalBalance_";
        protected static string GLITEMS_COMPANYCODES = "glItemCompanyCodes";
        protected static string COMPANYCODE_GLITEMS = "glItems_";
        protected static string GLITEMS_AVGCAPITALBALANCE = "glItemsAvgCapitalBalance";
    }
}