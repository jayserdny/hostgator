using MVC5_Seneca.EntityModels;
using System.Collections.Generic;
using System.Web.Mvc;
using MVC5_Seneca.ViewModels;
using System.ComponentModel;
using System;
using MVC5_Seneca.Models;

namespace MVC5_Seneca.ViewModels
{
    public class AddEditUserRolesViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public virtual IEnumerable<SelectListItem> Users { get; set; }
        public virtual IEnumerable<SelectListItem> UserRoles { get; set; }
        public virtual ApplicationUser User { get; set; }   
        public virtual ApplicationRole UserRole { get; set;}
        public List <UserNameRole> UserNameRoles { get; set; }      
    }
    public class UserNameRole
    {
        public string Name { get; set; }
        public string Id { get; set; }   
    }
    


}