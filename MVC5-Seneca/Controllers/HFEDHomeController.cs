using System;
using Castle.Core.Internal;
using System.Web.Mvc;

namespace MVC5_Seneca.Controllers
{
    public class HfedHomeController : Controller
    {
        // GET: HfedHome
        public ActionResult Index()
        {
            if (Session["StartDate"].ToString().IsNullOrEmpty())
            {
                Session["StartDate"] = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek).ToString("MM/dd/yyyy");
            }

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

        public ActionResult MaintainSchedules()
        {
            return RedirectToAction("Index", "HfedSchedules");
        }

        public ActionResult UpdateMyProfile()
        {
            return RedirectToAction("Edit", "HfedUpdateMyProfile");
        }

        public ActionResult DriverSignUp()
        {
            return RedirectToAction("DriverSignUp", "HfedSchedules");
        }
    }
}