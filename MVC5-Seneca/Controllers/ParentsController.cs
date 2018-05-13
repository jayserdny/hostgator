﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using Microsoft.WindowsAzure.Storage;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.EntityModels;
using MVC5_Seneca.ViewModels;

namespace MVC5_Seneca.Controllers
{
    public class ParentsController : Controller
    {
        private readonly SenecaContext _db = new SenecaContext();

        // GET: Parents
        public ActionResult Index()
        {                                                      
            return View(_db.Parents.ToList());
        }

        // GET: Parents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parent parent = _db.Parents.Find(id);
            if (parent == null)
            {
                return HttpNotFound();
            }
            return View(parent);
        }

        // GET: Parents/Create
        public ActionResult Create()
        {
            var viewModel = new AddEditParentViewModel {SelectedMotherFather = "M"}; // M is default 
             var staffRoleId = (from r in _db.Roles where (r.Name == "Staff") select r.Id).Single();
            List<SelectListItem> staffList = new List<SelectListItem>();
            var sortedUsers = _db.Users.OrderBy(u =>u.LastName).ToList();
            foreach (var user in sortedUsers)
            {
                foreach (var role in user.Roles)
                {
                    if (role.RoleId == staffRoleId)
                    {
                        staffList.Add(new SelectListItem
                        {
                            Text = user.LastName + @", " + user.FirstName,
                            Value = user.Id
                        });
                    }
                }
            }

            viewModel.StaffMembers = staffList;   
            return View(viewModel);
        }

        // POST: Parents/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MotherFather,FirstName,Address,HomePhone,CellPhone,Email,SelectedMotherFather,CaseManager")] AddEditParentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var parent = _db.Parents.Create();
                parent.MotherFather = model.SelectedMotherFather;
                parent.FirstName = model.FirstName;
                parent.Email = model.Email;
                parent.Address = model.Address;
                parent.HomePhone = model.HomePhone;
                parent.CellPhone = model.CellPhone;
                if (model.CaseManager != null)
                {
                    parent.CaseManager = _db.Users.Find(model.CaseManager.Id);
                }
                _db.Parents.Add(parent);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Parents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parent parent = _db.Parents.Find(id);
            if (parent == null)
            {
                return HttpNotFound();
            }

            var viewModel = new AddEditParentViewModel
            {
                Id = parent.Id,
                Address = parent.Address,
                CellPhone = parent.CellPhone,
                Email = parent.Email,
                FirstName = parent.FirstName,
                HomePhone = parent.HomePhone,
                SelectedMotherFather = parent.MotherFather
            };

        List<SelectListItem> staffList = new List<SelectListItem>();
            var sortedUsers = _db.Users.OrderBy(u => u.LastName).ThenBy(u => u.FirstName).ToList();
            staffList.Add(new SelectListItem { Text = @" (none)", Value = "0", Selected = false });
            var staffRoleId = (from r in _db.Roles where (r.Name == "Staff") select r.Id).Single();
            foreach (var user in sortedUsers)
                foreach (var role in user.Roles)
                    if (role.RoleId == staffRoleId)
                        if (parent.CaseManager == null)
                        {
                            staffList.Add(new SelectListItem { Text = user.FirstName + @" " + user.LastName, Value = user.Id, Selected = false });
                        }
                        else
                        {
                            if (user.Id == parent.CaseManager.Id)
                                staffList.Add(new SelectListItem { Text = user.FirstName + @" " + user.LastName, Value = user.Id, Selected = true });
                            else
                                staffList.Add(new SelectListItem { Text = user.FirstName + @" " + user.LastName, Value = user.Id, Selected = false });
                        }   
        
