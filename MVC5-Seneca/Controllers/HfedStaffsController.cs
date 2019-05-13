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
    public class HfedStaffsController : Controller
    {
        private SenecaContext db = new SenecaContext();

        // GET: HfedStaffs
        public ActionResult Index()
        {
            var model = new List<HfedStaff>();
            foreach (var hfedStaff in db.HfedStaffs.ToList())
            {
                using (var context = new SenecaContext())
                {
                    var sqlString = "SELECT Location_Id FROM HfedStaff WHERE Id = " + hfedStaff.Id;
                    var locationId = context.Database.SqlQuery<Int32>(sqlString).FirstOrDefault();
                    var location = db.HfedLocations.Find(locationId);

                    HfedStaff viewModel = new HfedStaff()
                    {
                        Id = hfedStaff.Id,
                        FirstName = hfedStaff.FirstName,
                        LastName = hfedStaff.LastName,
                        Email = hfedStaff.Email,  
                        Phone = hfedStaff.Phone, 
                        Location = location,
                        StaffNote = hfedStaff.StaffNote
                    };
                    model.Add(viewModel);
                }
            }
            return View(model);
        }       

        // GET: HfedStaffs/Create
        public ActionResult Create()
        {
            HfedStaff hfedStaffView = new HfedStaff()
            {
                HfedLocations = db.HfedLocations.OrderBy(l => l.Name).ToList()
            };

            return View(hfedStaffView);
        }

        // POST: HfedStaffs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Phone,Location,Email,StaffNote")] HfedStaff hfedStaff)
        {
            if (ModelState.IsValid)
            {
                HfedLocation location = db.HfedLocations.Find(hfedStaff.Location.Id);
                HfedStaff newHfedStaff = new HfedStaff() 
                {
                    FirstName = hfedStaff.FirstName,
                    LastName = hfedStaff.LastName,
                    Phone = hfedStaff.Phone,
                    Email = hfedStaff.Email,
                    Location = location,
                    StaffNote = hfedStaff.StaffNote 
                };
                db.HfedStaffs.Add(newHfedStaff);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hfedStaff);
        }

        // GET: HfedStaffs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HfedStaff hfedStaff = db.HfedStaffs.Find(id);
            if (hfedStaff == null)
            {
                return HttpNotFound();
            }
            hfedStaff.HfedLocations = db.HfedLocations.OrderBy(l => l.Name).ToList();
            return View(hfedStaff);
        }

        // POST: HfedStaffs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Location,Phone,Email,StaffNote")] HfedStaff hfedStaff)
        {
            if (ModelState.IsValid)
            {
                using (var context = new SenecaContext())
                {
                    var sqlString = "UPDATE HfedStaff SET ";
                    sqlString += "FirstName = '" + hfedStaff.FirstName + "',";
                    sqlString += "LastName = '" + hfedStaff.LastName + "',";
                    sqlString += "Phone = '" + hfedStaff.Phone + "',";
                    sqlString += "Email = '" + hfedStaff.Email + "',";
                    sqlString += "StaffNote = '" + hfedStaff.StaffNote + "',";
                    sqlString += "Location_Id = " + hfedStaff.Location.Id;
                    sqlString += " WHERE Id = " + hfedStaff.Id;
                    context.Database.ExecuteSqlCommand(sqlString);
                }
                // This version not updating location change: 
                //db.Entry(hfedStaff).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hfedStaff);
        }

        // GET: HfedStaffs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HfedStaff hfedStaff = db.HfedStaffs.Find(id);
            if (hfedStaff == null)
            {
                return HttpNotFound();
            }
            return View(hfedStaff);
        }

        // POST: HfedStaffs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HfedStaff hfedStaff = db.HfedStaffs.Find(id);
            db.HfedStaffs.Remove(hfedStaff);
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
