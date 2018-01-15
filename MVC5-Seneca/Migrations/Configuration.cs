namespace MVC5_Seneca.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using MVC5_Seneca.EntityModels;
    using MVC5_Seneca.Models;

    public sealed class Configuration : DbMigrationsConfiguration<MVC5_Seneca.DataAccessLayer.SenecaContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        private void WriteExceptionToDebugger(DbEntityValidationException ex)
        {
            Debug.WriteLine("DbEntityValidationException");
            Debug.Indent();
            foreach (DbEntityValidationResult ve in ex.EntityValidationErrors)
            {
                Debug.WriteLine(ve.Entry.Entity, "Entity");
                Debug.WriteLine(ve.Entry.State, "State");
                Debug.Indent();
                foreach (DbValidationError e in ve.ValidationErrors)
                {
                    Debug.WriteLine(e.ErrorMessage, "    " + e.PropertyName);
                }
                Debug.Unindent();
            }
            Debug.Unindent();
        }

        // In writing sample or seed data, try to avoid using Id keys. (Find some other unique field
        // or use Composite Primary Keys.)  
        protected override void Seed(MVC5_Seneca.DataAccessLayer.SenecaContext context)
        {

            //  This method will be called after migrating to the latest version.
            try
            {
                // Launch the debugger when 'Sequence contains no elements' error, to find out where:
                // System.Diagnostics.Debugger.Launch();

                context.Users.AddOrUpdate(x => x.Name,
                new User()
                {
                    Name = "p",
                    PasswordSalt = "SFoKiqpBioF7snJvANc6gkPi",
                    PasswordHash = "32-EF-66-B6-AE-4B-9F-08-25-D2-82-82-5D-01-3C-B1-17-07-5B-85-03-BB-A5-40-DE-8E-2F-FB-3A-D4-F5-92",
                    Active = true,
                    FirstName = "Peter",
                    LastName = "Rowny",
                    CityStateZip = "Bethesda, MD  20817",
                    HomePhone = "111-222-3333",
                    CellPhone = "222-333-4444",
                    Email = "peter@rowny.com"
                },
                new User()
                {
                    Name = "prowny",
                    PasswordSalt = "mSN7NYhH6Hdu87JOnjBBoZ7f",
                    PasswordHash = "56-68-AB-A7-2B-E2-6A-D1-98-1D-C7-C4-F6-41-B5-50-1F-9E-69-57-88-21-13-95-16-30-A2-31-DA-B1-35-AB",
                    Active = true,
                    FirstName = "Peter",
                    LastName = "Rowny",
                    CityStateZip = "Bethesda, MD  20817",
                    HomePhone = "111-222-3333",
                    CellPhone = "222-333-4444",
                    Email = "peter@rowny.com"
                },
                new User()
                {
                    Name = "dave",
                    PasswordSalt = "ViKDYBrKWxrN3udWDuOy0YB0",
                    PasswordHash = "45-27-91-CB-FA-65-93-3A-6A-4C-62-A5-5B-C6-41-4F-DA-44-27-C9-2F-91-21-CB-F4-88-31-38-26-73-10-15",
                    Active = true,
                    FirstName = "David",
                    LastName = "Weinstein",
                    CityStateZip = "",
                    HomePhone = "",
                    CellPhone = "(555) 555-6666",
                    Email = "davem1234@gmail.com"
                });
                context.SaveChanges();

                context.Parents.AddOrUpdate(x => x.FirstName,
                     new Parent()
                     {
                         MotherFather = "M",
                         FirstName = "-Not Selected-",  // for top of dropdownlists
                         HomePhone = null,
                         CellPhone = null,
                         Email = null
                     },
                new Parent()
                {
                    MotherFather = "M",
                    FirstName = "Samantha",
                    HomePhone = "333-444-5555",
                    CellPhone = null,
                    Email = null
                },
                new Parent()
                {
                    MotherFather = "M",
                    FirstName = "Shantia",
                    HomePhone = "888-777-6666",
                    CellPhone = null,
                    Email = null
                },
                new Parent()
                {
                    MotherFather = "M",
                    FirstName = "Tracey",
                    HomePhone = null,
                    CellPhone = "777-666-5555",
                    Email = "tracey.1234@yahoo.com"
                },
                new Parent()
                {
                    MotherFather = "M",
                    FirstName = "Denisse",
                    HomePhone = null,
                    CellPhone = "444-555-6789",
                    Email = null
                },
                new Parent()
                {
                    MotherFather = "M",
                    FirstName = "Jasmine",
                    HomePhone = null,
                    CellPhone = "123-456-7890",
                    Email = null
                },
                new Parent()
                {
                    MotherFather = "M",
                    FirstName = "Shakia",
                    HomePhone = null,
                    CellPhone = "189-023-4567",
                    Email = null
                },
                new Parent()
                {
                    MotherFather = "M",
                    FirstName = "Brendance",
                    HomePhone = null,
                    CellPhone = "555-444-3333",
                    Email = null
                });

                context.Schools.AddOrUpdate(x => x.Name,
                new School() { Name = "-Not Selected-" },   // for top of dropdownlists
                new School() { Name = "Watkins Mill Elementary" },
                new School() { Name = "Watkins Mill High School" });

                context.SaveChanges();

                var schoolNotSelected = (from s in context.Schools where s.Name == "-Not Selected-" select s).Single();
                var parentNotSelected = (from p in context.Parents where p.FirstName == "-Not Selected-" select p).Single();
                context.Students.AddOrUpdate(x => x.FirstName,
                    new Student
                    {
                        FirstName = "-Select ID-",
                        School = schoolNotSelected,
                        Parent = parentNotSelected
                    });
                context.SaveChanges();


                var parentSamantha = (from p in context.Parents where p.FirstName == "Samantha" select p).Single();
                var parentShantia = (from p in context.Parents where p.FirstName == "Shantia" select p).Single();
                var parentTracey = (from p in context.Parents where p.FirstName == "Tracey" select p).Single();
                var parentDenisse = (from p in context.Parents where p.FirstName == "Denisse" select p).Single();
                var parentJasmine = (from p in context.Parents where p.FirstName == "Jasmine" select p).Single();
                var parentShakia = (from p in context.Parents where p.FirstName == "Shakia" select p).Single();
                var parentBrendance = (from p in context.Parents where p.FirstName == "Brendance" select p).Single();

                var school1 = (from s in context.Schools where s.Name == "Watkins Mill Elementary" select s).Single();
                var school2 = (from s in context.Schools where s.Name == "Watkins Mill High School" select s).Single();
                context.Students.AddOrUpdate(x => x.FirstName,
                new Student()
                {
                    FirstName = "Heaven",
                    Gender = "F",
                    BirthDate = DateTime.ParseExact("2007-03-04", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    School = school1,
                    Parent = parentSamantha
                },
                new Student()
                {
                    FirstName = "Isaiah",
                    Gender = "M",
                    BirthDate = DateTime.ParseExact("2009-04-10", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    School = school1,
                    Parent = parentShantia
                },
                new Student()
                {
                    FirstName = "Aliyyah",
                    Gender = "F",
                    BirthDate = DateTime.ParseExact("2002-06-18", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    School = school2,
                    Parent = parentBrendance
                },
                new Student()
                {
                    FirstName = "Jayden",
                    Gender = "M",
                    BirthDate = DateTime.ParseExact("2006-09-13", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    School = school1,
                    Parent = parentTracey
                },
                new Student()
                {
                    FirstName = "Jeremiah",
                    Gender = "M",
                    BirthDate = DateTime.ParseExact("2009-10-26", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    School = school1,
                    Parent = parentSamantha
                },
                new Student()
                {
                    FirstName = "Trinity",
                    Gender = "F",
                    BirthDate = DateTime.ParseExact("2007-10-20", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    School = school1,
                    Parent = parentNotSelected
                });
                context.SaveChanges();

                context.DocumentTypes.AddOrUpdate(x => x.Name,
                    new DocumentType()
                    {
                        Name = "-Not Selected-"
                    },
                    new DocumentType()
                    {
                        Name = "ReportCard"
                    },
                    new DocumentType()
                    {
                        Name = "IreadyReport"
                    });
                context.SaveChanges();

                var student = (from s in context.Students where s.FirstName == "Trinity" select s).Single();
                var documenttype = (from t in context.DocumentTypes where t.Name == "IreadyReport" select t).Single();
                context.StudentReports.AddOrUpdate(x => x.DocumentDate,
                new StudentReport()
                {
                    DocumentDate = DateTime.ParseExact("2017-06-15", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Comments = "Trinity needs work on math and algebraic equations.",
                    Student = student,
                    DocumentType = documenttype,
                    DocumentLink = "/iReportFiles/Trinity06-15-17-M.pdf"
                },
                new StudentReport()
                {
                    DocumentDate = DateTime.ParseExact("2017-06-06", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Comments = "Trinity needs help with spelling.",
                    Student = student,
                    DocumentType = documenttype,
                    DocumentLink = "/iReportFiles/Trinity06-06-17-R.pdf"
                });

                var user0 = (from v in context.Users where v.Name == "p" select v).Single();
                var user1 = (from v in context.Users where v.Name == "dave" select v).Single();
                var user2 = (from v in context.Users where v.Name == "prowny" select v).Single();
                context.TutorNotes.AddOrUpdate(x => x.SessionNote,
                new TutorNote()
                {
                    Date = new DateTime(2017, 06, 15),
                    Student = student,
                    User = user1,
                    SessionNote = "Spent the session getting to know Trinity. She \"doesn't like math\".   Spent the session getting to know Trinity. She \"doesn't like math\".  (Testing repeat)"
                },
                new TutorNote()
                {
                    Date = new DateTime(2017, 06, 06),
                    Student = student,
                    User = user2,
                    SessionNote = "Trinity can now add 2-digit numbers!"
                });
                context.SaveChanges();

                context.UserRoles.AddOrUpdate(x => x.Id,
                new UserRole()
                {
                    Role = "Administrator",
                    User = user0
                });
                context.SaveChanges();
            }

            catch (DbEntityValidationException ex)
            {
                System.Diagnostics.Debugger.Launch();
                WriteExceptionToDebugger(ex);
            }
        }
    }
}

