using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.EntityModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Castle.Core.Internal;
using MVC5_Seneca.ViewModels;
using System.Web.WebPages;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MVC5_Seneca.Controllers
{
    public class HfedSchedulesController : Controller
    {
        private SenecaContext db = new SenecaContext();

        // GET: HfedSchedules
        public ActionResult Index(String startDate)
        {  
            var schedulesView = new List<HfedSchedule>();           
            List<HfedSchedule> hfedSchedules = null;     
            if (Session["StartDate"].ToString().IsNullOrEmpty())
            {
                hfedSchedules = db.HfedSchedules.ToList(); 
            }
            else
            {
                if (!startDate.IsNullOrEmpty())
                {
                    Session["StartDate"] = startDate;
                }
                DateTime date = Convert.ToDateTime(Session["StartDate"]);
                hfedSchedules = db.HfedSchedules.Where(s => s.Date >= date ).ToList();
            }
                                                  
            foreach (HfedSchedule hfedSchedule in hfedSchedules)
            {
               string sqlString = "SELECT * FROM HfedSchedule WHERE Id = " + hfedSchedule.Id;
               var schedule = db.Database.SqlQuery<HfedScheduleViewModel>(sqlString).ToList(); // Gets mapped Foreign Keys

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
                        if (!driverId.IsNullOrEmpty())
                        {
                            var x = db.HfedDrivers.Find(Convert.ToInt32(driverId));
                            SelectListItem selListItem = new SelectListItem() {Value = driverId, Text = x.FullName};
                            selectedDrivers.Add(selListItem);
                        }
                    }

                    hfedSchedule.SelectedHfedDrivers = selectedDrivers;
                }

                if (hfedSchedule.HfedClientIds != null)
                {
                    hfedSchedule.HfedClientsArray = hfedSchedule.HfedClientIds.Split(',').ToArray();
                    List<SelectListItem> selectedClients = new List<SelectListItem>();
                    foreach (string clientId in hfedSchedule.HfedClientsArray)
                    {
                        if (!clientId.IsNullOrEmpty())
                        {
                            var x = db.HfedClients.Find(Convert.ToInt32(clientId));
                            SelectListItem selListItem = new SelectListItem() {Value = clientId, Text = x.FullName};
                            selectedClients.Add(selListItem);
                        }
                    }

                    hfedSchedule.SelectedHfedClients = selectedClients ;
                }

                var s = hfedSchedule.ScheduleNote;  // For display, abbreviate to 10 characters:
                s = s.Length <= 10 ? s : s.Substring(0, 10) + "...";
                hfedSchedule.ScheduleNote = s;

                schedulesView.Add(hfedSchedule);
                                                                     
            }         

            return View(schedulesView);
        }
          
        // GET: HfedSchedules/Create
        public ActionResult Create()
        {   
            HfedSchedule newHfedSchedule = new HfedSchedule() 
            {
                Date =DateTime.Today,
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
        public ActionResult Create([Bind(Include ="Id,Date,PickUpTime," +
                                                  "Provider,Location,PointPerson," +
                                                  "ScheduleNote,Request,Complete," +                             
                                                  "HfedDriversArray,HfedClientsArray,VolunteerHours")]
            HfedSchedule hfedSchedule)
        {  
            if (hfedSchedule.PointPerson.Id == 0 || hfedSchedule.Location.Id == 0 || hfedSchedule.Provider.Id == 0)
    
            {   // Reload dropdown lists:
                hfedSchedule.HfedProviders = db.HfedProviders.OrderBy(p => p.Name).ToList();
                hfedSchedule.HfedLocations = db.HfedLocations.OrderBy(l => l.Name).ToList();
                hfedSchedule.HfedStaffs = db.HfedStaffs.OrderBy(s => s.LastName).ToList();
                hfedSchedule.HfedDrivers = db.HfedDrivers.OrderBy(d => d.LastName).ToList();
                hfedSchedule.HfedClients = db.HfedClients.OrderBy(c => c.LastName).ToList();
                return View(hfedSchedule); // (for error functions)
            } 

            if (hfedSchedule.HfedDriversArray.IsNullOrEmpty())
                {
                    hfedSchedule.HfedDriverIds = String.Empty;
                }
                else
                {
                    hfedSchedule.HfedDriverIds = string.Join(",", hfedSchedule.HfedDriversArray);
                }

                if (hfedSchedule.HfedClientsArray.IsNullOrEmpty())
                {
                    hfedSchedule.HfedClientIds = String.Empty;
                }
                else
                {
                    hfedSchedule.HfedClientIds = string.Join(",", hfedSchedule.HfedClientsArray);
                }

                //EF adding blank Foreign Key records: use raw SQL
                using (var context = new SenecaContext())
                {
                    string cmdString = "INSERT INTO HfedSchedule (";
                    cmdString += "Date,PickUpTime,ScheduleNote,Request,Complete,";
                    cmdString += "Location_Id,PointPerson_Id,Provider_Id,HfedDriverIds,HfedClientIds)";
                    cmdString += " VALUES (";
                    cmdString += "'" + hfedSchedule.Date + "','" + hfedSchedule.PickUpTime + "',";
                    cmdString += "'" + hfedSchedule.ScheduleNote + "','" + hfedSchedule.Request + "',";
                    cmdString += "'" + hfedSchedule.Complete + "'," + hfedSchedule.Location.Id + ",";
                    cmdString += hfedSchedule.PointPerson.Id + "," + hfedSchedule.Provider.Id + ",";
                    cmdString += "'" + hfedSchedule.HfedDriverIds + "',";
                    cmdString += "'" + hfedSchedule.HfedClientIds + "')";

                    context.Database.ExecuteSqlCommand(cmdString);
                }

                return RedirectToAction("Index");
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
                HfedClientIds = scheduletoEdit.HfedClientIds,
                VolunteerHours = scheduletoEdit.VolunteerHours 
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
        public ActionResult Edit([Bind(Include = "Id,Date,PickUpTime,Provider,Location,PointPerson,ScheduleNote,Request,Complete,HfedDriversArray,HfedClientsArray,VolunteerHours")] HfedSchedule hfedSchedule)
        {
            //EF adding blank Foreign Key records: use raw SQL
            if (hfedSchedule.HfedDriversArray.IsNullOrEmpty())
            {
                hfedSchedule.HfedDriverIds = String.Empty;
            }
            else
            {
                hfedSchedule.HfedDriverIds = string.Join(",", hfedSchedule.HfedDriversArray);
            }

            if (hfedSchedule.HfedClientsArray.IsNullOrEmpty())
            {
                hfedSchedule.HfedClientIds = String.Empty;
            }
            else
            {
                hfedSchedule.HfedClientIds = string.Join(",", hfedSchedule.HfedClientsArray);
            }
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
                cmdString += "Provider_Id=" + hfedSchedule.Provider.Id + ",";  
                cmdString += "HfedDriverIds='" + hfedSchedule .HfedDriverIds + "',";
                cmdString += "HfedClientIds='" + hfedSchedule .HfedClientIds + "',";
                if ((hfedSchedule.VolunteerHours ?? 0) == 0)
                {
                    cmdString += "VolunteerHours=NULL";
                }
                else
                {
                    cmdString += "VolunteerHours=" + hfedSchedule.VolunteerHours;
                }                                                                                                         
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

        public async Task<ActionResult> Email()
        {
            var apiKey = Properties.Settings.Default.HFEDSendGridClient ;
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("test@example.com", "DX Team"),
                Subject = "Hello World from the SendGrid CSharp SDK!",
                PlainTextContent = "Hello, Email!",
                HtmlContent = "<strong>Hello, Email!</strong>"
            };
            msg.AddTo(new EmailAddress("prowny@aol.com", "Test User"));
            var response = await client.SendEmailAsync(msg);

            return RedirectToAction("Index");
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
