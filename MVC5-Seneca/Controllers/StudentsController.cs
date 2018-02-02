using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.EntityModels;
using MVC5_Seneca.Models;
using MVC5_Seneca.ViewModels;

namespace MVC5_Seneca.Controllers
{
    public class StudentsController : Controller
    {
        private SenecaContext db = new SenecaContext();

        // GET: Students
        public ActionResult Index()
        {
            return View(db.Students.ToList());         
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            var viewModel = new AddEditStudentViewModel();               
            List<SelectListItem> schoolList = new List<SelectListItem>();
            foreach (School school in db.Schools)
            { 
                schoolList.Add(new SelectListItem { Text = school.Name, Value = school.Id.ToString()});            
            }
            viewModel.Schools = schoolList;

            List<SelectListItem> parentList = new List<SelectListItem>();
            foreach (Parent parent in db.Parents)
            {
               parentList.Add(new SelectListItem { Text = parent.FirstName, Value = parent.Id.ToString() });
            }
            viewModel.Parents = parentList;
            //viewModel.BirthDate = DateTime.Now.AddYears(-10);
            return View(viewModel);
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,Gender,BirthDate,School,Parent")] AddEditStudentViewModel viewModel) 
        {
            if (ModelState.IsValid && viewModel.FirstName != null)               
            {
                Student student = new Student
                {
                    FirstName = viewModel.FirstName,
                    Gender = viewModel.Gender,
                    BirthDate = viewModel.BirthDate,
                    Parent = (from p in db.Parents where p.Id == viewModel.Parent.Id select p).Single(),
                    School = (from s in db.Schools where s.Id == viewModel.School.Id select s).Single()
                };
                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }

            var viewModel = new AddEditStudentViewModel();

            List<SelectListItem> schoolList = new List<SelectListItem>();
            foreach (School school in db.Schools)
            {
                if (school.Id == student.School.Id)
                    schoolList.Add(new SelectListItem { Text = school.Name, Value = school.Id.ToString(), Selected = true });
                else
                    schoolList.Add(new SelectListItem { Text = school.Name, Value = school.Id.ToString(), Selected = false });
            }
            viewModel.Schools = schoolList;

            List<SelectListItem> parentList = new List<SelectListItem>();
            foreach (Parent parent in db.Parents)
            {
                if (parent.Id == student.Parent.Id)
                    parentList.Add(new SelectListItem { Text = parent.FirstName, Value = parent.Id.ToString(), Selected = true });
                else
                    parentList.Add(new SelectListItem { Text = parent.FirstName, Value = parent.Id.ToString(), Selected = false });
            }
            viewModel.Id = student.Id;
            viewModel.Parents = parentList;
            viewModel.FirstName = student.FirstName;
            viewModel.Gender = student.Gender;
            viewModel.BirthDate = student.BirthDate;
            viewModel.Parent = student.Parent;
            viewModel.School = student.School;
            return View(viewModel);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,Gender,BirthDate,School,Parent ")] AddEditStudentViewModel viewModel) 
        {
            if (ModelState.IsValid)
            {
                var student = db.Students.Find(viewModel.Id);                
                student.FirstName = viewModel.FirstName;
                student.Gender = viewModel.Gender;
                student.BirthDate = viewModel.BirthDate;
                student.Parent = (from p in db.Parents where p.Id == viewModel.Parent.Id select p).Single();
                student.School = (from s in db.Schools where s.Id == viewModel.School.Id select s).Single();
                db.SaveChanges();
             
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult ReturnToDashboard()
        {
            return RedirectToAction("Index", "Home");
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
