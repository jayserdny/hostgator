using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.EntityModels;

namespace MVC5_Seneca.Controllers
{
    public class TipsCategoriesController : Controller
    {
        private readonly SenecaContext _db = new SenecaContext();

        // GET: TipsCategories
        public ActionResult Index()
        {
            return View(_db.TipsCategories.ToList());
        }

        // GET: TipsCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TipsCategory tipsCategory = _db.TipsCategories.Find(id);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] TipsCategory tipsCategory)
        {
            if (ModelState.IsValid)
            {
                _db.TipsCategories.Add(tipsCategory);
                _db.SaveChanges();
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
            TipsCategory tipsCategory = _db.TipsCategories.Find(id);
            if (tipsCategory == null)
            {
                return HttpNotFound();
            }
            return View(tipsCategory);
        }

        // POST: TipsCategories/Edit/5                   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] TipsCategory tipsCategory)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(tipsCategory).State = EntityState.Modified;
                _db.SaveChanges();
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
            TipsCategory tipsCategory = _db.TipsCategories.Find(id);
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
            TipsCategory tipsCategory = _db.TipsCategories.Find(id);
            _db.TipsCategories.Remove(tipsCategory);
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
