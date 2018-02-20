using System;
using System.Collections.Generic;
using System.ComponentModel;
using MVC5_Seneca.EntityModels;
using System.Web.Mvc;

namespace MVC5_Seneca.ViewModels
{
    public class AddEditStudentViewModel
    {
        public int Id { get; set; } 

        // Input fields needed by the Create and Edit .cshtml files 
        public virtual IEnumerable<SelectListItem> Schools { get; set; }
        public virtual IEnumerable<SelectListItem> Parents { get; set; } 
         public virtual IEnumerable<SelectListItem> Users { get; set; }

        // Output fields used by the .cshtml file to return form results
        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("M/F")]
        public string Gender { get; set; }

        [DisplayName("Birthday")]
        public DateTime? BirthDate { get; set; }

        [DisplayName("School")]
        public virtual School School { get; set; }  

        [DisplayName("Parent")]
        public virtual Parent Parent { get; set; } 
        public virtual ICollection<StudentReport> StudentReports { get; set; }         
        public virtual ApplicationUser PrimaryTutor { get; set; }   
        public virtual string ErrorMessage { get; set; }
    }
}