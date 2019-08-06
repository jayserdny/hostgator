using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.EntityModels;

namespace MVC5_Seneca.Controllers
{
    public class HfedProvidersController : Controller
    {
        private SenecaContext db = new SenecaContext();

        // GET: HfedProviders
        public ActionResult Index()
        {
            return View(db.HfedProviders.OrderBy(n =>n.Name).ToList());
        }                                                          

        // GET: HfedProviders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HfedProviders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Address,MainPhone,Fax,Email,ContactName,ContactEmail,ContactPhone,BoxWeight,ProviderNote")] HfedProvider hfedProvider)
        {
            if (ModelState.IsValid)
            {    
                db.HfedProviders.Add(hfedProvider);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hfedProvider);
        }

        // GET: HfedProviders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HfedProvider hfedProvider = db.HfedProviders.Find(id);
            if (hfedProvider == null)
            {
                return HttpNotFound();
            }
            return View(hfedProvider);
        }

        // POST: HfedProviders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Address,MainPhone,Fax,Email,ContactName,ContactEmail,ContactPhone,BoxWeight,ProviderNote")] HfedProvider hfedProvider)
        {
            if (ModelState.IsValid)
            { 
                db.Entry(hfedProvider).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hfedProvider);
        }

        // GET: HfedProviders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HfedProvider hfedProvider = db.HfedProviders.Find(id);
            if (hfedProvider == null)
            {
                return HttpNotFound();
            }
            return View(hfedProvider);
        }

        // POST: HfedProviders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HfedProvider hfedProvider = db.HfedProviders.Find(id);
            db.HfedProviders.Remove(hfedProvider);
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
