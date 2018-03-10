using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MVC5_Seneca.DataAccessLayer;
using MVC5_Seneca.EntityModels;
using MVC5_Seneca.ViewModels;

namespace MVC5_Seneca.Controllers
{
    public class ContactsController : Controller
    {
        // GET: Contacts
        private readonly SenecaContext _db = new SenecaContext();

        public ActionResult Index()
        {
            ContactsDisplayViewModel model = new ContactsDisplayViewModel();
            var adminRoleId = (from r in _db.Roles where (r.Name == "Administrator") select r.Id).Single();
            var listAdministrators = new List<ApplicationUser>();
            var staffRoleId = (from r in _db.Roles where (r.Name == "Staff") select r.Id).Single();
            var listStaff = new List<Staff>();

            var users = _db.Users.ToList();
            foreach (var user in users)
            {
                foreach (var role in user.Roles)
                {                                                                                                                              
                    if (role.RoleId == adminRoleId)
                    {
                        listAdministrators.Add(user);
                    }

                    if (role.RoleId == staffRoleId)
                    {
                        var staff = new Staff()
                        {
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Title = user.Title,
                            WorkPhone = user.PhoneNumber,
                            CellPhone = user.PhoneNumber,
                            Email = user.Email
                        }; 
                        listStaff.Add(staff);
                    }
                }
            }
            
            // Already have 'Staff role' users in staffList; add db.Staff if not users:
            var staffMembers = _db.StaffMembers.ToList();
            foreach (var staffMember in staffMembers)
            {
                var usr = (from u in _db.Users where u.FirstName == staffMember.FirstName 
                                          && u.LastName == staffMember.LastName select u).Any();
                if (!usr)
                {
                 listStaff.Add(staffMember);
                }
            }

            model.Administrators = listAdministrators;
            model.Staff = listStaff;

            return View(model);
        }
        public ActionResult ReturnToDashboard()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}