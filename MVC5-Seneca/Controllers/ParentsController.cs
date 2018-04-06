using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Web.Mvc;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.EntityModels;
using MVC5_Seneca.ViewModels;

namespace MVC5_Seneca.Controllers
{
    public class ParentsController : Controller
    {
        private readonly SenecaContext _db = new SenecaContext();

        // GET: Parents
        public ActionResult Index()
        {                                                      
            return View(_db.Parents.ToList());
        }

        // GET: Parents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parent parent = _db.Parents.Find(id);
            if (parent == null)
            {
                return HttpNotFound();
            }
            return View(parent);
        }

        // GET: Parents/Create
        public ActionResult Create()
        {
            var viewModel = new AddEditParentViewModel { };
            viewModel.SelectedMotherFather = "M";  // default 
            
            List<SelectListItem> staffList = new List<SelectListItem>();
            var staffs = _db.StaffMembers.ToList();
            foreach (Staff staff in staffs)
            {
                staffList.Add(new SelectListItem {Text = staff.LastName + @", " + staff.FirstName, Value = staff.Id.ToString()});
            }                                    
            viewModel.StaffMembers = staffList;
            return View(viewModel);
        }

        // POST: Parents/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MotherFather,FirstName,Address,HomePhone,CellPhone,Email,SelectedMotherFather,CaseManager")] AddEditParentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var parent = _db.Parents.Create();
                parent.MotherFather = model.SelectedMotherFather;
                parent.FirstName = model.FirstName;
                parent.Email = model.Email;
                parent.Address = model.Address;
                parent.HomePhone = model.HomePhone;
                parent.CellPhone = model.CellPhone;
                if (model.StaffMember != null)
                {
                    parent.CaseManager = (from s in _db.StaffMembers where s.Id == model.StaffMember.Id select s).Single();
                }

                _db.Parents.Add(parent);
                _db.SaveChanges();
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
            Parent parent = _db.Parents.Find(id);
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

        List<SelectListItem> staffList = new List<SelectListItem>();
            var sortedStaff = _db.StaffMembers.OrderBy(s => s.LastName).ThenBy(s => s.FirstName).ToList();
            staffList.Add(new SelectListItem { Text = @" (none)", Value = "0", Selected = false });
            foreach (Staff staff in sortedStaff)
                if (parent.CaseManager == null)
                {
                    staffList.Add(new SelectListItem { Text = staff.FirstName + @" " + staff.LastName, Value = staff.Id.ToString(), Selected = false });
                }
                else
                {
                    if (staff.Id == parent.CaseManager.Id)
                        staffList.Add(new SelectListItem { Text = staff.FirstName + @" " + staff.LastName, Value = staff.Id.ToString(), Selected = true });
                    else
                        staffList.Add(new SelectListItem { Text = staff.FirstName + @" " + staff.LastName, Value = staff.Id.ToString(), Selected = false });
                }   
        
            viewModel.StaffMembers = staffList;
            return View(viewModel);
        }

        // POST: Parents/Edit/5                                                             
        [HttpPost]
        [ValidateAntiForgeryToken, MethodImpl(MethodImplOptions.NoOptimization)]
        public ActionResult Edit([Bind(Include = "Id,MotherFather,FirstName,Address,HomePhone,CellPhone,Email,SelectedMotherFather,StaffMember")] AddEditParentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var sqlString = "UPDATE Parent Set ";
                sqlString += "FirstName = '" + viewModel.FirstName + "',";
                sqlString += "Address = '" + viewModel.Address + "',";
                sqlString += "HomePhone = '" + viewModel.HomePhone + "',";
                sqlString += "CellPhone = '" + viewModel.CellPhone + "',";
                sqlString += "Email = '" + viewModel.Email + "',";
                sqlString += "MotherFather = '" + viewModel.SelectedMotherFather + "',";
                if (viewModel.StaffMember.Id == 0)
                {
                    sqlString += "CaseManager_Id = NULL";
                }
                else
                {
                    sqlString += "CaseManager_Id =" + viewModel.StaffMember.Id;
                } 
                sqlString += " WHERE Id =" + viewModel.Id; 
                using (var context = new SenecaContext())
                {
                    context.Database.ExecuteSqlCommand(sqlString);
                }

                //var caseManager = _db.StaffMembers.Find(viewModel.StaffMember.Id);
                //var parent = _db.Parents.Find(viewModel.Id);
                //if (parent != null  )
                //{
                //    parent.FirstName = viewModel.FirstName;
                //    parent.Address = viewModel.Address;
                //    parent.HomePhone = viewModel.HomePhone;
                //    parent.CellPhone = viewModel.CellPhone;
                //    parent.Email = viewModel.Email;
                //    parent.MotherFather = viewModel.SelectedMotherFather;
                                                                                                                                          
                // //Does not get hit unless breakpointed:
                // parent.SetCaseManager(caseManager);

                //if (viewModel.StaffMember.Id == 0)
                //{
                //    // Does not get hit unless breakpointed:
                //    parent.CaseManager = null;
                //}
                //else
                //{
                //    parent.CaseManager = (from s in _db.StaffMembers where s.Id == viewModel.StaffMember.Id select s).Single();
                //}
                //_db.SaveChanges();
                // } 

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
            Parent parent = _db.Parents.Find(id);
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
            Parent parent = _db.Parents.Find(id);
            if (parent != null) _db.Parents.Remove(parent);
            _db.SaveChanges();
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
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
  
    }

}
