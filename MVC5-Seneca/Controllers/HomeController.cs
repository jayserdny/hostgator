using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using MVC5_Seneca.ViewModels;                        

namespace MVC5_Seneca.Controllers
{
    public class HomeController : Controller
    {
        SenecaContext db = new SenecaContext();
        // GET: Home 
        //[AllowAnonymous]
        public ActionResult Index()
        {  
            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }            
          
            if (!User.IsInRole("Active"))        
            {
                return RedirectToAction("Login", "Account", new { errorMessage = "This account is awaiting confirmation." });
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
        [Authorize(Roles = "Active")]
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
            return RedirectToAction("ResetPassword", "Account");
        }

        public ActionResult DisplayStudentInfo()
        {
            return RedirectToAction("Index","DisplayStudentInfo");
        }

        public ActionResult ResetAnyPassword()
        {
            return RedirectToAction("ResetAnyPassword", "Account");
        }

        public ActionResult AddIdentityRole()
        {
            AddIdentityRoleViewModel viewModel = new AddIdentityRoleViewModel
            {
                Name = "Routine Not Implemented"
            };
            return View(viewModel);
        }

        public ActionResult IdentityRoleSave(string name)
        {                                                                                           
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult IdentityRoleSave([Bind(Include = "Name")] AddIdentityRoleViewModel viewModel)
        {
            return RedirectToAction("Index", "Home");
        }
        public ActionResult ReturnToDashboard()
        {
            return RedirectToAction("Index", "Home");
        }
        public ActionResult LogOut()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Session.Abandon();
            Session.RemoveAll();    
            return RedirectToAction("Login", "Account");
        }
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
    }
}