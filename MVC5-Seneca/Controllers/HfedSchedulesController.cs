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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using static MVC5_Seneca.Utilities;

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
                hfedSchedules = db.HfedSchedules.OrderBy(s => s.Date).ToList();
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
                hfedSchedules = db.HfedSchedules.Where(s => s.Date >= sDate && s.Date <= eDate).OrderBy(s => s.Date)
                    .ToList();
            }

            foreach (HfedSchedule hfedSchedule in hfedSchedules)
            {
                string sqlString = "SELECT * FROM HfedSchedule WHERE Id = " + hfedSchedule.Id;
                var schedule =
                    db.Database.SqlQuery<HfedScheduleViewModel>(sqlString).ToList(); // Gets mapped Foreign Keys

                sqlString = "SELECT * FROM HfedLocation WHERE Id = " + schedule[0].Location_Id;
                var location = db.Database.SqlQuery<HfedLocation>(sqlString).ToList();
                hfedSchedule.Location = location[0];

                hfedSchedule.PointPerson = db.Users.Find(schedule[0].PointPerson_Id);

                var did = schedule[0].Driver_Id;
                if (did != null)
                {
                    hfedSchedule.Driver = db.Users.Find(did);
                }

                sqlString = "SELECT * FROM HfedProvider WHERE Id = " + schedule[0].Provider_Id;
                var provider = db.Database.SqlQuery<HfedProvider>(sqlString).ToList();
                hfedSchedule.Provider = provider[0];
                if (hfedSchedule.HfedDriverIds != null)
                {
                    hfedSchedule.HfedDrivers = new List<ApplicationUser>();
                    hfedSchedule.HfedDriversArray = hfedSchedule.HfedDriverIds.Split(',').ToArray();
                    List<SelectListItem> selectedDrivers = new List<SelectListItem>();
                    foreach (string driverId in hfedSchedule.HfedDriversArray)
                    {
                        if (!driverId.IsNullOrEmpty())
                        {
                            var x = db.Users.Find(driverId);
                            if (x != null)
                                if (UserIsInRole(x, "Active"))
                                {
                                    {
                                        SelectListItem selListItem = new SelectListItem()
                                            {Value = driverId, Text = x.FullName};
                                        selectedDrivers.Add(selListItem);
                                        // One delivery - one driver rule: put driver name in schedule:
                                        hfedSchedule.Driver = x;
                                        hfedSchedule.DriverName = x.FullName;
                                        hfedSchedule.HfedDrivers.Add(x);
                                    }
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

                    hfedSchedule.SelectedHfedClients = selectedClients;
                }

                if (hfedSchedule.Households == null)
                {
                    hfedSchedule.Households = 0;
                }

                hfedSchedule.NoteToolTip =
                    hfedSchedule.ScheduleNote.Replace(" ",
                        "\u00a0"); // (full length on mouseover)    \u00a0 is the Unicode character for NO-BREAK-SPACE.
                var s = hfedSchedule.ScheduleNote; // For display, abbreviate to 10 characters:            
                s = s.Length <= 10 ? s : s.Substring(0, 10) + "...";
                hfedSchedule.ScheduleNote = s;

                hfedSchedule.ClientsTotal = hfedSchedule.SelectedHfedClients.Count( );
                hfedSchedule.FormattedDay = hfedSchedule.Date.ToString("ddd");
                hfedSchedule.FormattedDate = hfedSchedule.Date.ToString("MM/dd/yy");
                
                schedulesView.Add(hfedSchedule);
            }

            return View(schedulesView);
        }

        // GET: HfedSchedules/Create
        public ActionResult Create()
        {
            Session["OriginalClientIds"] ="";
            HfedScheduleViewModel  newHfedSchedule = new HfedScheduleViewModel() 
            {
                Date = Convert.ToDateTime(Session["StartDate"]),
                HfedProviders = db.HfedProviders.OrderBy(p => p.Name).ToList(),
                HfedLocations = db.HfedLocations.OrderBy(l => l.Name).ToList(),
                HfedClients = new List<HfedClient>(),                                                                 
                HfedDrivers = new List<ApplicationUser>(),
                HfedStaffs = new List<ApplicationUser>(),   
                HfedClientsArray = new string [1], 
                Request = false,
                Complete = false,
                Households = 0, Approved = false
            };
            var allUsers = db.Users.OrderBy(n => n.LastName).ToList();
            foreach (ApplicationUser user in allUsers)
            {
                if (UserIsInRole(user, "HfedDriver"))
                {
                    newHfedSchedule.HfedDrivers.Add(user);
                }

                if (UserIsInRole(user, "HfedStaff"))
                {
                    newHfedSchedule.HfedStaffs.Add(user);
                }
            }

            return View(newHfedSchedule);
        }

        private Boolean UserIsInRole(ApplicationUser user, string roleName)
        {
            var roleStore = new RoleStore<IdentityRole>(db);
            var roleMngr = new RoleManager<IdentityRole>(roleStore);
            var roles = roleMngr.Roles.ToList();

            foreach (IdentityRole role in roles)
            {
                if (role.Name == roleName)
                {
                    using (var context = new SenecaContext())
                    {
                        string strSql = "SELECT RoleId FROM AspNetUserRoles WHERE ";
                        strSql += "RoleId ='" + role.Id + "' AND UserId ='" + user.Id + "'";
                        string result = context.Database.SqlQuery<string>(strSql).FirstOrDefault();
                        if (result != null)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        // POST: HfedSchedules/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Date,PickUpTime,Provider,Location,PointPerson," +
                                                   "ScheduleNote,Driver,Request,Complete,Households,Approved," +
                                                   "HfedDriversArray,HfedClientsArray,VolunteerHours")]
            HfedScheduleViewModel hfedSchedule)
        {
            if (hfedSchedule.PointPerson.Id == null || hfedSchedule.Location.Id == 0 || hfedSchedule.Provider.Id == 0)
            {
                hfedSchedule = GetDropDownData(hfedSchedule); // Reload dropdown lists 
                if (hfedSchedule.PointPerson.Id == null){hfedSchedule.ErrorMessage = "Point Person required";}
                if (hfedSchedule.Location.Id == 0) { hfedSchedule.ErrorMessage = "Location required";}
                if (hfedSchedule.Provider.Id == 0) { hfedSchedule.ErrorMessage = "Provider required";}
                hfedSchedule = GetDropDownData(hfedSchedule); // Reload dropdown lists 
                return View(hfedSchedule); // (for error functions)
            }

            hfedSchedule = CheckForDuplicateClient(hfedSchedule);
            if (!hfedSchedule.ErrorMessage.IsNullOrEmpty() ) {return View(hfedSchedule);}

            if (hfedSchedule.HfedDriversArray.IsNullOrEmpty())
            {
                hfedSchedule.HfedDriverIds = string.Empty;
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

            //EF not adding blank Foreign Key records: use raw SQL
            using (var context = new SenecaContext())
            {
                string cmdString = "INSERT INTO HfedSchedule (";
                cmdString += "Date,PickUpTime,ScheduleNote,Request,Complete,";
                cmdString += "Households,Approved,";
                cmdString += "Location_Id,PointPerson_Id,Provider_Id,HfedDriverIds,HfedClientIds)";
                cmdString += " VALUES (";
                cmdString += "'" + hfedSchedule.Date + "','" + hfedSchedule.PickUpTime + "',";
                if (hfedSchedule.ScheduleNote.IsNullOrEmpty())
                {
                    cmdString += "'',";
                }
                else
                {
                    cmdString += "'" + hfedSchedule.ScheduleNote.Replace("'", "''") + "',";
                }

                cmdString += "0,0,"; // insert zeroes into Request & Complete
                if (hfedSchedule.Households == null)
                {
                    cmdString += "0,";
                }
                else
                {
                    cmdString += hfedSchedule.Households + ",";
                }

                if (hfedSchedule.Approved)
                {
                    cmdString += "1,";
                }
                else
                {
                    cmdString += "0,";
                }

                cmdString += hfedSchedule.Location.Id + ",";
                cmdString += "'" + hfedSchedule.PointPerson.Id + "'," + hfedSchedule.Provider.Id + ",";
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
                                                                                            
            var sqlString = "SELECT * FROM hfedSchedule WHERE Id =" + id;
            var scheduletoEdit = db.Database.SqlQuery<HfedScheduleViewModel>(sqlString).FirstOrDefault();
            if (scheduletoEdit == null){return HttpNotFound();}
            // OriginalClientIds are kept for use by HfedClients/GetClients; if the user inadverently changes Location,
            // the original selections (if any) will be reinstated when returning to the original Location.
            Session["OriginalClientIds"] = scheduletoEdit.HfedClientIds;
            var Loc = db.HfedLocations.Find(scheduletoEdit.Location_Id);    
            var hfedSchedule = new HfedScheduleViewModel
            {                                                                                                                     
                Location_Id = scheduletoEdit.Location_Id,
                Location = Loc,
                Date = scheduletoEdit.Date,
                PickUpTime = scheduletoEdit.PickUpTime,
                ScheduleNote = scheduletoEdit.ScheduleNote,
                Request = scheduletoEdit.Request,
                Complete = scheduletoEdit.Complete,
                Households = scheduletoEdit.Households,
                Approved = scheduletoEdit.Approved,
                HfedDriverIds = scheduletoEdit.HfedDriverIds,
                HfedClientIds = scheduletoEdit.HfedClientIds,
                VolunteerHours = scheduletoEdit.VolunteerHours
            };  
            hfedSchedule.Location =db.HfedLocations .Find( scheduletoEdit .Location_Id);
            hfedSchedule.PointPerson = db.Users.Find(scheduletoEdit.PointPerson_Id);
            if (scheduletoEdit.Driver_Id != null){hfedSchedule.Driver = db.Users.Find(scheduletoEdit.Driver_Id);}
            hfedSchedule.Provider = db.HfedProviders.Find(scheduletoEdit.Provider_Id);
            hfedSchedule.HfedDriversArray = hfedSchedule.HfedDriverIds.Split(',').ToArray();
            hfedSchedule.HfedClientsArray = hfedSchedule.HfedClientIds.Split(',').ToArray();
            hfedSchedule = GetDropDownData(hfedSchedule);
            return View(hfedSchedule);
        }

        // POST: HfedSchedules/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Date,PickUpTime,Provider,Location,PointPerson," +
                                                 "ScheduleNote,Driver,Request,Complete,Households,Approved," +
                                                 "HfedDriversArray,HfedClientsArray,VolunteerHours")]
            HfedScheduleViewModel  hfedSchedule)
        {
            if (hfedSchedule.HfedClientsArray.IsNullOrEmpty())
            {
                hfedSchedule.HfedClientIds = string.Empty;
            }
            else
            {
                hfedSchedule.HfedClientIds = string.Join(",", hfedSchedule.HfedClientsArray ?? throw new InvalidOperationException());
            }
            hfedSchedule = CheckForDuplicateClient(hfedSchedule);
            if (!hfedSchedule.ErrorMessage.IsNullOrEmpty())
            {
                hfedSchedule = GetDropDownData(hfedSchedule);
                return View(hfedSchedule); // (for error functions)
            }
            //EF not adding blank Foreign Key records: use raw SQL 
            string cmdString = "UPDATE HfedSchedule SET ";
            cmdString += "Date='" + hfedSchedule.Date + "',";
            cmdString += "PickUpTime='" + hfedSchedule.PickUpTime + "',";
            if (hfedSchedule.ScheduleNote != null)
            {
                cmdString += "ScheduleNote='" + hfedSchedule.ScheduleNote.Replace("'", "''") + "',";
            }
            else
            {
                cmdString += "ScheduleNote='',";
            }

            cmdString += "Request='" + hfedSchedule.Request + "',";
            cmdString += "Complete='" + hfedSchedule.Complete + "',";
            if (hfedSchedule.Households == null){hfedSchedule.Households = 0;}
            cmdString += "Households=" + hfedSchedule.Households + ",";
            if (hfedSchedule.Approved)
            {
                cmdString += "Approved=1,";
            }
            else
            {
                cmdString += "Approved=0,";
            }

            cmdString += "Location_Id=" + hfedSchedule.Location.Id + ",";
            cmdString += "PointPerson_Id='" + hfedSchedule.PointPerson.Id + "',";
            cmdString += "Driver_Id='" + hfedSchedule.Driver.Id + "',";
            cmdString += "Provider_Id=" + hfedSchedule.Provider.Id + ",";
            cmdString += "HfedDriverIds='" + hfedSchedule.HfedDriversArray + "',";
            cmdString += "HfedClientIds='" + hfedSchedule.HfedClientIds + "',";
            if (Math.Abs((hfedSchedule.VolunteerHours ?? 0)) < .01)
            {
                cmdString += "VolunteerHours=NULL";
            }
            else
            {
                cmdString += "VolunteerHours=" + hfedSchedule.VolunteerHours;
            }
            cmdString += " WHERE Id=" + hfedSchedule.Id;
            db.Database.ExecuteSqlCommand(cmdString); 
             
            var usrId = User.Identity.GetUserId();   
            var allUsers = db.Users.ToList();                                                     
            foreach (ApplicationUser user in allUsers)
            {
                if (UserIsInRole(user, "ReceiveHfedScheduleChangeEmail"))
                {
                    var recipientId = user.Id;  // Email Lynn Rose that an update has occurred (01/15/2020)
                    var unused = EmailHFEDScheduleChange(usrId, hfedSchedule.Id,
                                           hfedSchedule.Provider.Id, recipientId);
                }
            }   

            return RedirectToAction("Index");
        }
             
        private HfedScheduleViewModel GetDropDownData(HfedScheduleViewModel schedule)
        {
            schedule .HfedProviders = db.HfedProviders.OrderBy(p => p.Name).ToList();
            schedule.HfedClients = db.HfedClients.Where(c => c.Active 
                        && c.Location.Id == schedule.Location.Id) .OrderBy(c => c.LastName).ToList();
            schedule.HfedDrivers = new List<ApplicationUser>();
            schedule.HfedStaffs = new List<ApplicationUser>();
            schedule.HfedLocations = db.HfedLocations.OrderBy(l => l.Name).ToList();
           
            var allUsers = db.Users.OrderBy(n => n.LastName).ToList();
            foreach (var user in allUsers)
            {
                if (UserIsInRole(user, "HfedStaff"))
                {
                    schedule.HfedStaffs.Add(user);
                }

                if (UserIsInRole(user, "HfedDriver"))
                {
                    schedule.HfedDrivers.Add(user);
                }
            }
            return schedule;
        }

        private HfedScheduleViewModel CheckForDuplicateClient(HfedScheduleViewModel schedule)
        {
            // Check for duplicate clients on same day    
            var otherSchedules = db.HfedSchedules.Where(s => s.Date == schedule.Date
                                                             && s.Id != schedule.Id).ToList();
            if (otherSchedules.Count > 0 && schedule.HfedClientsArray != null)
            {
                foreach (HfedSchedule otherSched in otherSchedules)
                {
                    otherSched.HfedClientsArray = otherSched.HfedClientIds.Split(',').ToArray();
                    foreach (var c in otherSched.HfedClientsArray)
                    {
                        foreach (var c1 in schedule.HfedClientsArray)
                        {   
                            if (c == c1 && !c1.IsNullOrEmpty())
                            {
                                HfedClient dupClient = db.HfedClients.Find(Convert.ToInt32(c));
                                if (dupClient != null)
                                {
                                   schedule.ErrorMessage = dupClient.FullName
                                                                + " is already selected in another run on "
                                                                + otherSched.Date.ToShortDateString();

                                    schedule = GetDropDownData(schedule);
                                    return schedule; // (for error functions) 
                                }
                            }
                        }
                    }
                }
            }

            return schedule;
        }

        // GET: HfedSchedules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var scheduleToDelete = new HfedSchedule {Id = (int) id};
                
            string strSql = "SELECT * FROM HfedSchedule WHERE Id = " + id;
            var schedule = db.Database.SqlQuery<HfedScheduleViewModel >(strSql).ToList();

            scheduleToDelete.Location =db.HfedLocations.Find(schedule[0].Location_Id);
            scheduleToDelete.PointPerson = db.Users.Find(schedule[0].PointPerson_Id);
            scheduleToDelete.Provider = db.HfedProviders .Find( schedule[0].Provider_Id);
            scheduleToDelete.Date = schedule[0].Date;
                
            return View(scheduleToDelete );      
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

        public ActionResult CreateExcel()
        {                                                                                          
            XLWorkbook workbook = new XLWorkbook();
            IXLWorksheet ws = workbook.Worksheets.Add("Schedule");
            int activeRow = 1;
            ws.Cell(activeRow ,1).SetValue("Updated:");
            ws.Cell(activeRow,1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right; 
            ws.Cell(activeRow, 2).SetValue(DateTime.Today.ToString( "MM/dd/yyyy" ));

            activeRow += 1;
            ws.Cell(activeRow, 1).SetValue("Food Run");
            ws.Cell(activeRow, 2).SetValue("Provider");
            ws.Cell(activeRow, 3).SetValue("Location");
            ws.Cell(activeRow, 4).SetValue("Households");
            ws.Cell(activeRow, 5).SetValue("Pick Up");
            ws.Cell(activeRow, 6).SetValue("Point");
            ws.Cell(activeRow, 7).SetValue("Driver");
            ws.Cell(activeRow, 8).SetValue("Note");

            ws.Row(activeRow).Style.Font.Bold = true;
            DateTime startDate = Convert.ToDateTime(Session["StartDate"]);
            DateTime endDate = Convert.ToDateTime(Session["EndDate"]);
            var deliveryRequests = db.HfedSchedules.Where(s =>
                s.Date >= startDate && s.Date <= endDate).OrderBy(s => s.Date).ToList();
            foreach (HfedSchedule request in deliveryRequests)
            {
                activeRow += 1;
                var dow = request.Date.ToString("ddd") + " ";
                ws.Cell(activeRow, 1).SetValue(dow + request.Date.ToString("MM/dd/yy"));

                var sqlString = "SELECT * FROM HfedSchedule WHERE Id = " + request.Id;
                var schedule = db.Database.SqlQuery<HfedScheduleViewModel>(sqlString).ToList();

                sqlString = "SELECT * FROM HfedProvider WHERE Id = " + schedule[0].Provider_Id;
                var provider = db.Database.SqlQuery<HfedProvider>(sqlString).ToList();
                ws.Cell(activeRow, 2).SetValue(provider[0].Name);

                sqlString = "SELECT * FROM HfedLocation WHERE Id = " + schedule[0].Location_Id;
                var location = db.Database.SqlQuery<HfedLocation>(sqlString).ToList();
                ws.Cell(activeRow, 3).SetValue(location[0].Name);

                ws.Cell(activeRow, 4).SetValue(request.Households.ToString());
                ws.Cell(activeRow, 5).SetValue(request.PickUpTime);

                ApplicationUser pointPerson = db.Users.Find(schedule[0].PointPerson_Id);
                ws.Cell(activeRow, 6).SetValue(pointPerson.FirstName);

                string driverName = "";
                if (schedule[0].Driver_Id != null)
                {
                    ApplicationUser driver = db.Users.Find(schedule[0].Driver_Id);
                    if (driver != null) driverName = driver.FirstName;
                }

                ws.Cell(activeRow, 7).SetValue(driverName);
                ws.Cell(activeRow, 8).SetValue(request.ScheduleNote);
            }

            ws.Columns().AdjustToContents();
            MemoryStream ms = new MemoryStream();
            workbook.SaveAs(ms);
            ms.Position = 0;
            return new FileStreamResult(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {FileDownloadName = "DeliverySchedule.xlsx"};
        }

        // GET: DriverSignUp
        public ActionResult DriverSignUp()
        {   // Set Sign-Up month: if inside the last week of a month, then next month.
            var start = DateTime.Today; 
            int days = DateTime.DaysInMonth(start.Year, start.Month);
            if (start.Day > days - 7)  // In the last 6 days
            {
                start = start.AddMonths(1);
            }
            start = new DateTime(start.Year, start.Month, 1);
            var end = new DateTime(start.Year, start.Month, days);

            var scheduleList = db.HfedSchedules.Where
                (s => s.Date >= start && s.Date <= end).OrderBy(s => s.Date).ToList();
            HfedScheduleViewModel hfedSchedule = new HfedScheduleViewModel
            {
                UserIsOnSchedule = false, HfedScheds = new List<HfedSchedule>()
            };
            var usr = db.Users.Find(User.Identity.GetUserId());
            hfedSchedule.DriverFullName = usr.FullName;
            foreach (HfedSchedule sched in scheduleList)
            {
                string strSql = "SELECT * FROM HfedSchedule WHERE Id = " + sched.Id;
                var schedData = db.Database.SqlQuery<HfedScheduleViewModel>(strSql).FirstOrDefault();

                if (schedData != null)
                {
                    strSql = "SELECT * FROM HfedLocation WHERE Id = " + schedData.Location_Id;
                    sched.Location = db.Database.SqlQuery<HfedLocation>(strSql).FirstOrDefault();

                    sched.PointPerson = db.Users.Find(schedData.PointPerson_Id);

                    strSql = "SELECT * FROM HfedProvider WHERE Id = " + schedData.Provider_Id;
                    sched.Provider = db.Database.SqlQuery<HfedProvider>(strSql).FirstOrDefault();

                    sched.HfedDriversArray = schedData.HfedDriverIds.Split(',').ToArray();
                    sched.HfedClientsArray = schedData.HfedClientIds.Split(',').ToArray();
                    sched.HfedDrivers = new List<ApplicationUser>();
                    var did = schedData.Driver_Id;
                    if (!did.IsNullOrEmpty())
                    {
                        sched.Driver = db.Users.Find(did);
                        if (sched.Driver.UserName == User.Identity.Name)
                        {
                            hfedSchedule.DriverFullName = sched.Driver.FullName;
                            hfedSchedule.UserIsOnSchedule = true;
                        }
                    }
                }

                var allUsers = db.Users.OrderBy(n => n.FirstName).ToList();
                foreach (ApplicationUser user in allUsers)
                {
                    if (UserIsInRole(user, "HfedDriver"))
                    {
                        sched.HfedDrivers.Add(user);
                    }
                }

                // Convert viewmodel schedule to hfedschedule to add: 
                var hfedSched = new HfedSchedule
                {
                    Id = sched.Id,
                    Date = sched.Date,
                    PickUpTime = sched.PickUpTime,
                    Location = sched.Location,
                    PointPerson = sched.PointPerson,
                    Provider = sched.Provider,
                    HfedDriversArray = sched.HfedDriversArray,
                    HfedClientsArray = sched.HfedClientsArray,
                    HfedDrivers = sched.HfedDrivers,
                    SignUp = false,
                    Cancel = false,
                    Driver = sched.Driver,
                    DriverName = sched.DriverName,
                    FormattedDay = sched.Date.ToString("ddd"),
                    FormattedDate = sched.Date.ToString("MM/dd/yy")
                };
                hfedSchedule.HfedScheds.Add(hfedSched);
            }

            return View(hfedSchedule);
        }

        // POST: HfedSchedules/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DriverSignUp([Bind(Include = "HfedScheds")] HfedScheduleViewModel schedules)
        {
            foreach (var sched in schedules.HfedScheds)
            {
                if (sched.SignUp)
                {
                    using (var context = new SenecaContext())
                    {
                        string cmdString = "UPDATE HfedSchedule SET ";
                        cmdString += "Driver_Id='" + User.Identity.GetUserId() + "' ";
                        cmdString += " WHERE Id=" + sched.Id;
                        context.Database.ExecuteSqlCommand(cmdString);
                        // Send email to driver
                        //"You have signed up to deliver food from - to on date."
                    }
                }

                if (sched.Cancel)
                {
                    using (var context = new SenecaContext())
                    {
                        string cmdString = "UPDATE HfedSchedule SET ";
                        cmdString += "HfedDriverIds='',Driver_Id=null";
                        cmdString += " WHERE Id=" + sched.Id;
                        context.Database.ExecuteSqlCommand(cmdString);
                        // Send email to driver
                        //"You have unchecked your delivery on date."
                    }
                }
            }

            return RedirectToAction("DriverSignUp");
        }

        // GET: Next Month Schedules
        public ActionResult MonthNext()
        {
            var dt1 = Convert.ToDateTime(Session["StartDate"]);
            dt1 = dt1.AddMonths(1);
            Session["StartDate"] = dt1.Month + "/01/" + dt1.Year;
            var eom = DateTime.DaysInMonth(dt1.Year, dt1.Month);
            Session["EndDate"] = dt1.Month + "/" + eom + "/" + dt1.Year;
            return RedirectToAction("Index");
        }

        public ActionResult MonthPrevious()
        {
            var dt1 = Convert.ToDateTime(Session["StartDate"]);
            dt1 = dt1.AddMonths(-1);
            Session["StartDate"] = dt1.Month + "/01/" + dt1.Year;
            var eom = DateTime.DaysInMonth(dt1.Year, dt1.Month);
            Session["EndDate"] = dt1.Month + "/" + eom + "/" + dt1.Year;
            return RedirectToAction("Index");
        }

        public ActionResult Duplicate(int? id) // check Are You Sure?
        {
            var startDate = Convert.ToDateTime(Session["StartDate"]);
            startDate = startDate.AddMonths(1);
            int month = startDate.Month;
            int year = startDate.Year;

            var hfedSchedules = db.HfedSchedules.Where(s => s.Date.Month == month
                                                            && s.Date.Year == year).ToList();
            Session["ExistingSchedules"] = hfedSchedules.Count;  
            return View();                                  
        }

        // POST: HfedSchedules/Duplicate
        [HttpPost, ActionName("Duplicate")]
        [ValidateAntiForgeryToken]
        public ActionResult DuplicateConfirmed()
        {
            // Get this month's schedules:                                    
            var sDate = Convert.ToDateTime(Session["StartDate"]);
            var eDate = Convert.ToDateTime(Session["EndDate"]); 
            var hfedSchedules = db.HfedSchedules.Where(s => s.Date >= sDate && s.Date <= eDate).OrderBy(s => s.Date).ToList();
            
            foreach (HfedSchedule hfedSchedule in hfedSchedules)
            {
                string sqlString = "SELECT * FROM HfedSchedule WHERE Id = " + hfedSchedule.Id;
                var schedule = db.Database.SqlQuery<HfedScheduleViewModel>(sqlString).ToList(); // Gets mapped Foreign Keys

                sqlString = "SELECT * FROM HfedLocation WHERE Id = " + schedule[0].Location_Id;
                var location = db.Database.SqlQuery<HfedLocation>(sqlString).ToList();
                hfedSchedule.Location = location[0];

                hfedSchedule.PointPerson = db.Users.Find(schedule[0].PointPerson_Id);

                sqlString = "SELECT * FROM HfedProvider WHERE Id = " + schedule[0].Provider_Id;
                var provider = db.Database.SqlQuery<HfedProvider>(sqlString).ToList();
                hfedSchedule.Provider = provider[0];
                hfedSchedule.HfedDriverIds = null;  // no drivers in duplicate
                // adjust date:
                var dt = hfedSchedule.Date.AddMonths(1);
                var monthStart = new DateTime(dt.Year, dt.Month, 1);
                var dow = hfedSchedule.Date.DayOfWeek;       
                for (int i =-7; i < 8; i++)  // Find 1st date within month with same dow
                {
                    var newDate = dt.AddDays(i);
                    if (newDate.DayOfWeek == dow && newDate >= monthStart)
                    {
                        hfedSchedule.Date = newDate;
                        using (var context = new SenecaContext())
                        {
                            string cmdString = "INSERT INTO HfedSchedule (";
                            cmdString += "Date,PickUpTime,ScheduleNote,Request,Complete,Households,";
                            cmdString += "Location_Id,PointPerson_Id,Provider_Id,HfedDriverIds,HfedClientIds)";
                            cmdString += " VALUES (";
                            cmdString += "'" + hfedSchedule.Date + "','" + hfedSchedule.PickUpTime + "',";
                            if (hfedSchedule.ScheduleNote != null)
                            {
                                cmdString += "'" + hfedSchedule.ScheduleNote.Replace("'", "''") + "',";
                            }
                            else
                            {
                                cmdString += "'',";
                            }

                            cmdString += "0,0,"; // Request & Complete = false
                            cmdString += hfedSchedule.Households + ",";
                            cmdString += hfedSchedule.Location.Id + ",";
                            cmdString += "'" + hfedSchedule.PointPerson.Id + "'," + hfedSchedule.Provider.Id + ",";
                            cmdString += "'" + hfedSchedule.HfedDriverIds + "',";
                            cmdString += "'" + hfedSchedule.HfedClientIds + "')";

                            context.Database.ExecuteSqlCommand(cmdString);
                        }
                        break;
                    }
                }
            }                                                          
            // Set Start/End dates to view the newly created schedule:
            return RedirectToAction("MonthNext");
        }

        public ActionResult HfedContacts()
        {
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
