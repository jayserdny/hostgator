using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using MVC5_Seneca.ViewModels;

namespace MVC5_Seneca.Controllers
{
    public class HFEDHomeController : Controller
    {
        // GET: HFEDHome
        public ActionResult Index()
        {
            return View();
        }
    }
}