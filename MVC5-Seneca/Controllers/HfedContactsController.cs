using MVC5_Seneca.DataAccessLayer;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MVC5_Seneca.EntityModels;
using MVC5_Seneca.ViewModels;

namespace MVC5_Seneca.Controllers
{
    public class HfedContactsController : Controller
    {
        // GET: Contacts
        private readonly SenecaContext _db = new SenecaContext(); 
        public ActionResult Index()
        {
            var model = new ContactsDisplayViewModel();      
            var staffRoleId = (from r in _db.Roles where (r.Name == "HfedStaff") select r.Id).Single();
            var listStaff = new List<ApplicationUser>();

            var users = _db.Users.OrderBy( u => u.LastName).ToList();
            foreach (var user in users)
            {
                foreach (var role in user.Roles)
                {
                    if (role.RoleId == staffRoleId)
                    {
                        var staff = new ApplicationUser()
                        {
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Title = user.Title,
                            PhoneNumber = user.PhoneNumber,
                            Email = user.Email
                        };
                        listStaff.Add(staff);
                    }
                }
            }                                                                     
            model.Staff = listStaff;

            return View(model);
            }
        public ActionResult ReturnToDashboard()
        {
            return RedirectToAction("Index", "HfedHome");
        }
    }
}