﻿using System.Web.Mvc;

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
    }
}