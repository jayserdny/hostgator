using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC5_Seneca.EntityModels;

namespace MVC5_Seneca.Models
{
    public class UserRole
    {
        public int Id { get; set; }
        public string Role { get; set; } 
        public virtual UserDetail User { get; set; }        
    }
}