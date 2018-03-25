using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.EntityModels;
using MVC5_Seneca.ViewModels;

namespace MVC5_Seneca.Controllers
{
    public class TutorStudentController : Controller
    {
        private readonly SenecaContext _db = new SenecaContext();

        // GET: TutorStudent
        public ActionResult Index()
        {
            var viewModel = new List<TutorStudentViewModel>();

            var tutors = _db.Users.OrderBy(u => u.LastName).ToList();

            var roleStore = new RoleStore<IdentityRole>(_db);
            var roleMngr = new RoleManager<IdentityRole>(roleStore);
            string tutorRoleId = (from r in roleMngr.Roles where r.Name == "Tutor" select r.Id).ToString();
            var users = (from u in _db.Users.OrderBy(r => r.LastName)
                where u.Roles.Any(r => r.RoleId == tutorRoleId)
                select u).ToList();
            foreach (ApplicationUser tutor in tutors)
            {
                foreach (var role in tutor.Roles)
                {
                    var _role = (from r in _db.Roles where (r.Id == role.RoleId) select r).Single();
                    if (_role.Name == "Tutor")
                    {
                        var tutorStudents = new TutorStudentViewModel {Students = new List<Student>()};
                        foreach (Student student in _db.Students)
                        {
                            if (student.PrimaryTutor != null)
                            {
                                if (student.PrimaryTutor.Id == tutor.Id)
                                {
                                    tutorStudents.Students.Add(student);
                                }
                            }
                        }

                        if (tutorStudents.Students.Count() != 0)
                        {
                            tutorStudents.Tutor = (from u in _db.Users where (u.Id == tutor.Id) select u).Single();
                            viewModel.Add(tutorStudents);
                        }
                    }
                }
            } 
            return View(viewModel);
        } 
        public ActionResult ReturnToDashboard()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}