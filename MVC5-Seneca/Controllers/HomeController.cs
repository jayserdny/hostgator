using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC5_Seneca.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home 
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (Session["userId"] == null)
            {
                return RedirectToAction("Index", "Login");  
            }
            return View();
        }
        public ActionResult MaintainUsers()
        {
            return RedirectToAction("Index", "Users");
        }
        public ActionResult MaintainStudents()
        {
            return RedirectToAction("Index", "Students");
        }
        public ActionResult MaintainParents()
        {
            return RedirectToAction("Index", "Parents");
        }
        
        public ActionResult MaintainStaff()
        {
            return RedirectToAction("Index", "Staffs");
        }
        public ActionResult MaintainDocumentTypes()
        {
            return RedirectToAction("Index", "DocumentTypes");
        }
        public ActionResult MaintainStudentReports()
        {;
            return RedirectToAction("Index", "StudentReports");
        }

        public ActionResult MaintainSchools()
        {
            return RedirectToAction("Index", "Schools");
        }  
        public ActionResult UploadStudentReports()
        {
            return RedirectToAction("Index", "Upload");
        }

        public ActionResult MaintainUserRoles() 
        {
            return RedirectToAction("Index", "UserRoles");
        }

        //public ActionResult MaintainLocations()
        //{
        //    return RedirectToAction("Index", "Locations");
        //}
        public ActionResult MaintainTutorNotes() 
        {
            return RedirectToAction("Index", "TutorNotes");
        }
        public ActionResult DisplayProfileReports()
        {
            return RedirectToAction("Index", "StudentReports");
        }

        public ActionResult ChangeMyPassword()
        {
            return RedirectToAction("Edit", "ChangeMyPassword");
        }

        public ActionResult DisplayStudentInfo()
        {
            return RedirectToAction("Index","DisplayStudentInfo");
        }

        public ActionResult ResetAnyPassword()
        {
            return RedirectToAction("Index", "ResetAnyPassword");
        }
        public ActionResult LogOut()
        {
            Session.Abandon();
            Session.RemoveAll();
            Session["userId"] = null;
            return RedirectToAction("Index", "Login");
        }
    }
}