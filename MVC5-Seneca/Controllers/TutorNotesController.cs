using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.EntityModels;
using MVC5_Seneca.ViewModels;
using Newtonsoft.Json;

namespace MVC5_Seneca.Controllers
{
    public class TutorNotesController : Controller
    {
        private SenecaContext db = new SenecaContext();

        // GET: TutorNotes
        public ActionResult Index() => View(db.TutorNotes.ToList());

        // GET: TutorNotes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tutorNote = db.TutorNotes.Find(id);
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
                    userList.Add(new SelectListItem { Text = user.FirstName + " " + user.LastName, Value = user.Id, Selected = true });
                else
                    userList.Add(new SelectListItem { Text = user.FirstName + " " + user.LastName, Value = user.Id, Selected = false });
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
            var tutorNote = db.TutorNotes.Find(id);
            db.TutorNotes.Remove(tutorNote);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
                                                                                                                                                                                                                               
        public ActionResult SaveTutorNote(int studentId, DateTime date, string sessionNote) 
        {
            if (sessionNote.Length != 0)
            {
                var userId = User.Identity.GetUserId();
                var tutorNote = new TutorNote
                {
                    Date = date,
                    SessionNote = sessionNote,
                    ApplicationUser = (from u in db.Users where u.Id == userId select u).Single(),
                    Student = (from s in db.Students where s.Id == studentId select s).Single()
                };
                db.TutorNotes.Add(tutorNote);
                db.SaveChanges();
                String json = JsonConvert.SerializeObject(tutorNote, Formatting.Indented);
                return Content(json, "application/json");
            }
            else
            {
                String json = null;
                return Content(json, "application/json");
            };
               
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

            //var studentId = tutorNote.Student.Id;   // keep this in case we delete the note
     
            if (sessionNote.Length == 0)
            { // delete this note
                db.TutorNotes.Remove(tutorNote);
                db.SaveChanges();
                // create a new empty note, because Ajax Error: not hit.
                var note = new TutorNote
                {
                    Id = 0
                };
                String json = JsonConvert.SerializeObject(note, Formatting.Indented);
                return Content(json, "application/json");
            } 
            else                   
            {
                tutorNote.SessionNote = sessionNote;
                db.SaveChanges();
                String json = JsonConvert.SerializeObject(tutorNote, Formatting.Indented);
                return Content(json, "application/json");
            } 
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
            var note = db.TutorNotes.Find(id);
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
