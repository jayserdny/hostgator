using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.EntityModels;
using System.Collections.Generic;
using System.Web.Handlers;
using MVC5_Seneca.ViewModels;

namespace MVC5_Seneca.Controllers
{
    public class HfedSchedulesController : Controller
    {
        private SenecaContext db = new SenecaContext();

        // GET: HfedSchedules
        public ActionResult Index()
        {
            var schedulesView = new List<HfedSchedule>();            
            
            var hfedSchedules = db.HfedSchedules.ToList();
            foreach (HfedSchedule hfedSchedule in hfedSchedules)
            {
               string sqlString = "SELECT * FROM HfedSchedule WHERE Id = " + hfedSchedule.Id;
               var schedule = db.Database.SqlQuery<HfedScheduleViewModel >(sqlString).ToList();

                sqlString = "SELECT * FROM HfedLocation WHERE Id = " + schedule[0].Location_Id;
                var location = db.Database.SqlQuery<HfedLocation>(sqlString).ToList();
                hfedSchedule.Location = location[0];

                sqlString = "SELECT * FROM HfedStaff WHERE Id = " + schedule[0].PointPerson_Id;
                var staff = db.Database.SqlQuery<HfedStaff>(sqlString).ToList();
                hfedSchedule.PointPerson = staff[0];

                sqlString = "SELECT * FROM HfedProvider WHERE Id = " + schedule[0].Provider_Id;
                var provider = db.Database.SqlQuery<HfedProvider>(sqlString).ToList();
                hfedSchedule.Provider = provider[0];
                if (hfedSchedule.HfedDriverIds != null)
                {   
                    hfedSchedule.HfedDriversArray = hfedSchedule.HfedDriverIds.Split(',').ToArray();
                    List<SelectListItem> selectedDrivers = new List<SelectListItem>();
                    foreach (string driverId in hfedSchedule.HfedDriversArray)
                    {
                        var x = db.HfedDrivers.Find(Convert.ToInt32(driverId));
                        SelectListItem selListItem = new SelectListItem() {Value = "null", Text = x.LastName};
                        selectedDrivers.Add(selListItem); 
                    }

                    hfedSchedule.SelectedHfedDrivers = selectedDrivers;
                }

                if (hfedSchedule.HfedClientIds != null)
                {
                    hfedSchedule.HfedClientsArray = hfedSchedule.HfedClientIds.Split(',').ToArray();
                    List<SelectListItem> selectedClients = new List<SelectListItem>();
                    foreach (string clientId in hfedSchedule.HfedClientsArray)
                    {
                        var x = db.HfedDrivers.Find(Convert.ToInt32(clientId));
                        SelectListItem selListItem = new SelectListItem() { Value = "null", Text = x.LastName };
                        selectedClients.Add(selListItem);
                    }

                    hfedSchedule.SelectedHfedClients = selectedClients ;
                }

                schedulesView.Add(hfedSchedule);
            }                                                               
            return View(schedulesView);
        }
          
        // GET: HfedSchedules/Create
        public ActionResult Create()
        {   
            HfedSchedule newHfedSchedule = new HfedSchedule()
            {
                HfedProviders = db.HfedProviders.OrderBy(p => p.Name).ToList(),
                HfedLocations = db.HfedLocations.OrderBy(l => l.Name).ToList(),
                HfedStaffs = db.HfedStaffs.OrderBy(s => s.LastName).ToList(),
                HfedDrivers = db.HfedDrivers.OrderBy(d => d.LastName).ToList(),
                HfedClients = db.HfedClients.OrderBy(c => c.LastName).ToList()
            };  
            return View(newHfedSchedule);
        }

        // POST: HfedSchedules/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Date,PickUpTime,Provider,Location,PointPerson,ScheduleNote,Request,Complete,HfedDriversArray,HfedClientsArray")] HfedSchedule hfedSchedule)
        {
            hfedSchedule .HfedDriverIds = string.Join(",",hfedSchedule.HfedDriversArray);
            hfedSchedule.HfedClientIds  = string.Join(",", hfedSchedule.HfedClientsArray);
            //EF adding blank Foreign Key records: use raw SQL
            using (var context = new SenecaContext())
            {
                string cmdString = "INSERT INTO HfedSchedule (";
                cmdString += "Date,PickUpTime,ScheduleNote,Request,Complete,";
                cmdString += "Location_Id,PointPerson_Id,Provider_Id,HfedDriverIds,HfedClientIds)";
                cmdString += " VALUES (";
                cmdString += "'" + hfedSchedule.Date + "','" + hfedSchedule.PickUpTime + "',";
                cmdString += "'" + hfedSchedule .ScheduleNote + "','" +hfedSchedule .Request + "',";
                cmdString += "'" + hfedSchedule.Complete + "'," + hfedSchedule.Location.Id + ",";
                cmdString += hfedSchedule.PointPerson.Id + "," + hfedSchedule.Provider.Id + ",";
                cmdString += "'" + hfedSchedule.HfedDriverIds + "','" + hfedSchedule.HfedClientIds + "')";
                context.Database.ExecuteSqlCommand(cmdString);
            }
                                                                                     
            return RedirectToAction("Index");

                //return View(hfedSchedule);
            }

