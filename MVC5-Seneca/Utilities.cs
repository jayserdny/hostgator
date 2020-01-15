using System;
using System.Linq;
using System.Threading.Tasks;
using MVC5_Seneca.DataAccessLayer;
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
            var usr = context.Users.Find(userId );        
            var recipient = context.Users.Find(recipientId);       
            var schedDate = context.HfedSchedules.Where(i => i.Id == scheduleId).Select(i => i.Date).FirstOrDefault();
            // returns provider = null:   HfedSchedule sched = context.HfedSchedules.SingleOrDefault(i => i.Id == scheduleId);
            string providerName;
            using (context)
            {
               var sqlString = "SELECT Name FROM HfedProvider WHERE Id = " + providerId ;
               providerName = context.Database.SqlQuery<string>(sqlString).FirstOrDefault();
            }
                
            var client = new SendGridClient(Properties.Settings.Default.SendGridClient);
            var from = new EmailAddress("Admin@SenecaHeightsEducationProgram.org", "Coordinator, HFED");
            var subject = "HFED: Healthy Food Every Day";
            var to = new EmailAddress( recipient.Email, recipient .FullName );       
            var plainTextContent = "User " + usr.FullName + " has updated a " 
                    + schedDate.ToString( "MM/dd/yyyy") 
                    + " food run from " + providerName + "." 
                    + Environment.NewLine + " Please do not reply to this email.";       
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, null);
            await client.SendEmailAsync(msg).ConfigureAwait(false);
        }
    }
}