using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.EntityModels;
using MVC5_Seneca.Models;
using Newtonsoft.Json;

namespace MVC5_Seneca.Controllers
{
    public class ResetAnyPasswordController : Controller
    {  
        private SenecaContext db = new SenecaContext();

        public ActionResult Index()
        {
            ResetAnyPasswordViewModel model = new ResetAnyPasswordViewModel();
            List<SelectListItem> items = new List<SelectListItem>
            {
                new SelectListItem { Value = "0", Text = @"-Select User-" }
            };
            var users = (from u in db.Users select u).ToList();
            foreach (var item in users)
            {
                items.Add(new SelectListItem { Value = item.Id, Text = item.UserName + @": " + item.FirstName + @" " + item.LastName }); 
            }
            //model.Users = items;
            return View(model);
        }

        public async Task< ActionResult> Reset(String userId, String newPassword)       
        {                                                                                                                                     
            UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(db);
            UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(store);     
            String hashedNewPassword = UserManager.PasswordHasher.HashPassword(newPassword);
            ApplicationUser cUser = await store.FindByIdAsync(userId);
            await store.SetPasswordHashAsync(cUser, hashedNewPassword);
            await store.UpdateAsync(cUser);

            return View();      
        }   

        public ActionResult GetUserInfo(String id)
        {
            ApplicationUser user = (from u in db.Users where u.Id == id select u).Single();
            try
            {
                String json = JsonConvert.SerializeObject(user, Formatting.Indented);
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
