 using System.Collections.Generic;
 using System.Linq;
using System.Web.Mvc;
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

            foreach (ApplicationUser tutor in tutors)
            {
                foreach (var role in tutor.Roles)
                {
                    var identityRole = (from r in _db.Roles where (r.Id == role.RoleId) select r).Single();
                    if (identityRole.Name != "Tutor") continue;
                    var tutorStudents = new TutorStudentViewModel { PrimaryStudents = new List<Student>(), AssociateStudents = new List<Student>() };
                    foreach (Student student in _db.Students)
                    {
                        if (student.PrimaryTutor == null) continue;
                        if (student.PrimaryTutor.Id != tutor.Id) continue;
                        var count = _db.TutorNotes.OrderByDescending(n => n.Date).Where(n => n.Student.Id == student.Id && n.ApplicationUser.Id == tutor.Id).ToList();
                        student.PrimaryNoteCount = count.Count();
                        if (count.Count > 0)
                        {
                            student.LastPrimaryNoteDate = count[0].Date;
                        }
                        tutorStudents.PrimaryStudents.Add(student);
                    }
                    // Is this tutor an Associate Tutor for other students? 
                    var associateTuteeIds = (from t in _db.AssociateTutors where t.Tutor.Id == tutor.Id select t.Id).ToList();
                    foreach (int associateTuteeId in associateTuteeIds)
                    {
                        Student associateStudent = _db.Students.Find(associateTuteeId);
                        var count = _db.TutorNotes.OrderByDescending(n => n.Date).Where(n => n.Student.Id == associateStudent.Id && n.ApplicationUser.Id == tutor.Id).ToList();
                        if (associateStudent == null) continue;
                        associateStudent.AssociateNoteCount = count.Count();
                        if (count.Count > 0)
                        {
                            associateStudent.LastAssociateNoteDate = count[0].Date;
                        }
                        tutorStudents.AssociateStudents.Add(associateStudent);
                    }
                    if (!tutorStudents.PrimaryStudents.Any() && !tutorStudents.AssociateStudents.Any()) continue;
                    tutorStudents.Tutor = (from u in _db.Users where (u.Id == tutor.Id) select u).Single();
                    viewModel.Add(tutorStudents);
                    //} 
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