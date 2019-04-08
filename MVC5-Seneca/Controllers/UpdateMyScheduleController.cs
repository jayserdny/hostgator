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
        public ActionResult Edit()
        {
            var userId = User.Identity.GetUserId();
            var user = (from u in _db.Users where u.Id.ToString() == userId select u).Single();
            UpdateMyScheduleViewModel model = new UpdateMyScheduleViewModel();
            model.Id = user.Id;
            model.FirstName = user.FirstName;
            model.LastName = user.LastName;
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