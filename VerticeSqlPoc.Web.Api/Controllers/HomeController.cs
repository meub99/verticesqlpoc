using System.Web.Mvc;
using VerticeSqlPoc.Web.Services.Models.SQL;

namespace VerticeSqlPoc.Web.Api.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var tgl = new TableGL();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}