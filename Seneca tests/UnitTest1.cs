using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.EntityModels;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Diagnostics;
using MVC5_Seneca.Models;

namespace Seneca_tests
{

    [TestClass]
    public class UnitTest1
    {
        [AssemblyInitialize]
        public static void InitializeAssembly(TestContext context)
        {
            Database.SetInitializer(new DropDbAndSeed<SenecaContext, MVC5_Seneca.Migrations.Configuration>());
        }

        [TestMethod]
        public void TestSaveStudentTutorNote() 
        {
            using (SenecaContext db = new SenecaContext())
            {
                Debug.WriteLine(db.Database.Connection.ConnectionString);
                db.Database.Log = Console.Write;
                Student student = (from s in db.Students where s.FirstName == "Jayden" select s).Single();
                ApplicationUser user = (from u in db.Users where u.UserName == "p" select u).Single();

                TutorNote note = db.TutorNotes.Create();
                note.Date = DateTime.Now;
                note.SessionNote = "xxx";
                note.Student = student;
                note.ApplicationUser = user;

                db.TutorNotes.Add(note);

                db.SaveChanges();

                TutorNote foundNote = (from n in db.TutorNotes where n.SessionNote == "xxx" select n).SingleOrDefault();
                Assert.IsNotNull(foundNote);
                Assert.AreEqual(student, foundNote.Student);
                Assert.AreEqual(user, foundNote.ApplicationUser);
                Assert.IsTrue(student.TutorNotes.Contains(foundNote));
            }
        }

        //[TestMethod]
        //public void Test2()
        //{
        //    int x = 2 + 3;
        //    Assert.AreEqual(6, x);
        //}

        [TestMethod]
        public async Task TestSendGrid() 
        {
            var client = new SendGridClient("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");
            var from = new EmailAddress("Admin@senecaheightseducationprogram.org", "Administrator, SHEP");
            var subject = "SHEP: Seneca Heights Education Project";
            var to = new EmailAddress("peter@rowny.com", "Peter");
            var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }
    }
}


