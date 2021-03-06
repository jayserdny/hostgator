﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.EntityModels;
using MVC5_Seneca.ViewModels;             
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Blob; // Namespace for Blob storage types;
using MVC5_Seneca.Properties;

namespace MVC5_Seneca.Controllers
{
    public class StudentReportsController : Controller
    {
        private readonly SenecaContext _db = new SenecaContext();

        // GET: StudentReports
        public ActionResult Index()
        {
            if (User.IsInRole("Administrator"))
            {
                return View(_db.StudentReports.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");   // Dashboard
            }
        }

        // GET: StudentReports/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentReport studentReport = _db.StudentReports.Find(id);
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
            foreach (Student student in _db.Students)
            {
                studentList.Add(new SelectListItem { Text = student.FirstName, Value = student.Id.ToString() });
            }
            model.Students = studentList.OrderBy(s => s.Text);

            List<SelectListItem> documentTypeList = new List<SelectListItem>();
            foreach (DocumentType documentType in _db.DocumentTypes)
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
                StudentReport studentReport = new StudentReport
                {
                    DocumentDate = model.DocumentDate,
                    Comments = model.Comments,
                    DocumentLink = Settings.Default.DocumentStoragePath + model.DocumentLink,
                    DocumentType = (from d in _db.DocumentTypes where d.Id == model.DocumentType.Id select d).Single(),
                    Student = (from s in _db.Students where s.Id == model.Student.Id select s).Single()
                };

                _db.StudentReports.Add(studentReport);
                _db.SaveChanges();
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
            StudentReport studentReport = _db.StudentReports.Find(id);
            if (studentReport == null)
            {
                return HttpNotFound();
            }

            var viewModel = new AddEditStudentReportViewModel();

            List<SelectListItem> studentList = new List<SelectListItem>();
            foreach (Student student in _db.Students)
            {
                if (studentReport.Student.Id == student.Id)
                    studentList.Add(new SelectListItem { Text = student.FirstName, Value = student.Id.ToString(), Selected = true });
                else
                    studentList.Add(new SelectListItem { Text = student.FirstName, Value = student.Id.ToString(), Selected = false });
            }

            List<SelectListItem> documentTypeList = new List<SelectListItem>();
            foreach (DocumentType documentType in _db.DocumentTypes)
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
        public ActionResult Edit([Bind(Include = "Id,Student,DocumentDate,DocumentType,DocumentLink,Comments")] AddEditStudentReportViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var studentReport = _db.StudentReports.Find(viewModel.Id);
                if (studentReport != null)
                {
                    studentReport.DocumentDate = viewModel.DocumentDate;
                    studentReport.DocumentType = viewModel.DocumentType;
                    studentReport.Comments = viewModel.Comments;
                    studentReport.DocumentLink = viewModel.DocumentLink;
                    studentReport.Student =
                        (from s in _db.Students where s.Id == viewModel.Student.Id select s).Single();
                    studentReport.DocumentType =
                        (from d in _db.DocumentTypes where d.Id == viewModel.DocumentType.Id select d).Single();
                }

                _db.SaveChanges();

                if (User.IsInRole("Administrator"))
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index", "Home");  // Dashboard
                }
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
            StudentReport studentReport = _db.StudentReports.Find(id);
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
            StudentReport studentReport = _db.StudentReports.Find(id);
            if (studentReport != null)
            {
                _db.StudentReports.Remove(studentReport);
                _db.SaveChanges();

                CloudStorageAccount storageAccount =
                    CloudStorageAccount.Parse(Settings.Default.StorageConnectionString);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference("studentreports");
                CloudBlockBlob blob = container.GetBlockBlobReference(studentReport.DocumentLink);
                if (blob.Exists())
                {
                    blob.Delete();
                }
            }

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
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult ViewReport(int id)
        {
            if (!Request.IsAuthenticated)
            {
                return new HttpUnauthorizedResult();   
            }

            var report = _db.StudentReports.Find(id); 
            if (report != null)
            {                                                                                                     
                return Redirect("~/StudentReportFiles/" + report.DocumentLink);      
            }
            return new HttpUnauthorizedResult();               
        }                                                     
    }
}
