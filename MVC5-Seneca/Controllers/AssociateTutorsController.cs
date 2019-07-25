using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.EntityModels;
using MVC5_Seneca.ViewModels;
using Newtonsoft.Json;     

namespace MVC5_Seneca.Controllers
{
    public class AssociateTutorsController : Controller
    {
        private SenecaContext _db = new SenecaContext();   

        // GET: AssociateTutors
        public ActionResult Index()
        {
            var model = new List<AssociateTutor>();
            foreach (var tutor in _db.AssociateTutors.ToList())
            {    
                using (var context = new SenecaContext())
                {
                    var sqlString = "SELECT Tutor_Id FROM AssociateTutor WHERE Id = " + tutor.Id;
                    var tutorId = context.Database.SqlQuery<string>(sqlString).FirstOrDefault();
                    tutor.Tutor = _db.Users.Find(tutorId);
                   
                    sqlString = "SELECT Student_Id FROM AssociateTutor WHERE Id = " + tutor.Id;
                    var studentId = context.Database.SqlQuery<int>(sqlString).FirstOrDefault();
                    tutor.Student = _db.Students.Find(studentId);

                    model.Add(tutor);
                }
            }                                                             
            return View(model);
        }

        // GET: AssociateTutors/Details/5
        [SuppressMessage("ReSharper", "Mvc.ViewNotResolved")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssociateTutor associateTutor = _db.AssociateTutors.Find(id);
            if (associateTutor == null)
            {
                return HttpNotFound();
            }
            return View(associateTutor);
        }

        // GET: AssociateTutors/Create
        public ActionResult Create()
        {
            AddEditAssociateTutorViewModel model = new AddEditAssociateTutorViewModel();
            var tutorRoleId = (from r in _db.Roles where (r.Name == "Tutor") select r.Id).Single();
            List<SelectListItem> tutorList = new List<SelectListItem>();
            var users = _db.Users.OrderBy(u => u.LastName).ToList();
            foreach (var user in users)
            {
                foreach (var role in user.Roles)
                {
                    if (role.RoleId == tutorRoleId)
                    {
                        tutorList.Add(new SelectListItem { Text = user.FirstName + @" " + user.LastName, Value = user.Id });
                    }
                }
            }

            List<SelectListItem> studentList = new List<SelectListItem>
            {
                new SelectListItem { Text = @" Select", Value = "0" }
            };
            model.Tutors = tutorList;
            model.Students = studentList;
            return View(model);
        }

        // POST: AssociateTutors/Create          
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Tutor,Student")] AssociateTutor associateTutor)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = _db.Users.Find(associateTutor.Tutor.Id);
                Student student = _db.Students.Find(associateTutor.Student.Id);
                AssociateTutor newAssociateTutor = new AssociateTutor     
                {
                    Tutor = user,
                    Student = student
                };
                _db.AssociateTutors.Add(newAssociateTutor);
                try
                {
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    Exception x = ex;
                }

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Create", "AssociateTutors");
            }
        }

        // GET: AssociateTutors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssociateTutor associateTutor = _db.AssociateTutors.Find(id);
            if (associateTutor == null)
            {
                return HttpNotFound();
            }
            return View(associateTutor);
        }

        // POST: AssociateTutors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id")] AssociateTutor associateTutor)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(associateTutor).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(associateTutor);
        }

        // GET: AssociateTutors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssociateTutor associateTutor = _db.AssociateTutors.Find(id);
            if (associateTutor == null)
            {
                return HttpNotFound();
            }
            using (var context = new SenecaContext())
            {
                var sqlString = "SELECT Tutor_Id FROM AssociateTutor WHERE Id = " + associateTutor.Id;
                var tutorId = context.Database.SqlQuery<string>(sqlString).FirstOrDefault();
                associateTutor.Tutor = _db.Users.Find(tutorId);

                sqlString = "SELECT Student_Id FROM AssociateTutor WHERE Id = " + associateTutor.Id;
                var studentId = context.Database.SqlQuery<int>(sqlString).FirstOrDefault();
                associateTutor.Student = _db.Students.Find(studentId);
            }
            return View(associateTutor);
        }

        // POST: AssociateTutors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AssociateTutor associateTutor = _db.AssociateTutors.Find(id);
            _db.AssociateTutors.Remove(associateTutor);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult GetStudents(string id)
        {
            var allStudents = _db.Students.OrderBy(s => s.FirstName).ToList();
            var students = new List<Student>();  // Don't include student(s) for whom this tutor is already PrimaryTutor
                    // Or for whom this tutor is already Associate Tutor.
            foreach (Student student in allStudents)
            {
                // Check Associate Tutors for duplication:
                using (var context = new SenecaContext())
                {
                    var sqlString = "SELECT * FROM AssociateTutor WHERE Student_Id = " + student.Id;
                    sqlString += " AND Tutor_Id = '" + id + "'";
                    AssociateTutor existingAssociateTutor = null;
                    try
                    {
                        existingAssociateTutor = context.Database.SqlQuery<AssociateTutor>(sqlString).Single();
                    }
                    catch (Exception ex)
                    {
                        var x = ex;
                        existingAssociateTutor = null;
                    }

                    if (existingAssociateTutor == null)
                    { 
                        if (student.PrimaryTutor == null)
                        {
                            students.Add(student);
                        }
                        else
                        {
                            if (student.PrimaryTutor.Id != id)
                            {
                                students.Add(student);
                            }
                        }
                    }
                }
            }            
            String json = JsonConvert.SerializeObject(students, Formatting.Indented);
            return Content(json, "application/json");
        }
        public ActionResult ReturnToDashboard()
        {
            return RedirectToAction("Index", "Home");
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
