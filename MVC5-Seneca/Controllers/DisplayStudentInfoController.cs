using System;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Mvc;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.EntityModels;
using MVC5_Seneca.ViewModels;
using Newtonsoft.Json;

namespace MVC5_Seneca.Controllers
{
    public class DisplayStudentInfoController : Controller
    {
        public ActionResult ReturnToDashboard()
        {
            return RedirectToAction("Index", "Home");
        }
        private SenecaContext db = new SenecaContext();
        public ActionResult Index()

        {
            DisplayStudentInfoViewModel model = new DisplayStudentInfoViewModel();

            model.Students = db.Students.Select(s => new SelectListItem
            {
               Value = s.Id.ToString(),
                Text = s.FirstName
            })
            .ToList();
            model.Parents = db.Parents.ToList();
            //model.AllDocumentTypes = db.DocumentType.ToList();   // to look up DocumentType name
            model.Reports = db.StudentReports.Select(r => new SelectListItem
            {
                Value = r.Id.ToString(),
            })
            .ToList();
            return View(model);
        }        
     
        public ActionResult GetStudentDetails(int id /* drop down value */)
        {   
            Student student = (from s in db.Students where s.Id == id select s).Single();
            try
            {
                String json = JsonConvert.SerializeObject(student, Formatting.Indented);
                return Content(json, "application/json");
            }
            catch (Exception)
            {
                return null;
            }
        }
        public ActionResult EmailTo(int? Id)
        {
            Student student = db.Students.Find(Id);
            Parent parent = db.Parents.Find(student.Parent.Id);
            User user = db.Users.Find(@Session["userId"]);
            //return Redirect("mailto:prowny@aol.com");
            return Json(new { url = "mailto:prowny@aol.com" });
            //return null;
        }
    }    
}