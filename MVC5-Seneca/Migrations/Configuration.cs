﻿namespace MVC5_Seneca.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
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

        private void AddRole(MVC5_Seneca.DataAccessLayer.SenecaContext context, String name)
        {
            if (!context.Roles.Any(r => r.Name == name))
            {
                var store = new RoleStore<IdentityRole>(context);
                var manager = new RoleManager<IdentityRole>(store);
                var role = new IdentityRole(name);
                manager.Create(role);
            }
        }

        private void AddAdministrator(MVC5_Seneca.DataAccessLayer.SenecaContext context,
            String userName, String passwordHash, String securityStamp, String email = "",
            Boolean LockoutEnabled = true, string phoneNumber = "", 
            String firstName = "", String lastName = "", Boolean active = true)
        {  
            if (!context.Users.Any(u => u.UserName == userName))           
            {
                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var appUser = new ApplicationUser();
                appUser.UserName = userName;
                appUser.Active = active;
                appUser.Email = email;
                appUser.EmailConfirmed = (email != "");
                appUser.PhoneNumber = phoneNumber;      
                appUser.PhoneNumberConfirmed = (phoneNumber != "");
                appUser.PasswordHash = passwordHash;
                appUser.SecurityStamp = securityStamp;
                manager.Create(appUser);
                manager.AddToRole(appUser.Id, "Administrator");
            }
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
                AddRole(context, "Administrator");
                AddRole(context, "Primary Tutor");
                AddRole(context, "Associate Tutor");
                AddRole(context, "Receive Registration Email");
                AddRole(context, "Manager");

                AddAdministrator(context,"p", "AMxcdoBNYrk+PEZUwbAK46Uk1ffoFyqKbyQ1Rn+JIKxk0B2ZdBbCNjEx7jFYIns2Ug==", "c6adf2b0-03a5-4c43-bf59-069c8b8b25a2",
                    "prowny@aol.com",true,"3013655823","Peter", "Rowny", true);
                AddAdministrator(context, "dave", "AHeU6mfmXAYrBfr4IsIfgmghgwXRteBzHTu8TcT1GmeXZdqk1JN9w3Js+QeOYmrMFQ==", "ef00f25e-9c8f-4023-a824-8c05065b9fe0",
                    "davemwein@gmail.com",true, "2402741896‬", "Dave","Weinstein",true);
                AddAdministrator(context, "prowny", "ABF43OX0r8HcjPLxkQIxBwUnrtl2W4nA2khEGdEJn4eTxwvmZVxU+tTlJ7tl69Zq3w==", "1efd5200-0df4-4760-b892-c54a4b3d7dd8",
                    "peter@rowny.com",true, "2408885159", "Peter", "Rowny", true);

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
                var documentType = (from t in context.DocumentTypes where t.Name == "IreadyReport" select t).Single();
                context.StudentReports.AddOrUpdate(x => x.DocumentDate,
                new StudentReport()
                {
                    DocumentDate = DateTime.ParseExact("2017-06-15", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Comments = "Trinity needs work on math and algebraic equations.",
                    Student = student,
                    DocumentType = documentType,
                    DocumentLink = "/iReportFiles/Trinity06-15-17-M.pdf"
                },
                new StudentReport()
                {
                    DocumentDate = DateTime.ParseExact("2017-06-06", "yyyy-MM-dd", CultureInfo.InvariantCulture),
                    Comments = "Trinity needs help with spelling.",
                    Student = student,
                    DocumentType = documentType,
                    DocumentLink = "/iReportFiles/Trinity06-06-17-R.pdf"
                });                                                                                                                                                      
                var user1 = (from v in context.Users where v.UserName == "dave" select v).Single();
                var user2 = (from v in context.Users where v.UserName == "prowny" select v).Single();
                context.TutorNotes.AddOrUpdate(x => x.SessionNote,
                new TutorNote()
                {
                    Date = new DateTime(2017, 06, 15),
                    Student = student,
                    ApplicationUser = user1,
                    SessionNote = "Spent the session getting to know Trinity. She \"doesn't like math\".   Spent the session getting to know Trinity. She \"doesn't like math\".  (Testing repeat)"
                },
                new TutorNote()
                {
                    Date = new DateTime(2017, 06, 06),
                    Student = student,
                    ApplicationUser = user2,
                    SessionNote = "Trinity can now add 2-digit numbers!"
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

