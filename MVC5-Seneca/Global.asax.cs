using MVC5_Seneca.DataAccessLayer;
using System;
using System.Configuration;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MVC5_Seneca
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {   
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Session_Start(Object sender, EventArgs e)
        {
            DateTime bdt = DateTime.Today.AddDays(-(int) DateTime.Today.DayOfWeek);  // Beginning o Week
            string sdt = bdt.ToString("MM/dd/yyyy");
            HttpContext.Current.Session.Add("StartDate", sdt);
            string edt = bdt.AddDays(7).ToString("MM/dd/yyyy");
            HttpContext.Current.Session.Add("EndDate", edt);
        }

        protected void Application_BeginRequest()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
        }
    }
}
