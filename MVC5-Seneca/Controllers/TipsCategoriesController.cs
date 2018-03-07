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
    public class TipsCategoriesController : Controller
    {
        private readonly SenecaContext db = new SenecaContext();

        // GET: TipsCategories
        public ActionResult Index()
        {
            return View(db.TipsCategories.ToList());
        }

        // GET: TipsCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipsCategory tipsCategory = db.TipsCategories.Find(id);
            if (tipsCategory == null)
            {
                return HttpNotFound();
            }
            return View(tipsCategory);
        }

        // GET: TipsCategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TipsCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] TipsCategory tipsCategory)
        {
            if (ModelState.IsValid)
            {
                db.TipsCategories.Add(tipsCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipsCategory);
        }

        // GET: TipsCategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipsCategory tipsCategory = db.TipsCategories.Find(id);
            if (tipsCategory == null)
            {
                return HttpNotFound();
            }
            return View(tipsCategory);
        }

        // POST: TipsCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] TipsCategory tipsCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipsCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipsCategory);
        }

        // GET: TipsCategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipsCategory tipsCategory = db.TipsCategories.Find(id);
            if (tipsCategory == null)
            {
                return HttpNotFound();
            }
            return View(tipsCategory);
        }

        // POST: TipsCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TipsCategory tipsCategory = db.TipsCategories.Find(id);
            db.TipsCategories.Remove(tipsCategory);
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
