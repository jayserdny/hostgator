using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.EntityModels;
using MVC5_Seneca.ViewModels;
using Newtonsoft.Json;

namespace MVC5_Seneca
{
    public class ResetAnyPasswordController : Controller
    {  
        private SenecaContext db = new SenecaContext();

        public ActionResult Index()
        {
            ResetAnyPasswordViewModel model = new ResetAnyPasswordViewModel();
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Value = "0", Text = "-Select User-" });
            var users = (from u in db.Users select u).ToList();
            foreach (var item in users)
            {
                items.Add(new SelectListItem { Value = item.Id.ToString(), Text = item.UserName + ": " + item.FirstName + " " + item.LastName }); 
            }
            model.Users = items;
            return View(model);
        }             
                                                                                     
        //public ActionResult Reset(String userName)
        //{
            //User user = (from u in db.Users where u.UserName == userName select u).Single();
            //using (MVC5_Seneca.DataAccessLayer.SenecaContext db = new MVC5_Seneca.DataAccessLayer.SenecaContext())
            //    //if (_viewModel.NewPassword == null)
            //    if (Id == null)
            //    {   
            //        return View(user);
            //    }
            //var user = db.Users.Where(x => x.Id == _viewModel.User.Id).FirstOrDefault();
            //user.PasswordHash = App_Code.EncryptSHA256.EncodeSHA256(rap.NewPassword, user.PasswordSalt);
                                                                                                                                        
        //    try
        //    {
        //        //String json = JsonConvert.SerializeObject(user, Formatting.Indented);
        //        //return Content(json, "application/json");
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}       

        //public ActionResult GetUserInfo(int? Id)
        //{
        //    UserDetail user = (from u in db.Users where u.Id == Id select u).Single();
        //    try
        //    {
        //        String json = JsonConvert.SerializeObject(user, Formatting.Indented);
        //        return Content(json, "application/json");
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}
        public ActionResult ReturnToDashboard()
        {
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
