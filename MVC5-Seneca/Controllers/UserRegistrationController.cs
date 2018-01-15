using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.ViewModels;
using MVC5_Seneca.Models;
using MVC5_Seneca.EntityModels;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MVC5_Seneca.Controllers
{
    public class UserRegistrationController : Controller
    {
        private SenecaContext db = new SenecaContext();

        // GET: UserRegistration
        public ActionResult Index()
        {
            // return to login page after Create: 
            return RedirectToAction("Index", "Login");
        }       

        // GET: UserRegistration/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserRegistration/Create
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Create([Bind(Include = "Id,Name,PasswordSalt,PasswordHash,LoginErrorMessage,FirstName,LastName,Address,CityStateZip,Email,CellPhone,HomePhone")] CreateUserRegistrationViewModel CreateUserRegistrationViewModel)
        {

            if (ModelState.IsValid)
                // Check for existing User Name:
                using (MVC5_Seneca.DataAccessLayer.SenecaContext db = new MVC5_Seneca.DataAccessLayer.SenecaContext())
                {                                     
                    var userDetails = new EntityModels.User();
                    userDetails = db.Users.Where(x => x.Name == CreateUserRegistrationViewModel.Name).FirstOrDefault();
                    if (userDetails == null)
                    {
                        var user = db.Users.Create();
                        user.Name = CreateUserRegistrationViewModel.Name;
                        user.PasswordSalt = App_Code.EncryptSHA256.GenerateSALT(24);
                        user.PasswordHash = App_Code.EncryptSHA256.EncodeSHA256(CreateUserRegistrationViewModel.PasswordHash,user.PasswordSalt);
                        user.Active = false;
                        user.FirstName = CreateUserRegistrationViewModel.FirstName;
                        user.LastName = CreateUserRegistrationViewModel.LastName;
                        user.Address = CreateUserRegistrationViewModel.Address;
                        user.CityStateZip = CreateUserRegistrationViewModel.CityStateZip;
                        user.CellPhone = CreateUserRegistrationViewModel.CellPhone;
                        user.HomePhone = CreateUserRegistrationViewModel.HomePhone;
                        user.Email = CreateUserRegistrationViewModel.Email;
                     
                        db.Users.Add(user);
                        db.SaveChanges(); 
                        EmailAdministrators(user).Wait();
                        CreateUserRegistrationViewModel.LoginErrorMessage = "SUCCESFUL REGISTRATION. Please wait for Administrator verification.";
                       
                        return View("Create", CreateUserRegistrationViewModel);           
                    }
                    else
                    {
                        CreateUserRegistrationViewModel.LoginErrorMessage= "Invalid UserName (already in use).";
                        return View("Create", CreateUserRegistrationViewModel);
                    }
                }
            {             
                return View(CreateUserRegistrationViewModel);
            }
        }  // public ActionResult Create

        private async Task EmailAdministrators(User user)
        {
            var receiverRole = Properties.Settings.Default.RegistrationEmailReceiverRole;
            var _Roles = (from r in db.UserRoles where r.Role == receiverRole select r ).ToList();
            foreach (var _role in _Roles)
            {
                var _user = (from u in db.Users where u.Id == _role.User.Id select u).Single();
                var client = new SendGridClient(Properties.Settings.Default.SendGridClient);
                var from = new EmailAddress("Admin@SenecaHeightsEducationProgram.org", "Administrator, SHEP");
                var subject = "SHEP: Seneca Heights Education Project";
                var to = new EmailAddress(_user.Email, _user.FirstName);
                var plainTextContent = "New Registration: " + user.FirstName + " " + user.LastName + " " + user.Email;
                string htmlContent = null;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
            }
        }       
       
    }  // controller
}  // namespace
