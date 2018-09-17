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
        private readonly SenecaContext _db = new SenecaContext();

        // GET: TutorNotes
        public ActionResult Index() => View(_db.TutorNotes.ToList());

        // GET: TutorNotes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tutorNote = _db.TutorNotes.Find(id);
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
            TutorNote tutorNote = _db.TutorNotes.Find(id);
            if (tutorNote == null)
            {
                return HttpNotFound();
            }

            var viewModel = new AddEditTutorNoteViewModel();
            List<SelectListItem> userList = new List<SelectListItem>();
            foreach (ApplicationUser user in _db.Users)
            {
                if (user.UserName == tutorNote.ApplicationUser.UserName)
                    userList.Add(new SelectListItem { Text = user.FirstName + @" " + user.LastName, Value = user.Id, Selected = true });
                else
                    userList.Add(new SelectListItem { Text = user.FirstName + @" " + user.LastName, Value = user.Id, Selected = false });
            } 

            List<SelectListItem> studentList = new List<SelectListItem>();
            foreach (Student student in _db.Students)
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
                var tutorNote = _db.TutorNotes.Find(viewModel.Id);
                if (tutorNote != null)
                {
                    tutorNote.Date = viewModel.Date;
                    tutorNote.SessionNote = viewModel.SessionNote;
                    tutorNote.Student = (from s in _db.Students where s.Id == viewModel.Student.Id select s).Single();
                    tutorNote.ApplicationUser = (from u in _db.Users where u.Id == viewModel.User.Id select u).Single();
                }

                _db.SaveChanges();
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
            TutorNote tutorNote = _db.TutorNotes.Find(id);
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
            var tutorNote = _db.TutorNotes.Find(id);
            if (tutorNote != null) _db.TutorNotes.Remove(tutorNote);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
                                                                                                                                                                                                                               
        public ActionResult SaveTutorNote(int studentId, DateTime date, string sessionNote) 
        {                                                                                                                                                       
            if (sessionNote.Length != 0)
            {
                var userId = User.Identity.GetUserId();
                var tutorNote = new TutorNote
                {
                    Date =date.AddHours(5),
                    SessionNote = sessionNote,
                    ApplicationUser = (from u in _db.Users where u.Id == userId select u).Single(),
                    Student = (from s in _db.Students where s.Id == studentId select s).Single()
                };
                _db.TutorNotes.Add(tutorNote);
                _db.SaveChanges();
                String json = JsonConvert.SerializeObject(tutorNote, Formatting.Indented);
                return Content(json, "application/json");
            }
            else
            {
                return Content(null, "application/json");
            }
        }

        public ActionResult EditTutorSessionNote(int? id, string sessionNote)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TutorNote tutorNote = _db.TutorNotes.Find(id);            
            if (tutorNote == null)
            {
                return HttpNotFound();
            }

            //var studentId = tutorNote.Student.Id;   // keep this in case we delete the note
     
            if (sessionNote.Length == 0)
            { // delete this note
                _db.TutorNotes.Remove(tutorNote);
                _db.SaveChanges();
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
                _db.SaveChanges();
                String json = JsonConvert.SerializeObject(tutorNote, Formatting.Indented);
                return Content(json, "application/json");
            } 
        }

        public ActionResult GetTutorComments(int id /* Student */)
        {
            var comments = (from t in _db.TutorNotes orderby t.Date descending where t.Student.Id == id select t).ToList();
            try
            {
                String json = JsonConvert.SerializeObject(comments, Formatting.Indented);   

                return Content(json, "application/json");
            }
            catch (Exception)
            {
                return null;
            }
        }
        public ActionResult GetTutorNote(int? id)
        {
            var note = _db.TutorNotes.Find(id);
            if (note != null)
            {    
                note.UpdateAllowed = false;
                if (User.IsInRole("Administrator") || User.IsInRole("Tutor"))
                {
                    note.UpdateAllowed = true;
                }

                String json = JsonConvert.SerializeObject(note, Formatting.Indented);
                return Content(json, "application/json");
            }
            return null;
        } 
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult ReturnToDashboard()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
