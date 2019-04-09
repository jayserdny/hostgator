using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.EntityModels;
using MVC5_Seneca.ViewModels;

namespace MVC5_Seneca.Controllers
{
    public class UpdateMyScheduleController : Controller
    {
        private readonly SenecaContext _db = new SenecaContext();
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var user = (from u in _db.Users where u.Id == userId select u).Single();
            //var tutorStudents = new UpdateMyScheduleViewModel {MyTutees = new List<Student>()};
            UpdateMyScheduleViewModel model = new UpdateMyScheduleViewModel {MyTutees = new List<Student>()};
            model.Id = user.Id;
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
             
            foreach (Student student in _db.Students)
            {
                if (student.PrimaryTutor == null) continue;
                if (student.PrimaryTutor.Id != user.Id) continue;
                model.MyTutees.Add(student);
            }
            // Is this tutor an Associate Tutor for other students? 
            var associateTuteeIds = (from t in _db.AssociateTutors where t.Tutor.Id == user.Id select t.Student.Id).ToList();
            foreach (int associateTuteeId in associateTuteeIds)
            {
                Student associateStudent = _db.Students.Find(associateTuteeId);
                if (associateStudent == null) continue;
                model.MyTutees.Add(associateStudent );
            }

            List<SelectListItem> studentList = new List<SelectListItem>();
            var sortedStudents = _db.Students.OrderBy(s => s.FirstName);
            model.Students = sortedStudents.Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.FirstName
                })
                .ToList();
  
            return View(model);
    }

    // POST: UpdateMySchedule/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Title,PhoneNumber,Email")] UpdateMyScheduleViewModel viewModel)
    {
    if (ModelState.IsValid)
        {
        var user = _db.Users.Find(viewModel.Id);
            if (user != null)
            {
                user.FirstName = viewModel.FirstName;
                user.LastName = viewModel.LastName;
              

                _db.SaveChanges();
                return RedirectToAction("Index", "Home");
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