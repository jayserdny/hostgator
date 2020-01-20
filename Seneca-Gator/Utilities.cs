using System;
using System.Linq;
using System.Threading.Tasks;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.EntityModels;
using SendGrid;
using SendGrid.Helpers.Mail;


namespace MVC5_Seneca
{
    public static class Utilities
    {
        public static Boolean UserIsInRole(string userId, string roleName)
        {
            var sqlString = "SELECT Id FROM AspNetRoles WHERE Name = '" + roleName + "'";
            string roleId;
            using (var context = new SenecaContext())
            {
                roleId = context.Database.SqlQuery<string>(sqlString).FirstOrDefault();
                if (roleId == null)
                {
                    return false;
                }
            }

            sqlString = "SELECT UserId FROM AspNetUserRoles WHERE ";
            sqlString += "UserId = '" + userId + "' AND RoleId ='" + roleId + "'";
            using (var context = new SenecaContext())
            {
                var success = context.Database.SqlQuery<string>(sqlString).FirstOrDefault();
                if (success != null)
                {
                    return true;
                }

                return false;
            }
        }

        public static async Task EmailHFEDScheduleChange(string userId, int scheduleId, int providerId, string recipientId)
        {
            var context = new SenecaContext();
            var usr = context.Users.Find(userId);
            var recipient = context.Users.Find(recipientId);
            var schedDate = context.HfedSchedules.Where(i => i.Id == scheduleId).Select(i => i.Date).FirstOrDefault();
            // returns provider = null:   HfedSchedule sched = context.HfedSchedules.SingleOrDefault(i => i.Id == scheduleId);
            string providerName;
            using (context)
            {
                var sqlString = "SELECT Name FROM HfedProvider WHERE Id = " + providerId;
                providerName = context.Database.SqlQuery<string>(sqlString).FirstOrDefault();
            }

            var client = new SendGridClient(Properties.Settings.Default.SendGridClient);
            var from = new EmailAddress("Admin@SenecaHeightsEducationProgram.org", "Coordinator, HFED");
            var subject = "HFED: Healthy Food Every Day";
            var to = new EmailAddress(recipient.Email, recipient.FullName);
            var plainTextContent = "User " + usr.FullName + " has updated a "
                    + schedDate.ToString("MM/dd/yyyy")
                    + " food run from " + providerName + "."
                    + Environment.NewLine + " Please do not reply to this email.";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, null);
            await client.SendEmailAsync(msg).ConfigureAwait(false);
        }

        public static async Task EmailDriverSignup(string userId, int scheduleId, int providerId,
            int locationId, string recipientId, bool signUp)
        {
            var context = new SenecaContext();
            var usr = context.Users.Find(userId);
            var recipient = context.Users.Find(recipientId);
            var schedDate = context.HfedSchedules.Where(i => i.Id == scheduleId).Select(i => i.Date).FirstOrDefault();
            var schedTime = context.HfedSchedules.Where(i => i.Id == scheduleId).Select(i => i.PickUpTime).FirstOrDefault();
            // this returns provider & location null ==>: HfedSchedule sched = context.HfedSchedules.SingleOrDefault(i => i.Id == scheduleId);
            string providerName;
            string locationName;

            var sqlString = "SELECT Name FROM HfedProvider WHERE Id = " + providerId;
            providerName = context.Database.SqlQuery<string>(sqlString).FirstOrDefault();
            sqlString = "SELECT Name FROM HfedLocation WHERE Id = " + locationId;
            locationName = context.Database.SqlQuery<string>(sqlString).FirstOrDefault();

            var client = new SendGridClient(Properties.Settings.Default.SendGridClient);
            var from = new EmailAddress("Admin@SenecaHeightsEducationProgram.org", "Coordinator, HFED");
            var subject = "HFED: Healthy Food Every Day";
            var to = new EmailAddress(recipient.Email, recipient.FullName);
            var firstLine = signUp ? "You have signed up " : "You have cancelled ";
            var plainTextContent = "Greetings " + usr.FirstName + "!" + Environment.NewLine
                                   + firstLine + "a food run on "
                                   + schedDate.ToString("MM/dd/yyyy")
                                   + " from " + providerName + " to " + locationName
                                   + ", pickup time: " + schedTime
                                   + Environment.NewLine
                                   + "Thank you for being part of this initiative. "
                                   + "Your care into action truly helps people have what they need to thrive!";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, null);
            await client.SendEmailAsync(msg).ConfigureAwait(false);
            //var allUsers = new List<ApplicationUser>();

            var allUsers = context.Users.ToList();

            foreach (ApplicationUser user in allUsers)
            {
                if (UserIsInRole(user.Id, "ReceiveHfedScheduleChangeEmail"))
                {
                    to = new EmailAddress(user.Email, user.FullName);
                    plainTextContent = "COPY OF EMAIL TO " + usr.FullName
                    + Environment.NewLine + plainTextContent;
                    msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, null);
                    await client.SendEmailAsync(msg).ConfigureAwait(false);
                }
            }
        }
    }
}