using System.Configuration;

namespace VerticeSqlPoc.Web.Api.Models
{
    public class AppConfig
    {
        public const string ContentTypeJson = "application/json";

        public static string PrimarySql
        {
            get { return ConfigurationManager.ConnectionStrings["PrimarySql"].ConnectionString; }
        }

        public static string RedisConnection
        {
            get { return ConfigurationManager.AppSettings["RedisConnectionString"]; }
        }
    }
}
