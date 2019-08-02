using System.Web.Mvc;

namespace MVC5_Seneca.Controllers
{
    public class SHEP_HFEDController : Controller
    {
        // GET: SHEP_HFED
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SHEP()
        {
            Session["FromSHEP_HFEDMenu"] = "true";
            return RedirectToAction("Index", "Home");
        }

        public ActionResult HFED()
        {
            return RedirectToAction("Index", "HfedHome");
        }
    }
}