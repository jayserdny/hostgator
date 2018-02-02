using System;
using System.Collections.Generic;     
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.EntityModels;
using MVC5_Seneca.Models;
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
            DisplayStudentInfoViewModel model = new DisplayStudentInfoViewModel
            {
                User_Id = User.Identity.GetUserId()
            };
            var sortedStudents = db.Students.OrderBy(s => s.FirstName);       
            model.Students = sortedStudents.Select(s => new SelectListItem      
            {
                Value = s.Id.ToString(),
                Text = s.FirstName
            })
            .ToList();                                                                                     
            //model.Parents = db.Parents.ToList();
            //var reports = (from r in db.StudentReports orderby r.DocumentDate descending select r).ToList();           
            //foreach (StudentReport report in reports)
            //{
            //    model.Reports.Add(new SelectListItem
            //    {
            //        Value = report.Id.ToString(),
            //        Text = report.Comments
            //    });               
            //}

            //model.Reports = db.StudentReports.OrderByDescending( r => r.DocumentDate).Select(r => new SelectListItem
            //{
            //    Value = r.Id.ToString(),
            //})
            //.ToList();
           return View(model);
        }        
     
        [Authorize(Roles = "Active")]
        public ActionResult GetStudentDetails(int id /* drop down value */)
        {   
            Student student = (from s in db.Students where s.Id == id select s).Single();    
            //student.Reports.Clear();                   
            //var reports = (from r in db.StudentReports.Where(r => r.Student.Id == id) orderby r.DocumentDate descending select r).ToList();           
            //foreach (StudentReport report in reports)
            //{
            //    student.Reports.Add(report);               
            //}
  
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
    }    
}