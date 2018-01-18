using MVC5_Seneca.EntityModels;
using System.Collections.Generic;
using System.Web.Mvc;
using MVC5_Seneca.ViewModels;
using System.ComponentModel;
using System;

namespace MVC5_Seneca.ViewModels
{
    public class AddEditUserRolesViewModel
    {
        public int Id { get; set; }
        public string Role { get; set; }        
        public virtual IEnumerable<SelectListItem> Users { get; set; }
        public virtual IEnumerable<SelectListItem> UserRoles { get; set; }       
        public virtual ApplicationIdentity User { get; set; }

    }
}