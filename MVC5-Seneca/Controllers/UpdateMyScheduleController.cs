using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.CodeAnalysis;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.EntityModels;
using MVC5_Seneca.ViewModels;

namespace MVC5_Seneca.Controllers
{
    public class UpdateMyScheduleController : Controller
    {
        private readonly SenecaContext _db = new SenecaContext();
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            ApplicationUser user = (from u in _db.Users where u.Id == userId select u).Single();
            UpdateMyScheduleViewModel viewModel = new UpdateMyScheduleViewModel
            {Tutor = user};

            foreach (var tutorSchedule  in _db.TutorSchedules.ToList())
                using (var context = new SenecaContext())
                {
                    var sqlString = "SELECT Tutor_Id FROM TutorSchedule WHERE Id = " + tutorSchedule.Id;
                    var tutorId = context.Database.SqlQuery<string>(sqlString).FirstOrDefault();
                    var tutor = _db.Users.Find(tutorId);
                    if (tutor.Id != userId) continue;

                    sqlString = "SELECT Student_Id FROM TutorSchedule WHERE Id = " + tutorSchedule.Id;
                    var studentId = context.Database.SqlQuery<int>(sqlString).FirstOrDefault();
                    var student = _db.Students.Find(studentId);
                    
                    tutor.TutorSchedules =new List<TutorSchedule>();
                    TutorSchedule ts = new TutorSchedule
                    {
                        Id = tutorSchedule.Id,
                        Tutor = tutor,
                        Student = student,
                        MinutesPastMidnight = tutorSchedule.MinutesPastMidnight,
                        DayName = GetDayOfWeekName(tutorSchedule.DayOfWeekIndex),
                        DayOfWeekIndex = tutorSchedule.DayOfWeekIndex, 
                        TimeOfDay = ConvertToHhmm(tutorSchedule.MinutesPastMidnight)
                    };
                    viewModel.Tutor.TutorSchedules.Add(ts);
                }  
  
            return View(viewModel);
    }

    // POST: UpdateMySchedule/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Title,PhoneNumber,Email")] UpdateMyScheduleViewModel viewModel)
    {
    if (ModelState.IsValid)
        {
        var user = _db.Users.Find(viewModel.Id );
            if (user != null)
            {
                //user.FirstName = viewModel.FirstName;
                //user.LastName = viewModel.LastName;
              

                //_db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
        }
        return View(viewModel);
    }

        public ActionResult Create()
        {
            UpdateMyScheduleViewModel viewModel = new UpdateMyScheduleViewModel();
            return View(viewModel);
        }

        // POST: Teachers/Create                                                      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,LastName")] UpdateMyScheduleViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Teacher teacher = new Teacher
                {
                    //FirstName = viewModel.FirstName,
                    //LastName = viewModel.LastName
                };
                _db.Teachers.Add(teacher);
                _db.SaveChanges();
                return RedirectToAction("Index");

            }

            return View(viewModel);
        }

        public ActionResult ReturnToDashboard()
        {
        return RedirectToAction("Index", "Home");
        }

        private static Int32 GetDayOfWeekIndex(string dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case "Monday": return (0);
                case "Tuesday": return (1);
                case "Wednesday": return (2);
                case "Thursday": return (3);
                case "Friday": return (4);
                case "Saturday": return (5);
                case "Sunday": return (6);
            }
            return 0;
        }

        private string GetDayOfWeekName(int index)
        {
            switch (index)
            {
                case 0: return ("Monday");
                case 1: return ("Tuesday");
                case 2: return ("Wednesday");
                case 3: return ("Thursday");
                case 4: return ("Friday");
                case 5: return ("Saturday");
                case 6: return ("Sunday");
            }
            return ("Monday");
        }

        private static Int32 ConvertToMinutesPastMidnight(string hhmm)
        {
            if (hhmm == "TBD") { return (9999); }
            string[] hm = hhmm.Split(':');
            int h = short.Parse(hm[0]);
            if (h < 10) { h = h + 12; }
            int m = short.Parse(hm[1]);
            return (h * 60 + m);
        }

        private static string ConvertToHhmm(int mpm)
        {
            if (mpm == 9999) { return ("TBD"); }

            int h = mpm / 60;
            int m = mpm - (h * 60);
            if (h > 12) { h = h - 12; }
            return (h.ToString() + ":" + m.ToString("d2"));
        }
    }
}