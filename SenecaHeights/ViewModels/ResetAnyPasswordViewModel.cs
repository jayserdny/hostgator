using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC5_Seneca.EntityModels;
using Newtonsoft.Json;

namespace MVC5_Seneca.ViewModels
{
     public class ResetAnyPasswordViewModel
    {
       public List<SelectListItem> Users { get; set; }
       public ApplicationIdentity User { get; set; }
       public string Name { get; set; }   // UserName
       public string FirstName { get; set; }
       public string LastName { get; set; }
       public string Email { get; set; } 
        public string PhoneNumber { get; set; }
        public string Securitytamp { get; set; }
       public string NewPassword { get; set; }
    }
}