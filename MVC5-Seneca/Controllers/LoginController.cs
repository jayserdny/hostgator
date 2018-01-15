using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MVC5_Seneca.DataAccessLayer;

namespace MVC5_Seneca.Controllers
{  
    public class LoginController : Controller
    {
        private SenecaContext db = new SenecaContext();

        // GET: Login
        public ActionResult Index()
        {   
            FormsAuthentication.SignOut();
            return View();
        }
        
        public ActionResult Register() => RedirectToAction("Create", "UserRegistration");

        [HttpPost]
        public ActionResult Authorize(MVC5_Seneca.EntityModels.User UserModel)
        {
            using (MVC5_Seneca.DataAccessLayer.SenecaContext db = new MVC5_Seneca.DataAccessLayer.SenecaContext())
            {
                var userDetails = db.Users.Where(x => x.Name == UserModel.Name).FirstOrDefault();
                if (userDetails == null)
                {
                    UserModel.LoginErrorMessage = "Wrong username or password.";
                    return View("Index", UserModel);
                }
                else if (userDetails.Active == false)
                {
                    UserModel.LoginErrorMessage = "Inactive account.";
                    return View("Index", UserModel);
                } 
                else
                {  
                    {
                        string hash = App_Code.EncryptSHA256.EncodeSHA256(UserModel.PasswordHash,userDetails.PasswordSalt);             
                        if (hash == userDetails.PasswordHash)
                        {
                            Session["userId"] = userDetails.Id;
                            Session["userName"] = userDetails.Name;
                            Session["Administrator"] = false;
                            //var role =db.UserRoles.Where(r => r.User.Id == userDetails.Id && r.Role == "Administrator");
                            var roles= db.UserRoles.Where(r => r.User.Id == userDetails.Id).ToList();
                            if (roles.Count != 0)
                            {
                                foreach (var role in roles)
                                {
                                    if (role.Role == "Administrator")
                                    {
                                        Session["Administrator"] = true;
                                    }
                                }                                
                            }
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            UserModel.LoginErrorMessage = "Invalid password.";
                            return View("Index", UserModel);
                        }
                    }                   
                }
            }
        }
       
    }
}
