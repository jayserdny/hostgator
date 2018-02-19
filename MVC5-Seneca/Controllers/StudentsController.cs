﻿using System;
using System.Collections.Generic;
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
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace MVC5_Seneca.Controllers
{
    public class StudentsController : Controller
    {
        private SenecaContext db = new SenecaContext();

        // GET: Students
        public ActionResult Index()
        {
            return View(db.Students.OrderBy(f => f.FirstName).ToList());         
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
            var sortedParents = db.Parents.OrderBy(p => p.FirstName).ToList();
            foreach (Parent parent in sortedParents)
            {
               parentList.Add(new SelectListItem { Text = parent.FirstName, Value = parent.Id.ToString() });
            }

            List<SelectListItem> userList = new List<SelectListItem>();
            var sortedUsers = db.Users.OrderBy(u => u.LastName).ThenBy(u => u.FirstName).ToList();
            foreach (ApplicationUser user in sortedUsers)
            {
                userList.Add(new SelectListItem { Text = user.LastName + ", " + user.FirstName, Value = user.Id.ToString() });
            }

            viewModel.Parents = parentList;
            viewModel.Schools = schoolList;
            viewModel.Users = userList;
            //viewModel.BirthDate = DateTime.Now.AddYears(-10);
            return View(viewModel);
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,Gender,BirthDate,School,Parent,PrimaryTutor")] AddEditStudentViewModel viewModel) 
        {
            if (ModelState.IsValid 
                && viewModel.FirstName != null               
                && viewModel.School != null
                && viewModel.Parent != null)    
            {
                Student student = new Student
                {
                    FirstName = viewModel.FirstName,
                    Gender = viewModel.Gender,
                    BirthDate = viewModel.BirthDate,
                    Parent = (from p in db.Parents where p.Id == viewModel.Parent.Id select p).Single(),
                    School = (from s in db.Schools where s.Id == viewModel.School.Id select s).Single()                   
                };
                if (viewModel.PrimaryTutor.Id != null)
                {
                    student.PrimaryTutor = (from t in db.Users where t.Id == viewModel.PrimaryTutor.Id select t).Single();
                }

                db.Students.Add(student);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Create");
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
            var sortedParents = db.Parents.OrderBy(p => p.FirstName).ToList();
            foreach (Parent parent in sortedParents)
                if (student.Parent == null)
                {
                    parentList.Add(new SelectListItem { Text = parent.FirstName, Value = parent.Id.ToString(), Selected = false });
                }
                else
                { 
                    if (parent.Id == student.Parent.Id)
                        parentList.Add(new SelectListItem { Text = parent.FirstName, Value = parent.Id.ToString(), Selected = true });
                    else                 
                        parentList.Add(new SelectListItem { Text = parent.FirstName, Value = parent.Id.ToString(), Selected = false });
                }

            List<SelectListItem> primaryTutorList = new List<SelectListItem>
            {  
                new SelectListItem { Text = "- Select ID -", Value = "0", Selected = true }
            }; 
            var sortedUsers = db.Users.OrderBy(u => u.LastName).ThenBy(u => u.FirstName).ToList();
            foreach (ApplicationUser user in sortedUsers)
                if (student.PrimaryTutor == null)
                {
                    primaryTutorList.Add(new SelectListItem { Text = user.LastName + ", " + user.FirstName, Value = user.Id.ToString(), Selected = false });
                }
                else
                {
                    if (user.Id == student.PrimaryTutor.Id)
                        primaryTutorList.Add(new SelectListItem { Text = user.LastName + ", " + user.FirstName, Value = user.Id.ToString(), Selected = true });
                    else
                        primaryTutorList.Add(new SelectListItem { Text = user.LastName + ", " + user.FirstName, Value = user.Id.ToString(), Selected = false });
                }
            
            viewModel.Id = student.Id;
            viewModel.Parents = parentList;
            viewModel.Schools = schoolList;
            viewModel.Users = primaryTutorList;
            viewModel.FirstName = student.FirstName;
            viewModel.Gender = student.Gender;
            viewModel.BirthDate = student.BirthDate;
            viewModel.Parent = student.Parent;
            viewModel.School = student.School;
            viewModel.PrimaryTutor = student.PrimaryTutor;
            return View(viewModel);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,Gender,BirthDate,School,Parent,PrimaryTutor")] AddEditStudentViewModel viewModel) 
        {
            if (ModelState.IsValid)
            {
                var student = db.Students.Find(viewModel.Id);
                student.FirstName = viewModel.FirstName;
                student.Gender = viewModel.Gender;
                student.BirthDate = viewModel.BirthDate;
                if (viewModel.Parent != null)
                    { student.Parent = (from p in db.Parents where p.Id == viewModel.Parent.Id select p).Single(); }
                if (viewModel.School != null)
                    { student.School = (from s in db.Schools where s.Id == viewModel.School.Id select s).Single(); }
                if (viewModel.PrimaryTutor.Id != null && viewModel.PrimaryTutor.Id.ToString() != "0")
                {
                    student.PrimaryTutor = (from t in db.Users where t.Id == viewModel.PrimaryTutor.Id select t).Single();
                }
                else
                {
                    student.PrimaryTutor = null; // ENTITY FRAMEWORK WON'T ALLOW SETTING TO NULL?
                    //db.SaveChanges();         // TODO - why is this line inconsistent?
                    var sql = "UPDATE Student SET PrimaryTutor_Id = null WHERE Id = " + viewModel.Id;
                    db.Database.ExecuteSqlCommand(sql);
                    return RedirectToAction("Index");
                }
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
            // TODO check that all uploads and session notes are removed:
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(Properties.Settings.Default.StorageConnectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("studentreports");
            foreach (StudentReport report in db.StudentReports.Where(r => r.Student.Id == id))
            {
                CloudBlockBlob blob = container.GetBlockBlobReference(report.DocumentLink);
                if (blob.Exists())
                {
                    blob.Delete();
                }     
            }
            db.StudentReports.RemoveRange(db.StudentReports.Where(r => r.Student.Id == id));
            db.TutorNotes.RemoveRange(db.TutorNotes.Where(t => t.Student.Id == id));          
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
