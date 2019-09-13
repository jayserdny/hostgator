using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Castle.Core.Internal;
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
        // GET: HfedEmail/Index
        public ActionResult Index(HfedEmailViewModel email)
        {
            if (email != null)
            {
                TempData.Keep();
                email.Recipients = TempData["Recipients"] as List<HfedEmailRecipient>;    
            
                // Convert to HfedEmail (for different requests to all use Index, a different name of model is needed.)
                HfedEmail model = new HfedEmail()
                {
                    Title = email.Title,
                    EmailText = email.EmailText,
                    Recipients = email.Recipients
                }; 
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "HfedSchedules");
            }
        }

        // POST: HfedEmail/Index
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include ="Title,EmailText,Recipients")] HfedEmail hfedEmail )
        {
            foreach (HfedEmailRecipient recipient in hfedEmail.Recipients)
            {
                if (recipient.Checked)
                {
                    var text = "<p>";
                    text += hfedEmail.EmailText .Replace(Environment.NewLine, "</p><p>");
                    text += "</p>";
                    DateTime startDate = Convert.ToDateTime(Session["StartDate"]);
                    DateTime endDate = Convert.ToDateTime(Session["EndDate"]);
                    if (hfedEmail.Title == "Email Drivers - Show Schedule with Driver Names")
                    {
                        HfedEmail sched = GetDriverSchedule(true, startDate, endDate );
                        text += sched.EmailText;
                    }
                    if (hfedEmail.Title == "Email Drivers - Show Schedule with No Driver Names")
                    {
                        HfedEmail sched = GetDriverSchedule(false, startDate, endDate );
                        text += sched.EmailText;
                    }
                    SendEmail(recipient.Email, text);
                }
            }

            return RedirectToAction("Index", "HfedSchedules");
        }

        // GET: HfedEmail
        public ActionResult EmailStaffAsk()
        {
            var email = new HfedEmailViewModel();
            
            email.Title = "Email Staff - Request Schedules";
            var text = "Greetings MCCH HFED Team!\n";      
            text += " The HFED coordinator has tentatively copied the food delivery" +
                           " schedules from the past month into next month.\n Please sign in to the HFED" +
                           " website at MVC5Seneca.Azurewebsites.net to verify the details including" +
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
            
            var newList = new List<HfedEmailRecipient>();
            foreach (ApplicationUser user in recipients)
            {
                newList.Add(new HfedEmailRecipient() 
                    { Id = user.Id, FullName = user.FullName, Email = user.Email, Checked =true});
            }

            TempData["Recipients"] = newList; // TempData holds complex data not passed in Redirects.                            
            email.Recipients = newList;                          

            return RedirectToAction( "Index", email);
        }  
        
        public ActionResult  EmailDriversAsk()
        {
            TempData["WithDrivers"] = false;
            EmailDrivers();
            return RedirectToAction("EmailDrivers", false);
        }

        public ActionResult EmailDriversShow()
        {
            TempData["WithDrivers"] = true;
            EmailDrivers();
            return RedirectToAction("EmailDrivers", true);
        }

        public ActionResult  EmailDrivers()    // Ask drivers for availability / show with drivers
        {
            bool withDrivers = (bool) (TempData["WithDrivers"]);
            var email = new HfedEmailViewModel
            {
                Title = "Email Drivers - "
            };
            if (withDrivers)
            {
                email.Title += "Show Schedule with Driver Names";
            }
            else
            {
                email.Title += "Show Schedule with No Driver Names";
            }
            var text = "Greetings Team!\n";
            if (withDrivers)
            {
                text += "Thank you for being a part of the team that works to allow ";
                text += "our MCCH residents to have Healthy Food Every Day.\n ";
            }
            else
            {
                text += "Please take a look and let us know which food runs you are ";
                text += "available for and interested in doing. ";
                text += "We will get back to you soon to confirm.\n";
            }
            
            if (!withDrivers)
            {
                text += "Thank you for helping people have Healthy Food Every Day!";
            }

            email.EmailText = text;

            var allUsers = db.Users.ToList();
            List<ApplicationUser> recipients = new List<ApplicationUser>();
            foreach (ApplicationUser user in allUsers)
            {
                if (UserIsInRole(user, "HfedDriver"))
                {
                    recipients.Add(user);
                }
            }

            var newList = new List<HfedEmailRecipient>();
            foreach (ApplicationUser user in recipients)
            {
                newList.Add(new HfedEmailRecipient()
                    { Id = user.Id, FullName = user.FullName, Email = user.Email, Checked = true });
            }

            TempData["Recipients"] = newList; // TempData holds complex data not passed in Redirects.                            
            email.Recipients = newList;    
            return RedirectToAction("Index", email);  
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
                HtmlContent = htmlContent,                          
            };
            msg.AddTo(new EmailAddress(address, "HFED Team Member"));
            var response = await client.SendEmailAsync(msg);   
        }

        private static HfedEmail GetDriverSchedule(bool withDrivers, DateTime startDate, DateTime endDate)
        {
            using (var context = new SenecaContext())
            {
                string text = "<table border=" + (char) 34 + "1" + (char) 34 + ">";                 
                var sqlString = "SELECT * FROM HfedSchedule WHERE Date >='" + startDate
                                                                            + "' AND Date <='" + endDate + "' Order By Date";
                var deliveryRequests = context.Database.SqlQuery<HfedScheduleViewModel>(sqlString).ToList();
                foreach (HfedScheduleViewModel request in deliveryRequests)
                {
                    text += "<tr><td>" + request.Date.ToShortDateString() + "</td>";

                    sqlString = "SELECT * FROM HfedSchedule WHERE Id = " + request.Id;
                    var schedule = context.Database.SqlQuery<HfedScheduleViewModel>(sqlString).ToList();

                    sqlString = "SELECT * FROM HfedProvider WHERE Id = " + schedule[0].Provider_Id;
                    var provider = context.Database.SqlQuery<HfedProvider>(sqlString).ToList();
                    text += "<td>" + provider[0].Name + "</td>";

                    sqlString = "SELECT * FROM HfedLocation WHERE Id = " + schedule[0].Location_Id;
                    var location = context.Database.SqlQuery<HfedLocation>(sqlString).ToList();
                    text += "<td>" + location[0].Name + "</td>";

                    text += "<td>" + request.PickUpTime + "</td>";

                    ApplicationUser pointPerson = context.Users.Find(schedule[0].PointPerson_Id);
                    text += "<td>" + pointPerson.FirstName + "</td>";

                    text += "<td bgcolor=" + (char) 34 + "#FFFF00" + (char) 34 + ">";
                    if (withDrivers && !request.Driver_Id.IsNullOrEmpty())
                    {
                        ApplicationUser driver = context.Users.Find(request.Driver_Id);
                        string driverName;
                        if (driver == null)
                        {
                            driverName = "&nbsp;&nbsp;" +
                                         "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                                         "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                        }
                        else
                        {
                            driverName = driver.FirstName;
                        } 
                        text += driverName + "</td>";
                        text += "<td>" + request.ScheduleNote + "</td>";
                    }
                    else
                    {
                        text += "&nbsp;</td>";
                        text += "<td>" + request.ScheduleNote + "</td>";
                    }
                }
                text += "<tr></table> ";
                text += "<br /><br /><p>";
                HfedEmail hfedEmail = new HfedEmail() {EmailText = text};
                return (hfedEmail);
            }
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