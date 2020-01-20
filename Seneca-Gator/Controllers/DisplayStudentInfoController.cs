using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.EntityModels;
using MVC5_Seneca.ViewModels;
using Newtonsoft.Json;            

namespace MVC5_Seneca.Controllers
{
    public class DisplayStudentInfoController : Controller
    { 
        private readonly SenecaContext _db = new SenecaContext();
        public ActionResult Index()   
        {
            DisplayStudentInfoViewModel model = new DisplayStudentInfoViewModel
            {
                User_Id = User.Identity.GetUserId()
            };
            var sortedStudents = _db.Students.Where(s => s.Active).OrderBy(s => s.FirstName);       
            model.Students = sortedStudents.Select(s => new SelectListItem      
            {
                Value = s.Id.ToString(),
                Text = s.FirstName
            })
            .ToList();

            model.UpdateAllowed = "false";
            if (User.IsInRole("Administrator") || User.IsInRole("Tutor"))
            {
                model.UpdateAllowed = "true";
            }

            return View(model);
        }        
     
        [Authorize(Roles = "Active")]
        public ActionResult GetStudentDetails(int id /* drop down value */)
        {   
            Student student = (from s in _db.Students where s.Id == id select s).Single();

            student.Reports = student.Reports.OrderByDescending(r => r.DocumentDate).ToList();

           var associateTutors =  (from t in _db.AssociateTutors where t.Student.Id == id select t.Tutor.Id).ToList();   

                foreach (var record in associateTutors)
                {
                    var tutor = _db.Users.Find(record);
                    student.AssociateTutors.Add(tutor);
                } 

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
        public ActionResult ReturnToDashboard()
        {
            return RedirectToAction("Index", "Home");
        }
    }    
}