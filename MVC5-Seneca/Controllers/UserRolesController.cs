using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.EntityModels;
using MVC5_Seneca.ViewModels;

namespace MVC5_Seneca.Controllers
{
    public class UserRolesController : Controller
    {
        private SenecaContext _db = new SenecaContext();

        // GET: Roles
        public ActionResult Index()
        {  
            AddEditUserRolesViewModel model = new AddEditUserRolesViewModel
            {
                UserNameRoles = new List<UserNameRole>()
            };

            var users = _db.Users.OrderBy(u => u.LastName).ToList();
            foreach (var user in users)
            {
                foreach (var role in user.Roles)
                {
                    var _role = (from r in _db.Roles where (r.Id == role.RoleId) select r).Single();
                    UserNameRole userNameRole = new UserNameRole
                    {
                        Name = user.UserName + ": " + user.FirstName + " "
                                + user.LastName + " / " + _role.Name,
                        Id = user.Id + "|" + _role.Id,
                        Email = user.Email
                    };
                    model.UserNameRoles.Add(userNameRole);
                }
            }
            return View(model);
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            AddEditUserRolesViewModel viewModel = new AddEditUserRolesViewModel();

            var userRoles = (from r in _db.Roles select r).ToList();
            List<SelectListItem> Roles = new List<SelectListItem>();
            foreach (var role in userRoles)
            {
                Roles.Add(new SelectListItem() { Text = role.Name, Value = role.Id });
            }

            var users = (from u in _db.Users select u).ToList();
            List<SelectListItem> Users = new List<SelectListItem>();
            foreach (var user in users)
            {
                Users.Add(new SelectListItem() { Text = user.UserName + @" " + user.FirstName + @" " + user.LastName, Value = user.Id });
            }

            viewModel.UserRoles = Roles;
            viewModel.Users = Users;

            return View(viewModel);
        }

        // POST: Roles/Create
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AddEditUserRolesViewModel model)
        {   
            if (ModelState.IsValid)
            {
                //var roleStore = new RoleStore<IdentityRole>(_db);
                //var roleManager = new RoleManager<IdentityRole>(roleStore);

                var userStore = new UserStore<ApplicationUser>(_db);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var role = _db.Roles.Find(model.UserRole.Id);
                userManager.AddToRole(model.User.Id, role.Name);

                return RedirectToAction("Index", "UserRoles");
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }    

        // GET: Roles/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string[] splitId = id.Split('|');
            var userId = splitId[0];
            var roleId = splitId[1];
            var role = (from r in _db.Roles where (r.Id == roleId) select r).Single();
            if (role == null)
            {
                return HttpNotFound();
            }
            var model = new AddEditUserRolesViewModel();

            var user = _db.Users.Find(userId);
            model.Name = user.FirstName + " " + user.LastName;

            //List<SelectListItem> listRoles = new List<SelectListItem>();
            //var userRoles = (from r in _db.Roles select r).ToList();
            return View(model);
        }

        //    // POST: Roles/Edit/5
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public ActionResult Edit([Bind(Include = "Id,Name,User,Role")] UserRole model)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var userRole = db.UserRoles.Find(model.Id);
        //            userRole.Role = model.Role;                                                                                          
        //            db.SaveChanges();
        //            return RedirectToAction("Index");
        //        }
        //        return View(model);
        //    }

        //    // GET: Roles/Delete/5
        //    public ActionResult Delete(int? id)
        //    {
        //        if (id == null)
        //        {
        //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //        }
        //        UserRole role = db.UserRoles.Find(id);
        //        if (role == null)
        //        {
        //            return HttpNotFound();
        //        }
        //        return View(role);
        //    }

        //    // POST: Roles/Delete/5
        //    [HttpPost, ActionName("Delete")]
        //    [ValidateAntiForgeryToken]
        //    public ActionResult DeleteConfirmed(int id)
        //    {
        //        UserRole role = db.UserRoles.Find(id);
        //        db.UserRoles.Remove(role);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    public ActionResult ReturnToDashboard()
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //    protected override void Dispose(bool disposing)
        //    {
        //        if (disposing)
        //        {
        //            db.Dispose();
        //        }
        //        base.Dispose(disposing);
        //    }

        // GET: Users/Delete/5
        public ActionResult Delete(String id)   // contains userId | roleId
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            string[] splitId = id.Split('|');
            var userId = splitId[0];
            var roleId = splitId[1];
            var role = (from r in _db.Roles where (r.Id == roleId) select r).Single();
            ApplicationUser user = (from u in _db.Users.Where(u => u.Id == userId) select u).Single();
            if (user == null)
            {
                return HttpNotFound();
            }
            AddEditUserRolesViewModel model = new AddEditUserRolesViewModel
            {
                User = user,
                Name = role.Name
            };

            return View(model);
        }

        // POST: UserRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]                                                                                
        public async Task<ActionResult> DeleteConfirmed(AddEditUserRolesViewModel model)
        {
            string[] splitId = model.Id.Split('|');
            var userId = splitId[0];
            var roleId = splitId[1];
            var roles = new string [1];
            var role = _db.Roles.Find(roleId);
            roles[0] = role.Name;
                                                                                                                                                                                                 
            UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_db));
            await UserManager.RemoveFromRolesAsync(userId, roles).ConfigureAwait(false);                                                                            
            return RedirectToAction("Index");
        }
        public ActionResult ReturnToDashboard()
        {
            return RedirectToAction("Index", "Home");
        }
    }
 
}
