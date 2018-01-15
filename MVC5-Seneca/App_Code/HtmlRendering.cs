using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Mvc;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.EntityModels;

namespace MVC5_Seneca.App_Code
{
    public class HtmlRendering
    {
        public static string RenderViewToString(SenecaContext context, string viewName, object model)
        {
            //if (string.IsNullOrEmpty(viewName))
            //   viewName = context.RouteData.GetRequiredString("action");

            //var viewData = new ViewDataDictionary(model);

            using (var sw = new System.IO.StringWriter())
            {
            //    var viewResult = ViewEngines.Engines.FindPartialView(context, viewName);
            //    var viewContext = new ViewContext(context, viewResult.View, viewData, new TempDataDictionary(), sw);
            //    viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
    }

}