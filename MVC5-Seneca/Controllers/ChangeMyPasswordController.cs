using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC5_Seneca.DataAccessLayer;
using System.Security.Cryptography;
using System.Text;

namespace MVC5_Seneca.ViewModels
{
    public class ChangeMyPasswordController : Controller
    {
        public static Boolean pageExpired;
        // GET: ChangeMyPassword/Index
        public ActionResult Index()
        {
            // return to login page after password update:
            Session.Abandon();
            Session.RemoveAll();
            Session["userId"] = null;
            return RedirectToAction("Index", "Login");
        }
        
        // GET: ChangeMyPassword/Edit
        public ActionResult Edit()
        {
            pageExpired = false;
            return View();
        }


        // POST: ChangeMyPassword
        [HttpPost]
        public ActionResult Authorize(ChangeMyPasswordViewModel changeMyPasswordViewModel)
        {
            ChangeMyPasswordViewModel CMP = changeMyPasswordViewModel;
            if (CMP.NewPassword1 != CMP.NewPassword2)
            {
                changeMyPasswordViewModel.ErrorMessage = "Passwords do not match!";
                return View("Edit", changeMyPasswordViewModel);
            }
            else
            {
                using (MVC5_Seneca.DataAccessLayer.SenecaContext db = new MVC5_Seneca.DataAccessLayer.SenecaContext())
                {
                    if (pageExpired)
                    {
                        changeMyPasswordViewModel.ErrorMessage = "Page expired.";                     
                        return View("Edit", changeMyPasswordViewModel);
                    }
                    if (Session["userId"] == null)
                    {
                        return RedirectToAction("Index", "Login");
                    }
                    String userName = Session["userName"].ToString();
                    var user = db.Users.Where(x => x.UserName == userName).FirstOrDefault();
                    //user.PasswordHash = App_Code.EncryptSHA256.EncodeSHA256(CMP.NewPassword1,user.PasswordSalt);
                 
                    db.SaveChanges();
              
                    changeMyPasswordViewModel.ErrorMessage = "PASSWORD SUCCESFULLY CHANGED.";
                    pageExpired = true;
                    return View("Edit", changeMyPasswordViewModel);
                }                
            }            
        }

        public ActionResult ReturnToDashboard()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
