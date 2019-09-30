using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Castle.Core.Internal;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.EntityModels;
using MVC5_Seneca.ViewModels;
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace MVC5_Seneca.Controllers
{
    public class HfedClientsController : Controller
    {
        private SenecaContext db = new SenecaContext();

        // GET: HfedClients
        public ActionResult Index(int? locId)
        {
            var model = new HfedClientViewModel();
            if (TempData["ClientLocationId"] != null)
            { locId = Convert.ToInt32(TempData["ClientLocationId"]); }  // Returning from Create / Edit
            if (locId == 0 || locId == null)
            {
                model.HfedClients = db.HfedClients.OrderBy(l => l.LastName).ToList(); // select all Locations (default)
            }
            else
            {
                model.HfedClients = db.HfedClients.Where( l => l.Location .Id == locId).OrderBy(l => l.LastName).ToList();
            }

            model.HfedLocations  = db.HfedLocations.OrderBy(l => l.Name).ToList();        
            foreach (var loc in model .HfedLocations )
            {
               if(loc.Id == locId ) { model.SelectedId = loc.Id; }
            }
            foreach (var hfedClient in model.HfedClients.ToList())
            {
                using (var context = new SenecaContext())
                {
                    var sqlString = "SELECT Location_Id FROM HfedClient WHERE Id = " + hfedClient.Id;
                    var locationId = context.Database.SqlQuery<Int32>(sqlString).FirstOrDefault();      
                    var location = db.HfedLocations.Find(locationId);
                    hfedClient.Location = location;
                    hfedClient.FormattedBirthDate = hfedClient.DateOfBirth.ToString("MM/dd/yyyy");
                    hfedClient.NoteToolTip =
                        hfedClient.ClientNote.Replace(" ",
                            "\u00a0"); // (full length on mouseover)    \u00a0 is the Unicode character for NO-BREAK-SPACE.
                    var s = hfedClient.ClientNote ; // For display, abbreviate to 10 characters:            
                    s = s.Length <= 10 ? s : s.Substring(0, 10) + "...";
                    hfedClient.ClientNote = s;                
                }                         
            }
            return View(model);
        }

        // GET: HfedClients/Create
        public ActionResult Create()
        {
            var hfedClientView = new HfedClient
            {
                HfedLocations = db.HfedLocations.OrderBy(l => l.Name).ToList(), Active = true
            };
            return View(hfedClientView);
        }

        // POST: HfedClients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,DateOfBirth,Active,ClientNote,Location")] HfedClient hfedClient)
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

                if (!hfedClient.ClientNote.IsNullOrEmpty())
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
            TempData["ClientLocationId"] = hfedClient.Location.Id;
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
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,DateOfBirth,Active,Location,ClientNote")] HfedClient hfedClient)
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
                    else
                    {
                        sqlString += "ClientNote='',";
                    }

                    sqlString += "Location_Id = " + hfedClient.Location.Id;
                    sqlString += " WHERE Id = " + hfedClient.Id;
                    context.Database.ExecuteSqlCommand(sqlString);
                }
                // This version not updating location change: 
                //db.Entry(hfedClient).State = EntityState.Modified;
                //db.SaveChanges();

                TempData ["ClientLocationId"] = hfedClient.Location.Id;
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
            var hfedClient = db.HfedClients.Find(id);
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
            var hfedClient = db.HfedClients.Find(id);
            if (hfedClient != null) db.HfedClients.Remove(hfedClient);
            db.SaveChanges();                                                          
            return RedirectToAction("Index");
        }

        public ActionResult GetClients(int id /* drop down value of Location_Id */)
        { 
            // called when a Location has changed in Edit or Create schedule
            var clients = db.HfedClients.Where(c => c.Location.Id == id).OrderBy(c => c.LastName).ToList();   
            List<SelectListItem> clientList = new SelectList(clients, "Id", "FullName").ToList();
            // Insert the original ClientListIds into the first select list item:                         
            clientList .Insert(0, (new SelectListItem { Text = @"OriginalClientIds",
                Value = Session["OriginalClientIds"].ToString() }));  // Session value declared in HfedSchedules/Edit
            // OriginalClientIds are inserted for use by the Edit view; if the user inadverently changes Location,
            // the original selections (if any) will be reinstated when returning to the original Location.
            try
            {
                string json = JsonConvert.SerializeObject(clientList, Formatting.Indented);
                return Content(json, "application/json");
            }
            catch (Exception)
            {
                return null;
            }             
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
