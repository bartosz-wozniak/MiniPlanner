using System.Web.Mvc;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Jak korzystać z aplikacji?";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Skontaktuj się z adminitratorem.";

            return View();
        }
    }
}