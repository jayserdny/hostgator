using System;
using System.Web.Mvc;
using MVC5_Seneca.ViewModels;

namespace MVC5_Seneca.Controllers
{
    public class ChangeMyPasswordController : Controller
    {
        public static Boolean PageExpired;
        // GET: ChangeMyPassword/Index
        public ActionResult Index()
        {
            // return to login page after password update:
            Session.Abandon();
            Session.RemoveAll();    
            return RedirectToAction("Index", "Login");
        }
        
        // GET: ChangeMyPassword/Edit
        public ActionResult Edit()
        {
            PageExpired = false;
            return View();
        }


        // POST: ChangeMyPassword
        [HttpPost]
        public ActionResult Authorize(ChangeMyPasswordViewModel changeMyPasswordViewModel)
        {
            ChangeMyPasswordViewModel cmp = changeMyPasswordViewModel;
            if (cmp.NewPassword1 != cmp.NewPassword2)
            {
                changeMyPasswordViewModel.ErrorMessage = "Passwords do not match!";
                return View("Edit", changeMyPasswordViewModel);
            }

            using (DataAccessLayer.SenecaContext db = new DataAccessLayer.SenecaContext())
            {
                if (PageExpired)
                {
                    changeMyPasswordViewModel.ErrorMessage = "Page expired.";                     
                    return View("Edit", changeMyPasswordViewModel);
                }
                if (Session["userId"] == null)
                {
                    return RedirectToAction("Index", "Login");
                }                                                               
                 
                db.SaveChanges();
              
                changeMyPasswordViewModel.ErrorMessage = "PASSWORD SUCCESFULLY CHANGED.";
                PageExpired = true;
                return View("Edit", changeMyPasswordViewModel);
            }
        }

        public ActionResult ReturnToDashboard()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
