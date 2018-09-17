using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.EntityModels;
using MVC5_Seneca.ViewModels;
using Microsoft.WindowsAzure.Storage;

namespace MVC5_Seneca.Controllers
{
    public class StudentsController : Controller
    {
        private readonly SenecaContext _db = new SenecaContext();

        // GET: Students
        public ActionResult Index()
        {
            return View(_db.Students.OrderBy(f => f.FirstName).ToList());         
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = _db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            var viewModel = new AddEditStudentViewModel();
            //string errMsg = NewMethod();
            List<SelectListItem> schoolList = new List<SelectListItem>();
            foreach (School school in _db.Schools)
            { 
                schoolList.Add(new SelectListItem { Text = school.Name, Value = school.Id.ToString()});            
            }
            viewModel.Schools = schoolList;

            List<SelectListItem> parentList = new List<SelectListItem>();
            var sortedParents = _db.Parents.OrderBy(p => p.FirstName).ToList();
            foreach (Parent parent in sortedParents)
            {
               parentList.Add(new SelectListItem { Text = parent.FirstName, Value = parent.Id.ToString() });
            }

            List<SelectListItem> userList = new List<SelectListItem>(); 
            var sortedUsers = _db.Users.OrderBy(u => u.LastName).ThenBy(u => u.FirstName).ToList();
            foreach (ApplicationUser user in sortedUsers)
            {
                if (user.Id != null)
                    userList.Add(new SelectListItem {Text = user.LastName + @", " + user.FirstName, Value = user.Id});
            }

            List<SelectListItem> teacherList = new List<SelectListItem>();
            var sortedTeachers = _db.Teachers.OrderBy(p => p.LastName).ToList();
            teacherList.Add(new SelectListItem { Text =  @"(none) ", Value = "0", Selected = true });
            foreach (Teacher teacher in sortedTeachers)
            {
                teacherList.Add(new SelectListItem { Text = teacher.LastName + @", " +teacher.FirstName , Value = teacher.Id.ToString() });
            }

            viewModel.SpecialClass = false;
            viewModel.Parents = parentList;
            viewModel.Schools = schoolList;
            viewModel.Teachers = teacherList;
            viewModel.Users = userList;
            //viewModel.BirthDate = DateTime.Now.AddYears(-10);     
            return View(viewModel);
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,Gender,BirthDate,GradeLevel,SpecialClass,School,Parent,PrimaryTutor,Teacher")] AddEditStudentViewModel viewModel)
        {
            viewModel.ErrorMessage = null;
            if (viewModel.School.Id == 0)
            {
                viewModel.ErrorMessage = "School Required!";
            }
            if (viewModel.Parent.Id == 0)
            {
                viewModel.ErrorMessage = "Parent Required!";
            }  
            if (viewModel.FirstName == null)
            {
                viewModel.ErrorMessage = "First Name Required!";
            }

            if (viewModel.ErrorMessage != null)  // rebuild drop-down lists:
            {
                List<SelectListItem> schoolList = new List<SelectListItem>();
                foreach (School school in _db.Schools)
                {
                    schoolList.Add(new SelectListItem { Text = school.Name, Value = school.Id.ToString() });
                } 
                viewModel.Schools = schoolList;   
                List<SelectListItem> parentList = new List<SelectListItem>();

                var sortedParents = _db.Parents.OrderBy(p => p.FirstName).ToList();
                foreach (Parent parent in sortedParents)
                {
                    parentList.Add(new SelectListItem { Text = parent.FirstName, Value = parent.Id.ToString() });
                }
                viewModel.Parents = parentList;

                List<SelectListItem> userList = new List<SelectListItem>();
                var sortedUsers = _db.Users.OrderBy(u => u.LastName).ThenBy(u => u.FirstName).ToList();
                foreach (ApplicationUser user in sortedUsers)
                {
                    if (user.Id != null)
                        userList.Add(new SelectListItem { Text = user.LastName + @", " + user.FirstName, Value = user.Id });
                }
                viewModel.Users = userList;

                List<SelectListItem> teacherList = new List<SelectListItem>();
                var sortedTeachers = _db.Teachers.OrderBy(p => p.LastName).ToList();
                foreach (Teacher teacher in sortedTeachers)
                    teacherList.Add(new SelectListItem
                    {
                        Text = teacher.LastName + @", " + teacher.FirstName,
                        Value = teacher.Id.ToString()
                    });
                viewModel.Teachers = teacherList;

                return View(viewModel);
            }

            if (ModelState.IsValid)            
            { 
                Student student = new Student
                {
                    FirstName = viewModel.FirstName,
                    Gender = viewModel.Gender,
                    BirthDate = viewModel.BirthDate,
                    GradeLevel = viewModel.GradeLevel,
                    SpecialClass = viewModel.SpecialClass,
                    Parent = (from p in _db.Parents where p.Id == viewModel.Parent.Id select p).Single(),
                    School = (from s in _db.Schools where s.Id == viewModel.School.Id select s).Single()
                };

                if (viewModel.PrimaryTutor.Id != null)
                {
                    student.PrimaryTutor = (from t in _db.Users where t.Id == viewModel.PrimaryTutor.Id select t).Single();
                }
                if (viewModel.Teacher .Id != 0)
                {
                    student.Teacher  = (from t in _db.Teachers where t.Id == viewModel.Teacher.Id select t).Single();
                }
                _db.Students.Add(student);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }  
            return RedirectToAction("Create");
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = _db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }

            var viewModel = new AddEditStudentViewModel();

            List<SelectListItem> schoolList = new List<SelectListItem>();
            foreach (School school in _db.Schools)
            {
                if (school.Id == student.School.Id)
                    schoolList.Add(new SelectListItem { Text = school.Name, Value = school.Id.ToString(), Selected = true });
                else
                    schoolList.Add(new SelectListItem { Text = school.Name, Value = school.Id.ToString(), Selected = false });
            }
            viewModel.Schools = schoolList;

            List<SelectListItem> parentList = new List<SelectListItem>();
            var sortedParents = _db.Parents.OrderBy(p => p.FirstName).ToList();
            foreach (Parent parent in sortedParents)
                if (student.Parent == null)
                {
                    parentList.Add(new SelectListItem { Text = parent.FirstName, Value = parent.Id.ToString(), Selected = false });
                }
                else
                { 
                    if (parent.Id == student.Parent.Id)
                        parentList.Add(new SelectListItem { Text = parent.FirstName, Value = parent.Id.ToString(), Selected = true });
                    else                 
                        parentList.Add(new SelectListItem { Text = parent.FirstName, Value = parent.Id.ToString(), Selected = false });
                }

            List<SelectListItem> primaryTutorList = new List<SelectListItem>
            {  
                new SelectListItem { Text = @"(none)", Value = "0", Selected = true }
            }; 
            var sortedUsers = _db.Users.OrderBy(u => u.LastName).ThenBy(u => u.FirstName).ToList();
            foreach (ApplicationUser user in sortedUsers)
                if (student.PrimaryTutor == null)
                {
                    primaryTutorList.Add(new SelectListItem { Text = user.LastName + @", " + user.FirstName, Value = user.Id, Selected = false });
                }
                else
                {
                    primaryTutorList.Add(user.Id == student.PrimaryTutor.Id
                        ? new SelectListItem
                        {
                            Text = user.LastName + @", " + user.FirstName, Value = user.Id, Selected = true
                        }
                        : new SelectListItem
                        {
                            Text = user.LastName + @", " + user.FirstName, Value = user.Id, Selected = false
                        });
                }

            List<SelectListItem> teacherList = new List<SelectListItem>();
            bool firstTeacher = true;                                                                                                       
            var sortedTeachers = _db.Teachers.OrderBy(p => p.LastName).ToList();
            foreach (Teacher teacher in sortedTeachers)
            {
                if (student.Teacher != null)
                {
                    if (firstTeacher)
                    {
                        teacherList.Add(new SelectListItem {Text = @"(none) ", Value = "0", Selected = false});
                        firstTeacher = false;
                    }

                    if (teacher.Id == student.Teacher.Id)
                        teacherList.Add(new SelectListItem
                        {
                            Text = teacher.LastName + @", " + teacher.FirstName,
                            Value = teacher.Id.ToString(),
                            Selected = true
                        });
                    else
                        teacherList.Add(new SelectListItem
                        {
                            Text = teacher.LastName + @", " + teacher.FirstName,
                            Value = teacher.Id.ToString(),
                            Selected = false
                        });
                }
                else
                {
                    if (firstTeacher)
                    {
                        teacherList.Add(new SelectListItem {Text = @"(none) ", Value = "0", Selected = true});
                        firstTeacher = false;
                    }

                    teacherList.Add(new SelectListItem
                    {
                        Text = teacher.LastName + @", " + teacher.FirstName,
                        Value = teacher.Id.ToString(),
                        Selected = false
                    });
                }
            }

            viewModel.Teachers = teacherList;
            viewModel.Schools = schoolList;
            viewModel.Id = student.Id;
            viewModel.Parents = parentList;
            viewModel.Schools = schoolList;
            viewModel.Users = primaryTutorList;
            viewModel.FirstName = student.FirstName;
            viewModel.Gender = student.Gender;
            viewModel.BirthDate = student.BirthDate;
            if (student.GradeLevel != null)
            {                                                                                                                    
                viewModel.GradeLevel = (int) student.GradeLevel;
            }                                                                                   
            viewModel.SpecialClass = student.SpecialClass;
            viewModel.Parent = student.Parent;
            viewModel.School = student.School;
            viewModel.PrimaryTutor = student.PrimaryTutor;
            return View(viewModel);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,Gender,BirthDate,GradeLevel,SpecialClass,School,Parent,PrimaryTutor,Teacher")] AddEditStudentViewModel viewModel) 
        {
            if (ModelState.IsValid)
            {
                var student = _db.Students.Find(viewModel.Id);
                if (student != null)
                {
                    student.FirstName = viewModel.FirstName;
                    student.Gender = viewModel.Gender;
                    student.BirthDate = viewModel.BirthDate;
                    student.GradeLevel = viewModel.GradeLevel;
                    student.SpecialClass = viewModel.SpecialClass;
                    if (viewModel.Parent != null)
                    {
                        student.Parent = (from p in _db.Parents where p.Id == viewModel.Parent.Id select p).Single();
                    }

                    if (viewModel.School != null)
                    {
                        student.School = (from s in _db.Schools where s.Id == viewModel.School.Id select s).Single();
                    }
                                                
                    if (viewModel.PrimaryTutor.Id != "0")
                    {
                        student.PrimaryTutor = (from t in _db.Users where t.Id == viewModel.PrimaryTutor.Id select t).Single();
                    }
                    else
                    {
                        // student.PrimaryTutor = null;   // this statement is not peformed by Entity Framework
                        var sqlString = "UPDATE Student Set PrimaryTutor_Id = NULL ";
                        sqlString += "WHERE Id =" + viewModel.Id;
                        using (var context = new SenecaContext())
                        {
                            context.Database.ExecuteSqlCommand(sqlString);
                        } 
                    }

                    if (viewModel.Teacher  != null)
                    {
                        if (viewModel.Teacher.Id == 0)
                        {
                            student.Teacher = null;
                        }
                        else
                        {
                            student.Teacher = (from t in _db.Teachers where t.Id == viewModel.Teacher.Id select t).Single();
                        }
                    }
                }

                _db.SaveChanges();
             
                return RedirectToAction("Index");
            }
            return View(viewModel);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = _db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {                                                                                                           
            var storageAccount = CloudStorageAccount.Parse(Properties.Settings.Default.StorageConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("studentreports");
            foreach (var report in _db.StudentReports.Where(r => r.Student.Id == id))
            {
                var blob = container.GetBlockBlobReference(report.DocumentLink);
                if (blob.Exists())
                {
                    blob.Delete();
                }     
            }
            _db.StudentReports.RemoveRange(_db.StudentReports.Where(r => r.Student.Id == id));
            _db.TutorNotes.RemoveRange(_db.TutorNotes.Where(t => t.Student.Id == id));          
            var student = _db.Students.Find(id);
            if (student != null) _db.Students.Remove(student);
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
