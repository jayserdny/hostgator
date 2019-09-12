using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.EntityModels;
using MVC5_Seneca.ViewModels;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MVC5_Seneca.Controllers
{
    public class HfedEmailController : Controller
    {
        private SenecaContext db = new SenecaContext();
        // GET: HfedEmail
        public ActionResult Index(HfedEmailViewModel email)
        {
            if (email != null) email.Recipients = TempData["Recipients"] as List<HfedEmailRecipient>;
            //TempData["RecipientIds"] = recipientIds.ToArray();
            return View(email);
        }

        // POST: HfedEmail
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include ="Title,EmailText")] HfedEmail hfedEmail )
        {
            
            return (null);
        }

        // GET: HfedEmail
        public ActionResult EmailStaffAsk()
        {
            var email = new HfedEmailViewModel();
            
            email.Title = "Email Staff - Request Schedules";
            var text = "Greetings MCCH HFED Team!\n";      
            text += " The HFED coordinator has tentatively copied the food delivery" +
                           " schedules from the past month into next month. Please sign in to the HFED" +
                           " website at https://MVC5Seneca.Azurewebsites.net to verify the details including" +
                           " the list of clients.";
                email.EmailText = text;

            var allUsers = db.Users.ToList();
            List<ApplicationUser> recipients = new List<ApplicationUser>();
            foreach (ApplicationUser user in allUsers)
            {
                if (UserIsInRole(user, "HfedStaff"))
                {
                    recipients.Add(user);                
                }
            }
            string[] recipientIds = new string[recipients.Count];
            int length = recipients.Count;
            for (int i = 0; i < length; i++)
            {
                // Note that we employ the ToString() method to convert the Guid to the string
                recipientIds [i] = recipients[i].Id.ToString();
            }

            var newList = new List<HfedEmailRecipient>();
            foreach (ApplicationUser user in recipients)
            {
                newList.Add(new HfedEmailRecipient() 
                    { Id = user.Id, FullName = user.FullName, Email = user.Email, Checked =true});
            }

            TempData["Recipients"] = newList;
            TempData["RecipientIds"] = recipientIds.ToArray();
            email.Recipients = newList; 
            email.RecipientIds = recipientIds.ToArray();

            return RedirectToAction( "Index", email);
        }


        // POST: HfedEmail
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EmailStaffAsk([Bind(Include = "Title,EmailText")] HfedEmail  hfedEmail)
        {
            var text = hfedEmail.EmailText;
            return (null);
        }

        // Send individual email
        private async void SendEmail(string address, string htmlContent)
        {
            var apiKey = Properties.Settings.Default.HFEDSendGridClient;
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("Admin@SenecaHeightsEducationProgram.org", "HFED Coordinator"),
                Subject =("Healthy Food Every Day -  Schedule"),
                PlainTextContent  = htmlContent,                          
            };
            msg.AddTo(new EmailAddress(address, "HFED Team Member"));
            var response = await client.SendEmailAsync(msg);
        }

        private Boolean UserIsInRole(ApplicationUser user, string roleName)
        {
            var roleStore = new RoleStore<IdentityRole>(db);
            var roleMngr = new RoleManager<IdentityRole>(roleStore);
            var roles = roleMngr.Roles.ToList();

            foreach (IdentityRole role in roles)
            {
                if (role.Name == roleName)
                {
                    using (var context = new SenecaContext())
                    {
                        string strSql = "SELECT RoleId FROM AspNetUserRoles WHERE ";
                        strSql += "RoleId ='" + role.Id + "' AND UserId ='" + user.Id + "'";
                        string result = context.Database.SqlQuery<string>(strSql).FirstOrDefault();
                        if (result != null)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public ActionResult ReturnToHfedSchedules()
        {
            return RedirectToAction("Index", "HfedSchedules");
        }

    }
}