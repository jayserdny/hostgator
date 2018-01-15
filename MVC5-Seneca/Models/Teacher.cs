 using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MVC5_Seneca.Models;

namespace MVC5_Seneca.EntityModels
{
    public class Teacher
    {
        public int Id { get; set; }

        [DisplayName ("Last Name")]
        public string LastName { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        public School School { get; set; }

        [DisplayName("Work Phone")]
        public string WorkPhone { get; set; }

        [DisplayName("Cell Phone")]
        public string CellPhone { get; set; }

        public string Email { get; set; }  
    }
}