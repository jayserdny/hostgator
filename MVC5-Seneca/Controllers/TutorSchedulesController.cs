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
using MVC5_Seneca.ViewModels;

namespace MVC5_Seneca.Controllers
{
    public class TutorSchedulesController : Controller
    {
        private SenecaContext _db = new SenecaContext();

        // GET: TutorSchedules
        public ActionResult Index()
        {
            var viewModel = new List<TutorSchedule>();
            foreach (TutorSchedule tutorSchedule in _db.TutorSchedules)
            {
                viewModel.Add(tutorSchedule);
            }
            return View(viewModel);
        }

        // GET: TutorSchedules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TutorSchedule tutorSchedule = _db.TutorSchedules.Find(id);
            if (tutorSchedule == null)
            {
                return HttpNotFound();
            }
            return View(tutorSchedule);
        }

        // GET: TutorSchedules/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TutorSchedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DayName,TimeOfDay")] TutorSchedule tutorSchedule)
        {
            if (ModelState.IsValid)
            {
                _db.TutorSchedules.Add(tutorSchedule);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tutorSchedule);
        }

        // GET: TutorSchedules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TutorSchedule tutorSchedule = _db.TutorSchedules.Find(id);
            if (tutorSchedule == null)
            {
                return HttpNotFound();
            }
            return View(tutorSchedule);
        }

        // POST: TutorSchedules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DayName,TimeOfDay")] TutorSchedule tutorSchedule)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(tutorSchedule).State = EntityState.Modified;
               _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tutorSchedule);
        }

        // GET: TutorSchedules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TutorSchedule tutorSchedule = _db.TutorSchedules.Find(id);
            if (tutorSchedule == null)
            {
                return HttpNotFound();
            }
            return View(tutorSchedule);
        }

        // POST: TutorSchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TutorSchedule tutorSchedule = _db.TutorSchedules.Find(id);
            _db.TutorSchedules.Remove(tutorSchedule);
            _db.SaveChanges();
            return RedirectToAction("Index");
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
