using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace MVC5_Seneca
{
    public class MvcApplication : HttpApplication
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
            DateTime bdt = new DateTime(DateTime.Today.Year ,DateTime .Today .Month ,1);  // (initialize variable)
            if (DateTime.Today.Day > 15) // After mid-month, start with next month: 
            {
                var dt = DateTime.Today;
                bdt =new DateTime( dt.AddMonths(1) .Year, dt.AddMonths(1).Month,1) ;
            }

            string sdt = bdt.ToString("MM/dd/yyyy");
            HttpContext.Current.Session.Add("StartDate", sdt);          
            string edt = new DateTime( bdt.Year , bdt.Month , day: DateTime.DaysInMonth(bdt.Year ,bdt.Month)).ToString("MM/dd/yyyy");
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
