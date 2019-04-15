﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Web.WebPages;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.EntityModels;
using MVC5_Seneca.ViewModels;

namespace MVC5_Seneca.Controllers
{
    public class TutorSchedulesController : Controller
    {
        private SenecaContext _db = new SenecaContext();

        // GET: TutorSchedules
        public ActionResult Index()
        {
            var model = new List<TutorSchedule >();
            var scheduleList = new List <TutorSchedule>();
            foreach (var tutorSchedule in _db.TutorSchedules.ToList())
            {  
                using (var context = new SenecaContext())
                {
                    var sqlString = "SELECT Tutor_Id FROM TutorSchedule WHERE Id = " + tutorSchedule.Id;
                    var tutorId = context.Database.SqlQuery<string>(sqlString).FirstOrDefault();
                    tutorSchedule.Tutor = _db.Users.Find(tutorId);

                    sqlString = "SELECT Student_Id FROM TutorSchedule WHERE Id = " + tutorSchedule.Id;
                    var studentId = context.Database.SqlQuery<int>(sqlString).FirstOrDefault();
                    tutorSchedule.Student = _db.Students.Find(studentId);

                    model.Add(tutorSchedule);                      
                }                                       
            }
            return View(model);
            //return View(_db.TutorSchedules.ToList());
        }

        // GET: TutorSchedules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TutorSchedule tutorSchedule = _db.TutorSchedules.Find(id);
            if (tutorSchedule == null)
            {
                return HttpNotFound();
            }
            return View(tutorSchedule);
        }

        // GET: TutorSchedules/Create
        public ActionResult Create()
        {
            var viewModel = new TutorScheduleViewModel(); 
            var tutors = _db.Users.OrderBy(u => u.LastName).ToList();
            var validTutorList = new List<ApplicationUser>();
            foreach (ApplicationUser user in tutors)
            {
                foreach (var role in user.Roles)
                {
                    var identityRole = (from r in _db.Roles where (r.Id == role.RoleId) select r).Single();
                    if (identityRole.Name != "Tutor") continue;
                    //viewModel.Tutors.Add(user);
                    validTutorList.Add(user);
                }
            }

            viewModel.Tutors = validTutorList;
            viewModel.Students = (_db.Students.OrderBy(u => u.FirstName).ToList());
            viewModel.DaysList = new List<string>()
            {"Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"};
            viewModel.TimesList = new List<string>()
            {"TBD",
                "10:00","10:15","10:30","10:45","11:00","11:15","11:30","11:45",
                "1:00","1:15","1:30","1:45","2:00","2:15","2:30","2:45",
                "3:00","3:15","3:30","3:45","4:00","4:15","4:30","4:45",
                "5:00","5:15","5:30"
            };
            return View(viewModel);
        }

        // POST: TutorSchedules/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Tutor,Student,DayName,TimeOfDay")]
            TutorScheduleViewModel tutorSchedule)
        {
            tutorSchedule.ErrorMessage = null;
            if (tutorSchedule.Tutor.Id == null)
            {
                tutorSchedule.ErrorMessage = "Tutor Required!";
            }

            if (tutorSchedule.Student.Id == 0)
            {
                tutorSchedule.ErrorMessage = "Student Required!";
            }

            if (tutorSchedule.ErrorMessage != null || ModelState.IsValid != true) // rebuild drop-down lists:
            {
                var tutorRoleId = (from r in _db.Roles where (r.Name == "Tutor") select r.Id).Single();
                var listTutors = new List<ApplicationUser>();
                var users = _db.Users.ToList();
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

                tutorSchedule.Tutors = listTutors;
                tutorSchedule.Students = _db.Students.OrderBy(s => s.FirstName).ToList();
                List<string> daysList = new List<string>
                    {"Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"};
                tutorSchedule.DaysList = daysList;
                List<string> timesList = new List<string>
                { "TBD",
                    "10:00","10:15","10:30","10:45","11:00","11:15","11:30","11:45",
                    "1:00","1:15","1:30","1:45","2:00","2:15","2:30","2:45",
                    "3:00","3:15","3:30","3:45","4:00","4:15","4:30","4:45","5:00","5:15","5:30"
                };
                tutorSchedule.TimesList = timesList;
                return View(tutorSchedule);
            }

            ApplicationUser tutor = _db.Users.Find(tutorSchedule.Tutor.Id);
            Student student = _db.Students.Find(tutorSchedule.Student.Id);
            TutorSchedule  newTutorSchedule = new TutorSchedule()
            {
                Tutor = tutor,
                Student = student,
                DayName =tutorSchedule .DayName,
                TimeOfDay =tutorSchedule.TimeOfDay   
            };
            _db.TutorSchedules.Add(newTutorSchedule);
            try
            {
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                var x = ex;
            }      
            return RedirectToAction("Index");        
        }

        // GET: TutorSchedules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TutorSchedule tutorSchedule = _db.TutorSchedules.Find(id);
            if (tutorSchedule == null)
            {
                return HttpNotFound();
            }
            return View(tutorSchedule);
        }

        // POST: TutorSchedules/Edit/5.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DayName,TimeOfDay")] TutorSchedule tutorSchedule)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(tutorSchedule).State = EntityState.Modified;
               _db.SaveChanges();
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
            TutorSchedule tutorSchedule = _db.TutorSchedules.Find(id);
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
            TutorSchedule tutorSchedule = _db.TutorSchedules.Find(id);
            _db.TutorSchedules.Remove(tutorSchedule);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}