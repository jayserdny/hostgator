using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.ViewModels;

namespace MVC5_Seneca.EntityModels
{
    public class TutorSchedulesController : Controller
    {
        private SenecaContext db = new SenecaContext();

        // GET: TutorSchedules
        public ActionResult Index()
        {
            try
            {
                return View(db.TutorSchedules.ToList());
            }
            catch (Exception ex)
            {
                 var x = ex;
            }
            return View(db.TutorSchedules.ToList());
        }
         
        // GET: TutorSchedules/Create
        public ActionResult Create()                  
        {   
            TutorScheduleViewModel model = new TutorScheduleViewModel();
         
            var tutorRoleId = (from r in db.Roles where (r.Name == "Tutor") select r.Id).Single();
            var listTutors = new List<ApplicationUser>();
            var users = db.Users.ToList();
            foreach (var user in users)
            {
                foreach (var role in user.Roles)
                {
                    if (role.RoleId == tutorRoleId)
                    {
                        listTutors.Add(user);
                    } 
                }
            }                                      
            model.Tutors = listTutors;

            model.Students = db.Students.OrderBy(s => s.FirstName).ToList(); 

            List<string> daysList = new List<string>
            {"Monday","Tuesday","Wednesday","Thursday","Friday", "Saturday", "Sunday"};
            model.DaysList = daysList;
            List<string> timesList = new List<string>
            {
                "TBD","10:00", "10:15", "10:30", "10:45","11:00", "11:15", "11:30", "11:45",
                "12:00", "12:15", "12:30", "12:45","1:00", "1:15", "1:30", "1:45",
                "2:00", "2:15", "2:30", "2:45", "3:00","3:15", "3:30", "3:45",
                "4:00", "4:15", "4:30", "4:45", "5:00","5:15", "5:30"
            };
            model.TimesList = timesList;
            return View(model);
        }

        // POST: TutorSchedules/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Tutor,Student,DayName,TimeOfDay")] TutorScheduleViewModel viewModel)
        {
            viewModel.ErrorMessage = null;
            if (viewModel.Tutor.Id == null)
            {
                viewModel.ErrorMessage = "Tutor Required!";
            }
            if (viewModel.Student.Id == 0)
            {
                viewModel.ErrorMessage = "Student Required!";
            }

            if (viewModel.ErrorMessage != null) // rebuild drop-down lists:
            {      
                var tutorRoleId = (from r in db.Roles where (r.Name == "Tutor") select r.Id).Single();
                var listTutors = new List<ApplicationUser>();
                var users = db.Users.ToList();
                foreach (var user in users)
                {
                    foreach (var role in user.Roles)
                    {
                        if (role.RoleId == tutorRoleId)
                        {
                            listTutors.Add(user);
                        }
                    }
                }
                viewModel.Tutors = listTutors;
                viewModel.Students = db.Students.OrderBy(s => s.FirstName).ToList();
                List<string> daysList = new List<string>
                    {"Monday","Tuesday","Wednesday","Thursday","Friday", "Saturday", "Sunday"};
                viewModel.DaysList = daysList;
                List<string> timesList = new List<string>
                {"TBD","10:00", "10:15", "10:30", "10:45","11:00", "11:15", "11:30", "11:45",
                    "12:00", "12:15", "12:30", "12:45","1:00", "1:15", "1:30", "1:45",
                    "2:00", "2:15", "2:30", "2:45", "3:00","3:15", "3:30", "3:45",
                    "4:00", "4:15", "4:30", "4:45", "5:00","5:15", "5:30"};
                viewModel.TimesList = timesList;
                return View(viewModel);
            }

            if (ModelState.IsValid)
            {
                TutorSchedule schedule = new TutorSchedule
                {   
                    Tutor = viewModel.Tutor,
                    Student = viewModel.Student,
                    DayName = viewModel.DayName,
                    TimeOfDay = viewModel.TimeOfDay
                };   
                db.TutorSchedules.Add(schedule);
                try
                {db.SaveChanges();}
                catch (Exception ex)
                {
                    var x = ex;
                }
                return RedirectToAction("Index");
            }

            return View(viewModel);
        }

        // GET: TutorSchedules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TutorSchedule tutorSchedule = db.TutorSchedules.Find(id);
            if (tutorSchedule == null)
            {
                return HttpNotFound();
            }
            return View(tutorSchedule);
        }

        // POST: TutorSchedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DayName,TimeOfDay")] TutorSchedule tutorSchedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tutorSchedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tutorSchedule);
        }

        // GET: TutorSchedules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TutorSchedule tutorSchedule = db.TutorSchedules.Find(id);
            if (tutorSchedule == null)
            {
                return HttpNotFound();
            }
            return View(tutorSchedule);
        }

        // POST: TutorSchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TutorSchedule tutorSchedule = db.TutorSchedules.Find(id);
            db.TutorSchedules.Remove(tutorSchedule);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
