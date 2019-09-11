using System;
using System.Web.Mvc;
using System.Web.UI;
using MVC5_Seneca.EntityModels;
using MVC5_Seneca.ViewModels;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MVC5_Seneca.Controllers
{
    public class HfedEmailController : Controller
    {     
        // GET: HfedEmail
        public ActionResult Index(HfedEmailViewModel email)
        {                                                                                                                 
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
            var email = new HfedEmail();
            var text = "Greetings MCCH HFED Team!";      
            text += " The HFED coordinator has tentatively copied the food delivery" +
                           " schedules from the past month into next month. Please sign in to the HFED" +
                           " website at https://MVC5Seneca.Azurewebsites.net to verify the details including" +
                           " the list of clients.";
                email.EmailText = text;
                email.Title = "Email Staff - Request Schedules";
            return RedirectToAction( "Index", email);
            //return Redirect("/HfedEmail/Index", email);
            //string url = string.Format("/HfedEmail/Index?Title={0}&EmailText={1}", email.Title,email.EmailText);  
            // return Redirect(url);
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
        public ActionResult ReturnToHfedSchedules()
        {
            return RedirectToAction("Index", "HfedSchedules");
        }

    }
}