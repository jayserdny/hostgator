using System.Collections.Generic;
using MVC5_Seneca.EntityModels;

namespace MVC5_Seneca.ViewModels
{
    public sealed class ContactsDisplayViewModel
    {
        public ICollection<ApplicationUser> Administrators { get; set; }

        public ICollection <ApplicationUser> Staff { get; set; }
    }
}