            viewModel.StaffMembers = staffList;
            return View(viewModel);
        }

        // POST: Parents/Edit/5                                                             
        [HttpPost]
        [ValidateAntiForgeryToken, MethodImpl(MethodImplOptions.NoOptimization)]
        public ActionResult Edit([Bind(Include = "Id,MotherFather,FirstName,Address,HomePhone,CellPhone,Email,SelectedMotherFather,CaseManager")] AddEditParentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var sqlString = "UPDATE Parent Set ";
                sqlString += "FirstName = '" + viewModel.FirstName + "',";
                sqlString += "Address = '" + viewModel.Address + "',";
                sqlString += "HomePhone = '" + viewModel.HomePhone + "',";
                sqlString += "CellPhone = '" + viewModel.CellPhone + "',";
                sqlString += "Email = '" + viewModel.Email + "',";
                sqlString += "MotherFather = '" + viewModel.SelectedMotherFather + "',";
                if (!string.IsNullOrEmpty(viewModel.CaseManager.Id))
                {
                    sqlString += "CaseManager_Id ='" + viewModel.CaseManager.Id + "'";
                }
                else
                {
                    sqlString += "CaseManager_Id = NULL";
                } 
                sqlString += " WHERE Id =" + viewModel.Id; 
                using (var context = new SenecaContext())
                {
                    context.Database.ExecuteSqlCommand(sqlString);
                }

                //var caseManager = _db.StaffMembers.Find(viewModel.StaffMember.Id);
                //var parent = _db.Parents.Find(viewModel.Id);
                //if (parent != null  )
                //{
                //    parent.FirstName = viewModel.FirstName;
                //    parent.Address = viewModel.Address;
                //    parent.HomePhone = viewModel.HomePhone;
                //    parent.CellPhone = viewModel.CellPhone;
                //    parent.Email = viewModel.Email;
                //    parent.MotherFather = viewModel.SelectedMotherFather;
                                                                                                                                          
                // //Does not get hit unless breakpointed:
                // parent.SetCaseManager(caseManager);

                //if (viewModel.StaffMember.Id == 0)
                //{
                //    // Does not get hit unless breakpointed:
                //    parent.CaseManager = null;
                //}
                //else
                //{
                //    parent.CaseManager = (from s in _db.StaffMembers where s.Id == viewModel.StaffMember.Id select s).Single();
                //}
                //_db.SaveChanges();
                // } 

                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        // GET: Parents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var parent = _db.Parents.Find(id);
            if (parent == null)
            {
                return HttpNotFound();
            }
            var deleteParent = new AddEditParentViewModel
            {
                Id = parent.Id,                           
                MotherFather = parent.MotherFather,
                FirstName = parent.FirstName,
                Address=parent.Address,
                HomePhone = parent.HomePhone,
                CellPhone = parent.CellPhone,              
                Email = parent.Email
            };
            var students = _db.Students.Where(s => s.Parent.Id == parent.Id).ToList();
            if (students.Count > 0)
            {
                deleteParent.ErrorMessage = "This will result in the permanant deletion of student(s) ";
                var firstStudent = true;
                foreach (var student in students)
                {
                    if (firstStudent == false)
                    {
                        deleteParent.ErrorMessage += " AND ";        
                    }

                    deleteParent.ErrorMessage += student.FirstName;
                    firstStudent = false;
                }
                deleteParent.ErrorMessage += " AND all their reports and session notes!";
            }

            return View(deleteParent);
        }

        // POST: Parents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var students = _db.Students.Where(s => s.Parent.Id == id).ToList();
            if (students.Count > 0)
            {
                foreach (var child in students)
                {
                    var storageAccount = CloudStorageAccount.Parse(Properties.Settings.Default.StorageConnectionString);
                    var blobClient = storageAccount.CreateCloudBlobClient();
                    var container = blobClient.GetContainerReference("studentreports");
                    foreach (var report in _db.StudentReports.Where(r => r.Student.Id == child.Id))
                    {
                        var blob = container.GetBlockBlobReference(report.DocumentLink);
                        if (blob.Exists())
                        {
                            blob.Delete();                                                              
                        }
                    }
                    _db.StudentReports.RemoveRange(_db.StudentReports.Where(r => r.Student.Id == child.Id));
                    _db.TutorNotes.RemoveRange(_db.TutorNotes.Where(t => t.Student.Id == child.Id));
                    var student = _db.Students.Find(child.Id);
                    if (student != null) _db.Students.Remove(student);
                    _db.SaveChanges();
                }
            }  
          
            Parent parent = _db.Parents.Find(id);
            if (parent != null) _db.Parents.Remove(parent);
            _db.SaveChanges();
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
  
    }

}
