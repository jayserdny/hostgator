using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MVC5_Seneca.App_Start;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.Models;
using MVC5_Seneca.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace MVC5_Seneca.Controllers
{  
    public class LoginController : Controller
    {
        private SenecaContext db = new SenecaContext();

        public object AuthManager { get; private set; }

        // GET: Login
        public ActionResult Index()
        {   
            FormsAuthentication.SignOut();
            return View();
        }   
    }
}
