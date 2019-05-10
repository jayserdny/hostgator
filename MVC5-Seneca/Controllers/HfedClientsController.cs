using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DocumentFormat.OpenXml.Office2010.Excel;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.EntityModels;

namespace MVC5_Seneca.Controllers
{
    public class HfedClientsController : Controller
    {
        private SenecaContext db = new SenecaContext();

        // GET: HfedClients
        public ActionResult Index()
        {
            var model = new List<HfedClient>();
            foreach (var hfedClient in db.HfedClients.ToList())
            {
                using (var context = new SenecaContext())
                {
                    var sqlString = "SELECT Location_Id FROM HfedClient WHERE Id = " + hfedClient.Id;
                    var locationId = context.Database.SqlQuery<Int32>(sqlString).FirstOrDefault();
                    var location = db.HfedLocations.Find(locationId);

                    HfedClient viewModel = new HfedClient()
                    {
                        Id = hfedClient.Id,
                        FirstName = hfedClient.FirstName,
                        LastName = hfedClient.LastName,
                        DateOfBirth =hfedClient .DateOfBirth, 
                        Location = location,
                        ClientNote = hfedClient.ClientNote
                    };
                    model.Add(viewModel);
                }                         
            }
            return View(model);
        }

        // GET: HfedClients/Create
        public ActionResult Create()
        {
            HfedClient hfedClientView = new HfedClient()
            {
                HfedLocations = db.HfedLocations.OrderBy(l => l.Name).ToList() 
            };

            return View(hfedClientView);
        }

        // POST: HfedClients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,DateOfBirth,Location,ClientNote")] HfedClient hfedClient)
        {
            if (ModelState.IsValid)
            {
                HfedLocation location = db.HfedLocations.Find(hfedClient.Location.Id);
                HfedClient  newHfedClient = new HfedClient( )
                {
                   FirstName =hfedClient.FirstName,
                   LastName =hfedClient.LastName,
                   DateOfBirth =hfedClient.DateOfBirth,
                   Location = location,
                   ClientNote =hfedClient .ClientNote 
                };
                db.HfedClients.Add(newHfedClient);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hfedClient);
        }

        // GET: HfedClients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HfedClient hfedClient = db.HfedClients.Find(id);
            if (hfedClient == null)
            {
                return HttpNotFound();
            }

            hfedClient.HfedLocations = db.HfedLocations.OrderBy(l => l.Name).ToList();
            return View(hfedClient);
        }

        // POST: HfedClients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,DateOfBirth,Location,ClientNote")] HfedClient hfedClient)
        {
            if (ModelState.IsValid)
            {
                using (var context = new SenecaContext())
                {
                    var sqlString = "UPDATE HfedClient SET ";
                    sqlString += "FirstName = '" + hfedClient.FirstName + "',";
                    sqlString += "LastName = '" + hfedClient.LastName + "',";
                    sqlString += "DateOfBirth = '" + hfedClient.DateOfBirth + "',";
                    sqlString += "ClientNote = '" + hfedClient.ClientNote + "',";
                    sqlString += "Location_Id = " + hfedClient.Location.Id;
                    sqlString += " WHERE Id = " + hfedClient.Id;
                    context.Database.ExecuteSqlCommand(sqlString);
                }
                // This version not updating location change: 
                //db.Entry(hfedClient).State = EntityState.Modified;
                //db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hfedClient);
        }

        // GET: HfedClients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HfedClient hfedClient = db.HfedClients.Find(id);
            if (hfedClient == null)
            {
                return HttpNotFound();
            }
            return View(hfedClient);
        }

        // POST: HfedClients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HfedClient hfedClient = db.HfedClients.Find(id);
            db.HfedClients.Remove(hfedClient);
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
