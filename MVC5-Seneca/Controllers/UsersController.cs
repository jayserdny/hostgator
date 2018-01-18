using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.EntityModels;

namespace MVC5_Seneca.Controllers
{
    //public class UsersController : Controller
    //{
    //    private SenecaContext db = new SenecaContext();

    //    // GET: Users
    //    public ActionResult Index()
    //    {
    //        return View(db.OldUsers.ToList());
    //    }

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

    //    // GET: Users/Create
    //    public ActionResult Create()
    //    {
    //        return View();
    //    }

    //    // POST: Users/Create
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult Create([Bind(Include = "Id,Name,Password")] UserDetail user)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            db.OldUsers.Add(user);
    //            db.SaveChanges();
    //            return RedirectToAction("Index");
    //        }

    //        return View(user);
    //    }

    //    // GET: Users/Edit/5
    //    public ActionResult Edit(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //        }
    //        UserDetail user = db.OldUsers.Find(id);
    //        if (user == null)
    //        {
    //            return HttpNotFound();
    //        }
    //        return View(user);
    //    }

    //    // POST: Users/Edit/5
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult Edit([Bind(Include = "Id,Name,PasswordSalt,PasswordHash,Active,FirstName,LastName,Address,CityStateZip,HomePhone,CellPhone,Email")] UserDetail user)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            db.Entry(user).State = EntityState.Modified;
    //            db.SaveChanges();
    //            return RedirectToAction("Index");
    //        }
    //        return View(user);
    //    }

    //    // GET: Users/Delete/5
    //    public ActionResult Delete(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //        }
    //        UserDetail user = db.OldUsers.Find(id);
    //        if (user == null)
    //        {
    //            return HttpNotFound();
    //        }
    //        return View(user);
    //    }

    //    // POST: Users/Delete/5
    //    [HttpPost, ActionName("Delete")]
    //    [ValidateAntiForgeryToken]
    //    public ActionResult DeleteConfirmed(int id)
    //    {
    //        UserDetail user = db.OldUsers.Find(id);
    //        db.OldUsers.Remove(user);
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
    //}
}
