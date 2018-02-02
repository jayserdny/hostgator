using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.EntityModels;
using MVC5_Seneca.ViewModels;

namespace MVC5_Seneca.Controllers
{
    public class ParentsController : Controller
    {
        private SenecaContext db = new SenecaContext();

        // GET: Parents
        public ActionResult Index()
        {
            return View(db.Parents.ToList());
        }

        // GET: Parents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parent parent = db.Parents.Find(id);
            if (parent == null)
            {
                return HttpNotFound();
            }
            return View(parent);
        }

        // GET: Parents/Create
        public ActionResult Create()
        {
            AddEditParentViewModel model = new AddEditParentViewModel
            {
                SelectedMotherFather = "M"  // default
            };
            return View(model);
        }

        // POST: Parents/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MotherFather,FirstName,Address,HomePhone,CellPhone,Email,SelectedMotherFather")] AddEditParentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var parent = db.Parents.Create();
                parent.MotherFather = model.SelectedMotherFather;
                parent.FirstName = model.FirstName;
                parent.Email = model.Email;
                parent.Address = model.Address;
                parent.HomePhone = model.HomePhone;
                parent.CellPhone = model.CellPhone;
             
                db.Parents.Add(parent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }

        // GET: Parents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parent parent = db.Parents.Find(id);
            if (parent == null)
            {
                return HttpNotFound();
            }

            var viewModel = new AddEditParentViewModel
            {
                Id = parent.Id,
                Address = parent.Address,
                CellPhone = parent.CellPhone,
                Email = parent.Email,
                FirstName = parent.FirstName,
                HomePhone = parent.HomePhone,
                SelectedMotherFather = parent.MotherFather
            };
            return View(viewModel);
        }

        // POST: Parents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MotherFather,FirstName,Address,HomePhone,CellPhone,Email,SelectedMotherFather")] AddEditParentViewModel viewModel)
        {
            if (ModelState.IsValid) 
            {
                var parent = db.Parents.Find(viewModel.Id);
                parent.FirstName = viewModel.FirstName;
                parent.Address = viewModel.Address;
                parent.HomePhone = viewModel.HomePhone;
                parent.CellPhone = viewModel.CellPhone;
                parent.Email = viewModel.Email;
                parent.MotherFather = viewModel.SelectedMotherFather;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        // GET: Parents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parent parent = db.Parents.Find(id);
            if (parent == null)
            {
                return HttpNotFound();
            }
            return View(parent);
        }

        // POST: Parents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Parent parent = db.Parents.Find(id);
            db.Parents.Remove(parent);
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
