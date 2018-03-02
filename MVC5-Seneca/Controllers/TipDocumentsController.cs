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

namespace MVC5_Seneca
{
    public class TipDocumentsController : Controller
    {
        private SenecaContext db = new SenecaContext();

        // GET: TipDocuments
        public ActionResult Index()
        {
            return View(db.TipDocuments.ToList());
        }

        // GET: TipDocuments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipDocument tipDocument = db.TipDocuments.Find(id);
            if (tipDocument == null)
            {
                return HttpNotFound();
            }
            return View(tipDocument);
        }

        // GET: TipDocuments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipDocuments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,DocumentLink,User")] TipDocument tipDocument)
        {
            if (ModelState.IsValid)
            {
                db.TipDocuments.Add(tipDocument);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipDocument);
        }

        // GET: TipDocuments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipDocument tipDocument = db.TipDocuments.Find(id);
            if (tipDocument == null)
            {
                return HttpNotFound();
            }
            return View(tipDocument);
        }

        // POST: TipDocuments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,DocumentLink,User")] TipDocument tipDocument)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipDocument).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipDocument);
        }

        // GET: TipDocuments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipDocument tipDocument = db.TipDocuments.Find(id);
            if (tipDocument == null)
            {
                return HttpNotFound();
            }
            return View(tipDocument);
        }

        // POST: TipDocuments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipDocument tipDocument = db.TipDocuments.Find(id);
            db.TipDocuments.Remove(tipDocument);
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
