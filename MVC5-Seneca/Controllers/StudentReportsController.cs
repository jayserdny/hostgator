using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.EntityModels;
using MVC5_Seneca.ViewModels;
using Newtonsoft.Json;

namespace MVC5_Seneca.Controllers
{
    public class StudentReportsController : Controller
    {
        private SenecaContext db = new SenecaContext();

        // GET: StudentReports
        public ActionResult Index()
        {
            return View(db.StudentReports.ToList());
        }

        // GET: StudentReports/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentReport studentReport = db.StudentReports.Find(id);
            if (studentReport == null)
            {
                return HttpNotFound();
            }
            return View(studentReport);
        }

        // GET: StudentReports/Create
        public ActionResult Create()
        {
            AddEditStudentReportViewModel  model = new AddEditStudentReportViewModel();
            List<SelectListItem> studentList = new List<SelectListItem>();
            foreach (Student student in db.Students)
            {
                studentList.Add(new SelectListItem { Text = student.FirstName, Value = student.Id.ToString() });
            }
            model.Students = studentList;

            List<SelectListItem> documentTypeList = new List<SelectListItem>();
            foreach (DocumentType documentType in db.DocumentTypes)
            {
                documentTypeList.Add(new SelectListItem { Text = documentType.Name, Value = documentType.Id.ToString() });
            }
            model.DocumentTypes = documentTypeList;        
            return View(model);
        }    

        // POST: StudentReports/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
               public ActionResult Create([Bind(Include = "DocumentDate,Comments,Student,DocumentType,DocumentLink,PostedFile")] AddEditStudentReportViewModel model)
        {              
            if (ModelState.IsValid)
            {
                StudentReport studentReport = new StudentReport();
                studentReport.DocumentDate = model.DocumentDate;
                studentReport.Comments = model.Comments;
                studentReport.DocumentLink = Properties.Settings.Default.DocumentStoragePath + model.DocumentLink;
                studentReport.DocumentType = (from d in db.DocumentTypes where d.Id == model.DocumentType.Id select d).Single();
                studentReport.Student = (from s in db.Students where s.Id == model.Student.Id select s).Single();

                var x = model.PostedFile.ContentLength;

                db.StudentReports.Add(studentReport);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: StudentReports/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentReport studentReport = db.StudentReports.Find(id);
            if (studentReport == null)
            {
                return HttpNotFound();
            }

            var viewModel = new AddEditStudentReportViewModel();

            List<SelectListItem> studentList = new List<SelectListItem>();
            foreach (Student student in db.Students)
            {
                if (studentReport.Student.Id == student.Id)
                    studentList.Add(new SelectListItem { Text = student.FirstName, Value = student.Id.ToString(), Selected = true });
                else
                    studentList.Add(new SelectListItem { Text = student.FirstName, Value = student.Id.ToString(), Selected = false });
            }

            List<SelectListItem> documentTypeList = new List<SelectListItem>();
            foreach (DocumentType documentType in db.DocumentTypes)
            {
                if (studentReport.DocumentType.Id == documentType.Id)
                    documentTypeList.Add(new SelectListItem { Text = documentType.Name, Value = documentType.Id.ToString(), Selected = true });
                else
                    documentTypeList.Add(new SelectListItem { Text = documentType.Name, Value = documentType.Id.ToString(), Selected = false });
            }
            viewModel.Id = studentReport.Id;
            viewModel.Student = studentReport.Student;
            viewModel.Comments = studentReport.Comments;
            viewModel.DocumentType = studentReport.DocumentType;
            viewModel.DocumentDate = studentReport.DocumentDate;
            viewModel.DocumentLink = studentReport.DocumentLink;
            viewModel.DocumentTypes = documentTypeList;
            viewModel.Students = studentList;
            return View(viewModel);
        }

        // POST: StudentReports/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Student,DocumentDate,DocumentType,DocumentLink,Comments")] StudentReport viewModel)
        {
            if (ModelState.IsValid)
            {
                var studentReport = db.StudentReports.Find(viewModel.Id);
                studentReport.DocumentDate = viewModel.DocumentDate;
                studentReport.DocumentType = viewModel.DocumentType;
                studentReport.Comments = viewModel.Comments;
                studentReport.DocumentLink = viewModel.DocumentLink;
                studentReport.Student = (from s in db.Students where s.Id == viewModel.Student.Id select s).Single();
                studentReport.DocumentType = (from d in db.DocumentTypes where d.Id == viewModel.DocumentType.Id select d).Single();
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(viewModel); 
        }

        // GET: StudentReports/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentReport studentReport = db.StudentReports.Find(id);
            if (studentReport == null)
            {
                return HttpNotFound();
            }
            return View(studentReport);
        }

        // POST: StudentReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            StudentReport studentReport = db.StudentReports.Find(id);
            db.StudentReports.Remove(studentReport);
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
