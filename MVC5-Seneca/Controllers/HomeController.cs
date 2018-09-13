using System.IO;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using MVC5_Seneca.ViewModels;

namespace MVC5_Seneca.Controllers
{
    public class HomeController : Controller
    {                                                                          
        // GET: Home           
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
        public ActionResult MaintainAssociateTutors()
        {
            return RedirectToAction("Index", "AssociateTutors");
        }

        public ActionResult MaintainParents()
        {
            return RedirectToAction("Index", "Parents");
        }       
      
        public ActionResult MaintainDocumentTypes()
        {
            return RedirectToAction("Index", "DocumentTypes");
        }
        public ActionResult MaintainStudentReports()
        {
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
        public ActionResult MaintainTipsCategories()
        {
            return RedirectToAction("Index", "TipsCategories");
        }  
        public ActionResult UploadTeachingTips()
        {
            return RedirectToAction("Index", "TipDocuments");
        }

        //public ActionResult MaintainLocations()
        //{
        //    return RedirectToAction("Index", "Locations");
        //}
        public ActionResult MaintainTutorNotes() 
        {
            return RedirectToAction("Index", "TutorNotes");
        }
        public ActionResult Contacts()
        {
            return RedirectToAction("Index", "Contacts");
        }
        public ActionResult DisplayProfileReports()
        {
            return RedirectToAction("Index", "StudentReports");
        } 
        public ActionResult UpdateMyProfile()
        {
            return RedirectToAction("Edit", "UpdateMyProfile");
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
        public ActionResult LoginLog()
        {
            return RedirectToAction("Index", "Logins");
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
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        public ActionResult Manual()
        {
            // Should open in the browser:
            string filePath = "~/AdministratorManual/Administrator Manual.pdf";
            return File(filePath, "application/pdf");
        }

    }
}