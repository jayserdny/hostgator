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
    public class HfedDriversController : Controller
    {
        private SenecaContext db = new SenecaContext();

        // GET: HfedDrivers
        public ActionResult Index()
        {
            return View(db.HfedDrivers.ToList());
        }

        // GET: HfedDrivers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HfedDrivers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Phone,Fax,Email,DriverNote")] HfedDriver hfedDriver)
        {
            if (ModelState.IsValid)
            {
                db.HfedDrivers.Add(hfedDriver);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hfedDriver);
        }

        // GET: HfedDrivers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HfedDriver hfedDriver = db.HfedDrivers.Find(id);
            if (hfedDriver == null)
            {
                return HttpNotFound();
            }
            return View(hfedDriver);
        }

        // POST: HfedDrivers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Phone,Fax,Email,DriverNote")] HfedDriver hfedDriver)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hfedDriver).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hfedDriver);
        }

        // GET: HfedDrivers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HfedDriver hfedDriver = db.HfedDrivers.Find(id);
            if (hfedDriver == null)
            {
                return HttpNotFound();
            }
            return View(hfedDriver);
        }

        // POST: HfedDrivers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HfedDriver hfedDriver = db.HfedDrivers.Find(id);
            db.HfedDrivers.Remove(hfedDriver);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ReturnToHfedDashboard()
        {
            return RedirectToAction("Index", "HfedHome");
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
