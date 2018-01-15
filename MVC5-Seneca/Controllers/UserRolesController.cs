using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.Models;
using MVC5_Seneca.EntityModels;
using MVC5_Seneca.ViewModels;
using System.Data.Entity.Migrations;

namespace MVC5_Seneca
{
    public class UserRolesController : Controller 
    {
        private SenecaContext db = new SenecaContext();

        // GET: Roles
        public ActionResult Index()
        {
            var userRoles = db.UserRoles.ToList();
            List<AddEditUserRolesViewModel> viewModel = new List<AddEditUserRolesViewModel>();
            foreach (var userRole in userRoles)
            {
                AddEditUserRolesViewModel vm = new AddEditUserRolesViewModel();
                vm.Id = userRole.Id;
                vm.Role = userRole.Role;
                vm.User = (from u in db.Users where u.Id == userRole.User.Id select u).Single();
                viewModel.Add(vm);
            }
                return View(viewModel);
        }

        // GET: Roles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserRole role = db.UserRoles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            var viewModel = new AddEditUserRolesViewModel();
            var userRoles = Properties.Settings.Default.UserRoles;

            List<SelectListItem> Roles = new List<SelectListItem>();
            foreach (String text in userRoles)
            {
                Roles.Add(new SelectListItem() { Text = text });
            }

            List<SelectListItem> Users = new List<SelectListItem>();
            foreach (User user in db.Users.ToList()) 
            {
                Users.Add(new SelectListItem() { Text = user.FirstName + " " + user.LastName, Value = user.Id.ToString() });
            }


            viewModel.UserRoles = Roles;
            viewModel.Users = Users;

            return View(viewModel);
        }

        // POST: Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Role,User")] AddEditUserRolesViewModel model) 
        {
            //if (ModelState.IsValid)
            if(model.Role != null && model.User != null)
            {
                // complete User needed to fill required fields (otherwise SaveChanges creates a new user):
                User user = (from u in db.Users where u.Id == model.User.Id select u).Single();
                model.User = user;             

                var userRole = new UserRole();
                userRole.Role = model.Role;
                userRole.User = model.User;
                db.UserRoles.Add(userRole);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Roles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserRole role = db.UserRoles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            var viewModel = new AddEditUserRolesViewModel();
            var userRoles = Properties.Settings.Default.UserRoles;

            List<SelectListItem> Roles = new List<SelectListItem>();
            foreach (String text in userRoles)
            {
                Roles.Add(new SelectListItem() { Text = text });
            }

            List<SelectListItem> Users = new List<SelectListItem>();
            foreach (User user in db.Users.ToList())
            {
                Users.Add(new SelectListItem() { Text = user.FirstName + " " + user.LastName, Value = user.Id.ToString() });
            }    

            viewModel.UserRoles = Roles;
            viewModel.User = role.User;

            return View(viewModel);
        }

        // POST: Roles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,User,Role")] UserRole model)
        {
            if (ModelState.IsValid)
            {
                var userRole = db.UserRoles.Find(model.Id);
                userRole.Role = model.Role;                                                                                          
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Roles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserRole role = db.UserRoles.Find(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserRole role = db.UserRoles.Find(id);
            db.UserRoles.Remove(role);
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
