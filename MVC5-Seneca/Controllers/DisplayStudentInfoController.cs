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
            var sortedStudents = _db.Students.OrderBy(s => s.FirstName);       
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
            if (student.Parent != null)
            {
                var parent = (from p in _db.Parents where p.Id == student.Parent.Id select p).Single();
            }

            student.Reports = student.Reports.OrderByDescending(r => r.DocumentDate).ToList();

            foreach (var record in _db.AssociateTutors)
            {            
                var sqlString = "";
                using (var context = new SenecaContext())
                {
                    sqlString = "SELECT Student_Id FROM AssociateTutor WHERE Id = " + record.Id;
                        var studentId = context.Database.SqlQuery<int>(sqlString).FirstOrDefault();     
                    if (studentId == id)
                    {
                        sqlString = "SELECT Tutor_Id FROM AssociateTutor WHERE Id = " + record.Id;
                        var tutorId = context.Database.SqlQuery<string>(sqlString).FirstOrDefault();
                        var tutor = _db.Users.Find(tutorId);  
                        student.AssociateTutors.Add(tutor);
                    }
                }
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