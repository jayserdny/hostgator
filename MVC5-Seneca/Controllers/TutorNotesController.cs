using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.EntityModels;
using MVC5_Seneca.Models;
using MVC5_Seneca.ViewModels;
using Newtonsoft.Json;

namespace MVC5_Seneca.Controllers 
{
    public class TutorNotesController : Controller
    {
        private SenecaContext db = new SenecaContext();

        // GET: TutorNotes
        public ActionResult Index()
        {
            return View(db.TutorNotes.ToList());
        }

        // GET: TutorNotes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TutorNote tutorNote = db.TutorNotes.Find(id);
            if (tutorNote == null)
            {
                return HttpNotFound();
            }
            return View(tutorNote);
        }   

        // GET: TutorNotes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TutorNote tutorNote = db.TutorNotes.Find(id);
            if (tutorNote == null)
            {
                return HttpNotFound();
            }

            var viewModel = new AddEditTutorNoteViewModel();
            List<SelectListItem> userList = new List<SelectListItem>();
            foreach (ApplicationUser user in db.Users)
            {
                if (user.UserName == tutorNote.ApplicationUser.UserName)
                    userList.Add(new SelectListItem { Text = user.FirstName + " " + user.LastName, Value = user.Id.ToString(), Selected = true });
                else
                    userList.Add(new SelectListItem { Text = user.FirstName + " " + user.LastName, Value = user.Id.ToString(), Selected = false });
            } 

            List<SelectListItem> studentList = new List<SelectListItem>();
            foreach (Student student in db.Students)
            {
                if (student.Id == tutorNote.Student.Id)
                    studentList.Add(new SelectListItem { Text = student.FirstName, Value = student.Id.ToString(), Selected = true });
                else
                    studentList.Add(new SelectListItem { Text = student.FirstName, Value = student.Id.ToString(), Selected = false });
            }
            viewModel.Id = tutorNote.Id;
            viewModel.Date = tutorNote.Date;
            //viewModel.User = tutorNote.ApplicationUser;
            viewModel.Users = userList;
            viewModel.Students = studentList;
            return View(viewModel);
        }

        // POST: TutorNotes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,User,Student,Date,SessionNote")] AddEditTutorNoteViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var tutorNote = db.TutorNotes.Find(viewModel.Id);
                tutorNote.Date = viewModel.Date;
                tutorNote.SessionNote = viewModel.SessionNote;
                tutorNote.Student = (from s in db.Students where s.Id == viewModel.Student.Id select s).Single();
                tutorNote.ApplicationUser = (from u in db.Users where u.Id == viewModel.User.Id select u).Single();                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        // GET: TutorNotes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TutorNote tutorNote = db.TutorNotes.Find(id);
            if (tutorNote == null)
            {
                return HttpNotFound();
            }
            return View(tutorNote);
        }

        // POST: TutorNotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TutorNote tutorNote = db.TutorNotes.Find(id);
            db.TutorNotes.Remove(tutorNote);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //public ActionResult SaveTutorNote([Bind(Include = "User_Id,Student_Id,Date,SessionNote")]  AddEditTutorNoteViewModel viewModel)
        public ActionResult SaveTutorNote(int Student_Id, DateTime Date, string SessionNote) // AddEditTutorNoteViewModel viewModel)
        {
            string userId = User.Identity.GetUserId();
            var tutorNote = new TutorNote
            {
                Date = Date,
                SessionNote = SessionNote,
                ApplicationUser = (from u in db.Users where u.Id == userId select u).Single(),
                Student = (from s in db.Students where s.Id == Student_Id select s).Single()
            };
            db.TutorNotes.Add(tutorNote);
            db.SaveChanges();

            AddEditTutorNoteViewModel note = new AddEditTutorNoteViewModel
            {
                Id = tutorNote.Id,
                User = tutorNote.ApplicationUser,
                SessionNote = tutorNote.SessionNote,
                TutorNotes = (from t in db.TutorNotes where t.Student.Id == tutorNote.Student.Id orderby t.Date descending select t).ToList()
            };

            String json = JsonConvert.SerializeObject(note, Formatting.Indented);
 
            return Content(json, "application/json");
        }

        public ActionResult EditTutorSessionNote(int? id, string sessionNote)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TutorNote tutorNote = db.TutorNotes.Find(id);
            if (tutorNote == null)
            {
                return HttpNotFound();
            }
            tutorNote.SessionNote = sessionNote;
            db.SaveChanges();

            AddEditTutorNoteViewModel note = new AddEditTutorNoteViewModel
            {
                Id = tutorNote.Id,
                SessionNote = tutorNote.SessionNote,
                TutorNotes = (from t in db.TutorNotes where t.Student.Id == tutorNote.Student.Id orderby t.Date descending select t).ToList()
            };
            String json = JsonConvert.SerializeObject(note, Formatting.Indented);

            return Content(json, "application/json");
        }

        public ActionResult GetTutorComments(int id /* Student */)
        {
            var comments = (from t in db.TutorNotes orderby t.Date descending where t.Student.Id == id select t).ToList();
            try
            {
                String json = JsonConvert.SerializeObject(comments, Formatting.Indented);
                //Debug.WriteLine(json);

                return Content(json, "application/json");
            }
            catch (Exception)
            {
                return null;
            }
        }
        public ActionResult GetTutorNote(int? id)
        {
            TutorNote note = db.TutorNotes.Find(id);
            if (note != null)
            {
                String json = JsonConvert.SerializeObject(note, Formatting.Indented);
                return Content(json, "application/json");
            }
            return null;
        } 
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult ReturnToDashboard()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
