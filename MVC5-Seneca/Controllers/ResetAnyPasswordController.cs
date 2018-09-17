﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.EntityModels;
using Newtonsoft.Json;

namespace MVC5_Seneca.Controllers
{
    public class ResetAnyPasswordController : Controller
    {  
        private readonly SenecaContext _db = new SenecaContext();
        public async Task< ActionResult> Reset(String userId, String newPassword)       
        {                                                                                                                                     
            UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(_db);
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(store);     
            String hashedNewPassword = userManager.PasswordHasher.HashPassword(newPassword);
            ApplicationUser cUser = await store.FindByIdAsync(userId);
            await store.SetPasswordHashAsync(cUser, hashedNewPassword);
            await store.UpdateAsync(cUser);

            return View();      
        }   

        public ActionResult GetUserInfo(String id)
        {
            ApplicationUser user = (from u in _db.Users where u.Id == id select u).Single();
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
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
