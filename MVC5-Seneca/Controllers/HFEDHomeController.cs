using System.Web.Mvc;

namespace MVC5_Seneca.Controllers
{
    public class HfedHomeController : Controller
    {
        // GET: HfedHome
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MaintainLocations()
        {
            return RedirectToAction("Index", "HfedLocations");
        }

        public ActionResult MaintainClients()
        {
            return RedirectToAction("Index", "HfedClients");
        }

        public ActionResult MaintainStaff()
        {
            return RedirectToAction("Index", "HfedStaffs");
        }

        public ActionResult MaintainDrivers()
        {
            return RedirectToAction("Index", "HfedDrivers");
        }

        public ActionResult MaintainProviders()
        {
            return RedirectToAction("Index", "HfedProviders");
        }
    }
}