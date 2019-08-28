using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
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
            foreach (var hfedClient in db.HfedClients.OrderBy( l =>l.LastName).ToList())
            {
                using (var context = new SenecaContext())
                {
                    var sqlString = "SELECT Location_Id FROM HfedClient WHERE Id = " + hfedClient.Id;
                    var locationId = context.Database.SqlQuery<Int32>(sqlString).FirstOrDefault();
                    var location = db.HfedLocations.Find(locationId);

                    EntityModels.HfedClient viewModel = new HfedClient()
                    {
                        Id = hfedClient.Id,
                        FirstName = hfedClient.FirstName,
                        LastName = hfedClient.LastName,
                        DateOfBirth = hfedClient.DateOfBirth, 
                        Location = location,
                        ClientNote = hfedClient.ClientNote,
                        Active = hfedClient.Active  
                    };
                    model.Add(viewModel);
                }                         
            }
            return View(model);
        }

        // GET: HfedClients/Create
        public ActionResult Create()
        {
            EntityModels.HfedClient hfedClientView = new EntityModels.HfedClient() 
            {
                HfedLocations = db.HfedLocations.OrderBy(l => l.Name).ToList() 
            };
            hfedClientView.Active = true;
            return View(hfedClientView);
        }

        // POST: HfedClients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,DateOfBirth,Active,ClientNote,Location")] EntityModels.HfedClient hfedClient)
        {
            //EF adding blank Foreign Key records: use raw SQL
            using (var context = new SenecaContext())
            {
                string cmdString = "INSERT INTO HfedClient (";
                cmdString += "FirstName,LastName,DateOfBirth,Active,ClientNote,Location_Id)"; 
                cmdString += " VALUES (";
                cmdString += "'" + hfedClient.FirstName + "','" + hfedClient.LastName + "',";
                cmdString += "'" + hfedClient.DateOfBirth + "',";
                if (hfedClient.Active)
                {
                    cmdString += "1,";
                }
                else
                {
                    cmdString += "0,";
                }

                if (hfedClient.ClientNote != null)
                {
                    cmdString += "'" + hfedClient.ClientNote.Replace("'", "''") + "',";
                }
                else
                {
                    cmdString += "'',";
                }
                cmdString += hfedClient.Location.Id + ")";
                context.Database.ExecuteSqlCommand(cmdString);
            }
            return RedirectToAction("Index");       
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
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,DateOfBirth,Active,Location,ClientNote")] EntityModels.HfedClient hfedClient)
        {
            if (ModelState.IsValid)
            {
                using (var context = new SenecaContext())
                {
                    var sqlString = "UPDATE HfedClient SET ";
                    if (hfedClient.FirstName != null)
                    {
                        sqlString += "FirstName = '" + hfedClient.FirstName + "',";
                    }

                    if (hfedClient.LastName != null)
                    {
                        sqlString += "LastName = '" + hfedClient.LastName + "',";
                    }

                    sqlString += "DateOfBirth = '" + hfedClient.DateOfBirth + "',";

                    if (hfedClient.Active)
                    {
                        sqlString += "Active = 1,";
                    }
                    else
                    {
                        sqlString += "Active = 0,";
                    }

                    if (hfedClient.ClientNote != null)
                    {
                        sqlString += "ClientNote = '" + hfedClient.ClientNote.Replace("'", "''") + "',";
                    }

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
            EntityModels.HfedClient hfedClient = db.HfedClients.Find(id);
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
            EntityModels.HfedClient hfedClient = db.HfedClients.Find(id);
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
