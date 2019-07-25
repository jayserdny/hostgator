using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.EntityModels;
using System.Collections.Generic;
using System.IO;
using Castle.Core.Internal;
using MVC5_Seneca.ViewModels;
using ClosedXML.Excel;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MVC5_Seneca.Controllers
{
    public class HfedSchedulesController : Controller
    {
        private SenecaContext db = new SenecaContext();

        // GET: HfedSchedules
        public ActionResult Index(String startDate, String endDate)
        {  
            var schedulesView = new List<HfedSchedule>();           
            List<HfedSchedule> hfedSchedules;     
            if (Session["StartDate"].ToString().IsNullOrEmpty() || Session["EndDate"].ToString().IsNullOrEmpty())
            {
                hfedSchedules = db.HfedSchedules.OrderBy( s => s.Date).ToList(); 
            }
            else
            {
                if (!startDate.IsNullOrEmpty())
                {   
                    Session["StartDate"] = startDate;
                }

                if (!endDate.IsNullOrEmpty())
                {
                    Session["EndDate"] = endDate;
                }

                DateTime sDate = Convert.ToDateTime(Session["StartDate"]);
                DateTime eDate = Convert.ToDateTime(Session["EndDate"]);
                hfedSchedules = db.HfedSchedules.Where(s => s.Date >= sDate && s.Date <= eDate).OrderBy(s => s.Date).ToList();
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
                            if (x != null)
                            {
                                SelectListItem selListItem = new SelectListItem() {Value = driverId, Text = x.FullName};
                                selectedDrivers.Add(selListItem);
                            }
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
                            if (x != null)
                            {
                                SelectListItem selListItem = new SelectListItem() {Value = clientId, Text = x.FullName};
                                selectedClients.Add(selListItem);
                            }
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
                HfedClients = db.HfedClients.OrderBy(c => c.LastName).ToList(),
                Request = true 
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

            HfedScheduleViewModel hfedSchedule = new HfedScheduleViewModel( )
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

        public ActionResult EmailStaffAsk()
        {
            EmailStaff();  
            return RedirectToAction("Index");
        }

        public ActionResult EmailDriversAsk()
        {
            EmailDrivers(false);  // No drivers
            return RedirectToAction("Index");
        }

        public ActionResult EmailDriversShow()
        {
            EmailDrivers(true);  // Show drivers
            return RedirectToAction("Index");
        }
        public ActionResult EmailReminder()
        {
            EmailReminders() ;  
            return RedirectToAction("Index");
        }

        private void EmailDrivers(Boolean withDrivers)    // Ask drivers for availability / show with drivers
        {
            var htmlContent = "<p>Greetings Team!</p>";
            if (withDrivers)
            {
                htmlContent += "<p>Thank you for being a part of the team that works to allow ";
                htmlContent += "our MCCH residents to have Healthy Food Every Day.</p> ";
            }
            else
            {
                htmlContent += "<p>Please take a look and let us know which food runs you are ";
                htmlContent += "available for and interested in doing. ";
                htmlContent += "We will get back to you soon to confirm.</p>";
            }

            //var htmlContent = "<table style=" + (char)34 + "border: 1px;" + (char)34 + ">";    // doesn't work
            htmlContent += "<table border=" + (char)34 + "1" + (char)34 + ">";
            DateTime startDate = Convert.ToDateTime(Session["StartDate"]);
            DateTime endDate = Convert.ToDateTime(Session["EndDate"]);
            var deliveryRequests = db.HfedSchedules.Where(s =>
                s.Date >= startDate && s.Date <= endDate
                                    && s.Request && s.Complete == false).OrderBy(s => s.Date).ToList();
            foreach (HfedSchedule request in deliveryRequests)
            {
                htmlContent += "<tr><td>" + request.Date.ToShortDateString() + "</td>";

                var sqlString = "SELECT * FROM HfedSchedule WHERE Id = " + request.Id;
                var schedule = db.Database.SqlQuery<HfedScheduleViewModel>(sqlString).ToList();

                sqlString = "SELECT * FROM HfedProvider WHERE Id = " + schedule[0].Provider_Id;
                var provider = db.Database.SqlQuery<HfedProvider>(sqlString).ToList();
                htmlContent += "<td>" + provider[0].Name + "</td>";

                sqlString = "SELECT * FROM HfedLocation WHERE Id = " + schedule[0].Location_Id;
                var location = db.Database.SqlQuery<HfedLocation>(sqlString).ToList();
                htmlContent += "<td>" + location[0].Name + "</td>";

                htmlContent += "<td>" + request.PickUpTime + "</td>";

                sqlString = "SELECT * FROM HfedStaff WHERE Id = " + schedule[0].PointPerson_Id;
                var pointPerson = db.Database.SqlQuery<HfedStaff>(sqlString).ToList();
                htmlContent += "<td>" + pointPerson[0].FirstName + "</td>";
                htmlContent += "<td bgcolor=" + (char) 34 + "#FFFF00" + (char) 34 + ">";

                string drivers = "";
                if (withDrivers && !request.HfedDriverIds.IsNullOrEmpty())
                {
                    string[] driversArray = request.HfedDriverIds.Split(',').ToArray(); 
                    foreach (string driverId in driversArray)
                    {
                        HfedDriver driver = db.HfedDrivers.Find(Convert.ToInt32(driverId));
                        if (drivers.Length != 0)
                        {
                            drivers += ", ";
                        } 
                        if (driver != null) drivers += driver.FirstName;
                    } 
                }
                else
                {
                    drivers = "&nbsp;&nbsp;" +
                                   "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                                   "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
                }
                htmlContent += drivers + "</td>";
                if (withDrivers)
                {
                    htmlContent += "<td>" + request.ScheduleNote + "</td>";
                }  
            }
            htmlContent += "<tr></table> ";
            htmlContent += "<br /><br /><p>";
            if (!withDrivers)
            {
                htmlContent += "Thank you for helping people have Healthy Food Every Day!</p>";
            }

            var driversList = db.HfedDrivers.ToList();
            foreach (HfedDriver driver in driversList)
            {
                string addr = driver.Email;
                if (!addr.IsNullOrEmpty())
                {
                    Email(driver.Email, htmlContent);
                }
            }
        }

        private async void Email(string address, string htmlContent)
        {   
            var apiKey = Properties.Settings.Default.HFEDSendGridClient;
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage();
            msg.SetFrom("Admin@SenecaHeightsEducationProgram.org", "HFED Coordinator");
            msg.SetSubject("Healthy Food Every Day -  Schedule");  
            msg.AddContent(MimeType.Html, htmlContent);
            msg.AddTo(new EmailAddress(address, "HFED Volunteer Driver"));
            var unused = await client.SendEmailAsync(msg);
        }

        public void EmailStaff()    // Email MCCH teams (staff) to populate next month's schedule
        {
            var htmlContent = "<p>Greetings MCCH HFED Team!</p><br />";
            htmlContent += "<p>Please send your food delivery requests to the HFED Coordinator ";
            htmlContent += "for next month</p> ";
            List<HfedStaff> staffList = db.HfedStaffs.ToList();
            foreach (HfedStaff staff in staffList)
            {
                Email(staff.Email, htmlContent);
            }
        }

        public void EmailReminders()
        {   
            DateTime reminderDate = DateTime.Today.AddDays(2);
            string reminderEmailList = "";
            // Add MCCH Community Engagement (Lynn Rose) to Email List 
            reminderEmailList += "prowny@aol.com";
           
            var schedules = db.HfedSchedules.Where(s => s.Date == reminderDate).ToList();  
            foreach (HfedSchedule reminder in schedules)
            {
                string htmlContent = "<p>Reminder:</p>";
                htmlContent += "<p>You have an upcoming HFED delivery:</p>";
                htmlContent += "<table border=" + (char)34 + "1" + (char)34 + "><tr>";

                var sqlString = "SELECT * FROM HfedSchedule WHERE Id = " + reminder.Id;
                var schedule = db.Database.SqlQuery<HfedScheduleViewModel>(sqlString).ToList();
                htmlContent += "<td>" + reminder.Date .ToShortDateString() + "</td>";
   
                sqlString = "SELECT * FROM HfedProvider WHERE Id = " + schedule[0].Provider_Id;
                var provider = db.Database.SqlQuery<HfedProvider>(sqlString).ToList();
                htmlContent += "<td>" + provider[0].Name + "</td>";

                sqlString = "SELECT * FROM HfedLocation WHERE Id = " + schedule[0].Location_Id;
                var location = db.Database.SqlQuery<HfedLocation>(sqlString).ToList();
                htmlContent += "<td>" + location[0].Name + "</td>";

                htmlContent += "<td>" + reminder.PickUpTime + "</td>";    
              
                sqlString = "SELECT * FROM HfedStaff WHERE Id = " + schedule[0].PointPerson_Id;
                var pointPerson = db.Database.SqlQuery<HfedStaff>(sqlString).ToList();
                htmlContent += "<td>" + pointPerson[0].FirstName + "</td>";
                reminderEmailList += "," + pointPerson[0].Email;
                string drivers = "";
                if (!reminder.HfedDriverIds.IsNullOrEmpty())
                {
                    string[] driversArray = reminder.HfedDriverIds.Split(',').ToArray();
                    foreach (string driverId in driversArray)
                    {
                        HfedDriver driver = db.HfedDrivers.Find(Convert.ToInt32(driverId));
                        if (drivers.Length != 0)
                        {
                            drivers += ", ";
                        }

                        if (driver?.Email != null)
                        {
                            drivers += driver.FirstName;
                            reminderEmailList += "," + driver.Email;
                        }                                                                  
                    }                                                                        
                }
                else
                {
                    drivers = "&nbsp;&nbsp;" +
                              "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" +
                              "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>";
                }

                htmlContent += "<td>" + drivers + "</td></tr></table>";
                htmlContent += "<table><tr><td>Info:</td></tr>";

                htmlContent += "<tr><td>Point Person:</td>";
                htmlContent += "<td>" + pointPerson[0].FirstName + "&nbsp;" + pointPerson[0].LastName + "</td>";
                htmlContent += "<td>" + pointPerson[0].Phone + "</td><td>" + pointPerson[0].Email + "</td></tr></table>";

                htmlContent += "<table><tr><td>Note:</td>";
                htmlContent += "<td>" + reminder .ScheduleNote + "</td></tr></table>";
        
                htmlContent += "<br /><br /><p>";
                string[] emailList = reminderEmailList.Split(',').ToArray();
                foreach (string emailAddress in emailList)
                {
                    if (!emailAddress.IsNullOrEmpty() && !htmlContent.IsNullOrEmpty())
                    {                                                
                        Email(emailAddress, htmlContent);
                    }

                    htmlContent = "";
                }
            }
            //return RedirectToAction("Index", "HfedSchedules");
        }

        public ActionResult CreateExcel()
        {
            XLWorkbook workbook = new XLWorkbook();
            IXLWorksheet ws = workbook.Worksheets.Add("Requests");
            int activeRow = 1;
            ws.Cell(activeRow, 1).SetValue("Food Delivery Schedule"); 
            ws.Row(1).Style.Font.Bold = true;
            DateTime startDate = Convert.ToDateTime(Session["StartDate"]);
            DateTime endDate = Convert.ToDateTime(Session["EndDate"]);
            var deliveryRequests = db.HfedSchedules.Where(s =>
                s.Date >= startDate && s.Date <= endDate
                                    && s.Request && s.Complete == false).OrderBy(s => s.Date).ToList();
            foreach (HfedSchedule request in deliveryRequests)
            {
                activeRow += 1;
                ws.Cell(activeRow, 1).SetValue(request.Date.ToShortDateString());

                var sqlString = "SELECT * FROM HfedSchedule WHERE Id = " + request.Id;
                var schedule = db.Database.SqlQuery<HfedScheduleViewModel>(sqlString).ToList();

                sqlString = "SELECT * FROM HfedProvider WHERE Id = " + schedule[0].Provider_Id;
                var provider = db.Database.SqlQuery<HfedProvider>(sqlString).ToList();
                ws.Cell(activeRow, 2).SetValue(provider[0].Name);

                sqlString = "SELECT * FROM HfedLocation WHERE Id = " + schedule[0].Location_Id;
                var location = db.Database.SqlQuery<HfedLocation>(sqlString).ToList();
                ws.Cell(activeRow, 3).SetValue(location[0].Name);

                ws.Cell(activeRow, 4).SetValue(request.PickUpTime);

                sqlString = "SELECT * FROM HfedStaff WHERE Id = " + schedule [0].PointPerson_Id;
                var pointPerson = db.Database.SqlQuery<HfedStaff>(sqlString).ToList();  
                ws.Cell(activeRow, 5).SetValue(pointPerson[0].FirstName);

                string drivers = "";
                if (!request.HfedDriverIds.IsNullOrEmpty())
                {
                    string[] driversArray = request.HfedDriverIds.Split(',').ToArray();
                    foreach (string driverId in driversArray)
                    {
                        HfedDriver driver = db.HfedDrivers.Find(Convert.ToInt32(driverId));
                        if (drivers.Length != 0)
                        {
                            drivers += ", ";
                        }

                        if (driver != null) drivers += driver.FirstName;
                    }
                }
                ws.Cell(activeRow, 6).SetValue(drivers); 
                ws.Cell(activeRow, 7).SetValue(request.ScheduleNote);
            }
            ws.Columns().AdjustToContents();
            //ws.Column( 6).Style.Fill.BackgroundColor = XLColor.Amber;
            //ws.Column(6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;   
            MemoryStream ms = new MemoryStream();
            workbook.SaveAs(ms);
            ms.Position = 0;
            return new FileStreamResult(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {FileDownloadName = "DeliverySchedule.xlsx"};
        }

        public ActionResult DriverSignUp()
        {
            DateTime start = Convert.ToDateTime(Session["StartDate"]);
            DateTime end = Convert.ToDateTime(Session["EndDate"]);
            var scheduleList = db.HfedSchedules.Where
                (s => s.Date >= start && s.Date <= end).OrderBy(s => s.Date).ToList();
            HfedScheduleViewModel hfedSchedule = new HfedScheduleViewModel();
            hfedSchedule.HfedSchedules = new List<HfedSchedule>();
            foreach (HfedSchedule sched in scheduleList)
            {                                                                                                                          
                string strSql = "SELECT * FROM HfedSchedule WHERE Id = " + sched.Id;
                var schedData = db.Database.SqlQuery<HfedScheduleViewModel >(strSql).FirstOrDefault();

                if (schedData != null)
                {
                    strSql = "SELECT * FROM HfedLocation WHERE Id = " + schedData.Location_Id;
                    sched.Location = db.Database.SqlQuery<HfedLocation>(strSql).FirstOrDefault();

                    strSql = "SELECT * FROM HfedStaff WHERE Id = " + schedData.PointPerson_Id;
                    sched.PointPerson = db.Database.SqlQuery<HfedStaff>(strSql).FirstOrDefault();

                    strSql = "SELECT * FROM HfedProvider WHERE Id = " + schedData.Provider_Id;
                    sched.Provider = db.Database.SqlQuery<HfedProvider>(strSql).FirstOrDefault();

                    sched.HfedDriversArray = schedData.HfedDriverIds.Split(',').ToArray();
                    sched.HfedClientsArray = schedData.HfedClientIds.Split(',').ToArray();
                }
                sched.HfedDrivers = db.HfedDrivers.OrderBy(d => d.FirstName).ToList();

                     // Convert viewmodel to hfedschedule?
                     var hfedSched = new HfedSchedule();
                     hfedSched.Id = sched.Id;
                     hfedSched.Date = sched.Date;
                     hfedSched.PickUpTime = sched.PickUpTime;
                     hfedSched.Location = sched.Location;
                     hfedSched.PointPerson = sched.PointPerson;
                     hfedSched.Provider = sched.Provider;
                     hfedSched.HfedDriversArray = sched.HfedDriversArray;
                     hfedSched.HfedClientsArray = sched.HfedClientsArray;
                     hfedSched.SignUp = false;
                hfedSchedule.HfedSchedules.Add(hfedSched);
            }  

            return View(hfedSchedule);
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
