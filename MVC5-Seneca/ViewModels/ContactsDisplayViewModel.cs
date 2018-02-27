using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MVC5_Seneca.EntityModels;

namespace MVC5_Seneca.ViewModels
{
    public class ContactsDisplayViewModel
    {
        public virtual ICollection<ApplicationUser> Administrators { get; set; }

        public virtual ICollection <Staff> Staff { get; set; }
    }
}