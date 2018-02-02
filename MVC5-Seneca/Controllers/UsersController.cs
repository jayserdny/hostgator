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

namespace MVC5_Seneca.Controllers
{
    public class UsersController : Controller
    {
        SenecaContext db = new SenecaContext();

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        //    // GET: Users/Details/5
        //    //public ActionResult Details(int? id)
        //    //{
        //    //    if (id == null)
        //    //    {
        //    //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    //    }
        //    //    ApplicationIdentity user = db.Users.Find(id);
        //    //    if (user == null)
        //    //    {
        //    //        return HttpNotFound();
        //    //    }
        //    //    return View(user);
        //    //}

        // GET: Users/Edit/5
        public ActionResult Edit(String userName)
        {
            if (userName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = (from u in db.Users.Where(u => u.UserName == userName) select u).Single();
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserName,Active,FirstName,LastName,PhoneNumber,Email")] ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser saveUser = (from u in db.Users.Where(u => u.UserName == user.UserName) select u).Single();
                saveUser.UserName = user.UserName;   
                saveUser.FirstName = user.FirstName;
                saveUser.LastName = user.LastName;
                saveUser.PhoneNumber = user.PhoneNumber;
                saveUser.Email = user.Email;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(String userName)
        {
            if (userName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser user = (from u in db.Users.Where(u => u.UserName == userName) select u).Single();
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(String userName)
        {
            ApplicationUser user = (from u in db.Users.Where(u => u.UserName == userName) select u).Single();
            db.Users.Remove(user);
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
