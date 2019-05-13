using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.EntityModels;

namespace MVC5_Seneca.Controllers
{
    public class HfedLocationsController : Controller
    {
        private SenecaContext db = new SenecaContext();

        // GET: HfedLocations
        public ActionResult Index()
        {
            return View(db.HfedLocations.ToList());
        }

        // GET: HfedLocations/Create
        public ActionResult Create()
        { 
            return View();
        }

        // POST: HfedLocations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Address,Location,MainPhone,LocationNote")] HfedLocation hfedLocation)
        {
            if (ModelState.IsValid)
            {
                db.HfedLocations.Add(hfedLocation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hfedLocation);
        }

        // GET: HfedLocations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HfedLocation hfedLocation = db.HfedLocations.Find(id);
            if (hfedLocation == null)
            {
                return HttpNotFound();
            }
            return View(hfedLocation);
        }

        // POST: HfedLocations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Address,MainPhone,LocationNote")] HfedLocation hfedLocation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hfedLocation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hfedLocation);
        }

        // GET: HfedLocations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HfedLocation hfedLocation = db.HfedLocations.Find(id);
            if (hfedLocation == null)
            {
                return HttpNotFound();
            }
            return View(hfedLocation);
        }

        // POST: HfedLocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HfedLocation hfedLocation = db.HfedLocations.Find(id);
            if (hfedLocation != null) db.HfedLocations.Remove(hfedLocation);
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