        // GET: HfedSchedules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HfedSchedule scheduletoEdit = db.HfedSchedules.Find(id);
            if (scheduletoEdit == null)
            {
                return HttpNotFound();
            }

            HfedScheduleViewModel hfedSchedule = new HfedScheduleViewModel
            {
                HfedProviders = db.HfedProviders.OrderBy(p => p.Name).ToList(),
                HfedLocations = db.HfedLocations.OrderBy(l => l.Name).ToList(),
                HfedStaffs = db.HfedStaffs.OrderBy(s => s.LastName).ToList(),
                HfedDrivers = db.HfedDrivers.OrderBy(d => d.FirstName).ToList(),
                HfedClients = db.HfedClients.OrderBy(c => c.LastName).ToList(),  
                Date = scheduletoEdit.Date,
                PickUpTime = scheduletoEdit.PickUpTime,
                ScheduleNote = scheduletoEdit.ScheduleNote,
                Request = scheduletoEdit.Request,
                Complete = scheduletoEdit.Complete,
                HfedDriverIds = scheduletoEdit.HfedDriverIds,
                HfedClientIds = scheduletoEdit.HfedClientIds
            };

            string sqlString = "SELECT * FROM HfedSchedule WHERE Id = " + id;
            var schedule = db.Database.SqlQuery<HfedScheduleViewModel>(sqlString).ToList();

            sqlString = "SELECT * FROM HfedLocation WHERE Id = " + schedule[0].Location_Id;
            var location = db.Database.SqlQuery<HfedLocation>(sqlString).ToList();
            hfedSchedule.Location = location[0];

            sqlString = "SELECT * FROM HfedStaff WHERE Id = " + schedule[0].PointPerson_Id;
            var staff = db.Database.SqlQuery<HfedStaff>(sqlString).ToList();
            hfedSchedule.PointPerson = staff[0];

            sqlString = "SELECT * FROM HfedProvider WHERE Id = " + schedule[0].Provider_Id;
            var provider = db.Database.SqlQuery<HfedProvider>(sqlString).ToList();
            hfedSchedule.Provider = provider[0];

            hfedSchedule.HfedDriversArray = hfedSchedule.HfedDriverIds.Split(',').ToArray();
            hfedSchedule.HfedClientsArray = hfedSchedule.HfedClientIds.Split(',').ToArray();

            return View(hfedSchedule);
        }

        // POST: HfedSchedules/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Date,Provider,Location,PickUpTime,PointPerson,ScheduleNote,Request,Complete")] HfedSchedule hfedSchedule)
        {
            //EF adding blank Foreign Key records: use raw SQL
            using (var context = new SenecaContext())
            {
                string cmdString = "UPDATE HfedSchedule SET ";
                cmdString += "Date='" + hfedSchedule.Date + "',";
                cmdString += "PickUpTime='" + hfedSchedule.PickUpTime + "',";
                cmdString += "ScheduleNote='" + hfedSchedule.ScheduleNote + "',";
                cmdString += "Request='" + hfedSchedule.Request + "',";
                cmdString += "Complete='" + hfedSchedule.Complete + "',";
                cmdString += "Location_Id=" + hfedSchedule.Location.Id + ",";
                cmdString += "PointPerson_Id=" + hfedSchedule.PointPerson.Id + ",";
                cmdString += "Provider_Id=" + hfedSchedule.Provider.Id ;
                cmdString += " WHERE Id=" + hfedSchedule.Id;
             context.Database.ExecuteSqlCommand(cmdString);
            }                                                                                          
            return RedirectToAction("Index");
          
            //return View(hfedSchedule);
        }

        // GET: HfedSchedules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HfedSchedule hfedSchedule = db.HfedSchedules.Find(id);
            if (hfedSchedule == null)
            {
                return HttpNotFound();
            }
            return View(hfedSchedule);
        }

        // POST: HfedSchedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HfedSchedule hfedSchedule = db.HfedSchedules.Find(id);
            if (hfedSchedule != null) db.HfedSchedules.Remove(hfedSchedule);
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
