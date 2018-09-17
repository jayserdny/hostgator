using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.EntityModels;
using MVC5_Seneca.ViewModels;

namespace MVC5_Seneca.Controllers
{
    public class TeachersController : Controller
    {
        private readonly SenecaContext _db = new SenecaContext();

        // GET: Teachers
        public ActionResult Index()
        {
            return View(_db.Teachers.ToList());
        }

        // GET: Teachers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = _db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // GET: Teachers/Create
        public ActionResult Create()
        {
            var viewModel = new AddEditTeacherViewModel();        
            //List<SelectListItem> schoolList = new List<SelectListItem>();
            //foreach (School school in _db.Schools)
            //{
            //    schoolList.Add(new SelectListItem { Text = school.Name, Value = school.Id.ToString() });
            //}
            //viewModel.Schools = schoolList;
            return View(viewModel);
        }

        // POST: Teachers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,LastName,School,FirstName,WorkPhone,CellPhone,Email")] AddEditTeacherViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Teacher teacher = new Teacher
                {
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    CellPhone = viewModel.CellPhone,
                    WorkPhone = viewModel.WorkPhone,
                    Email = viewModel.Email
                    //School = (from s in _db.Schools where s.Id == viewModel.School.Id select s).Single()
                };
                _db.Teachers.Add(teacher);
                _db.SaveChanges();
                return RedirectToAction("Index");

            }

            return View(viewModel);
        }

        // GET: Teachers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = _db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }

            AddEditTeacherViewModel viewModel = new AddEditTeacherViewModel
            {

                //List<SelectListItem> schoolList = new List<SelectListItem>();
                //foreach (School school in _db.Schools)
                //{
                //    if (school.Id == teacher.School.Id)
                //        schoolList.Add(new SelectListItem { Text = school.Name, Value = school.Id.ToString(), Selected = true });
                //    else
                //        schoolList.Add(new SelectListItem { Text = school.Name, Value = school.Id.ToString(), Selected = false });
                //}
                //viewModel.Schools = schoolList;

                Id = teacher.Id,
                LastName = teacher.LastName,
                FirstName = teacher.FirstName,
                //viewModel.School = teacher.School;
                WorkPhone = teacher.WorkPhone,
                CellPhone = teacher.CellPhone,
                Email = teacher.Email
            };

            return View(viewModel);
        }

        // POST: Teachers/Edit/5       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,LastName,FirstName,School,WorkPhone,CellPhone,Email")]
            AddEditTeacherViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var teacher = _db.Teachers.Find(viewModel.Id);
                if (teacher != null)
                {
                    teacher.LastName = viewModel.LastName;
                    teacher.FirstName = viewModel.FirstName;
                    teacher.WorkPhone = viewModel.WorkPhone;
                    if (viewModel.School != null)
                    {
                        teacher.School = (from s in _db.Schools where s.Id == viewModel.School.Id select s).Single();
                    }

                    teacher.CellPhone = viewModel.CellPhone;
                    teacher.Email = viewModel.Email;
                }

                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        // GET: Teachers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = _db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Teacher teacher = _db.Teachers.Find(id);
            if (teacher != null) _db.Teachers.Remove(teacher);
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
